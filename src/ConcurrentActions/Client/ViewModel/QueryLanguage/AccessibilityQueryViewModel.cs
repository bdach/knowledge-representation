﻿using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Global;
using Client.Interface;
using Client.View.QueryLanguage;
using Client.ViewModel.Terminal;
using DynamicSystem;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    /// <summary>
    /// View model for <see cref="AccessibilityQueryView"/> which represents an accessibility query in the scenario.
    /// </summary>
    public class AccessibilityQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<AccessibilityQuery>
    {
        /// <summary>
        /// Keyword describing the query.
        /// </summary>
        public string Label => "accessible";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "AccessibilityQuery";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a target formula.
        /// </summary>
        public IFormulaViewModel Target { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; protected set; }

        /// <summary>
        /// Command adding a new empty compound action.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddEmptyCompoundAction { get; protected set; }

        /// <inheritdoc />
        public ReactiveCommand<ActionViewModel, ActionViewModel> AddAtomicAction { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public bool AnyChildFocused => IsFocused || Target.AnyChildFocused;

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="AccessibilityQueryViewModel"/> instance.
        /// </summary>
        public AccessibilityQueryViewModel()
        {
            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);

            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.Target.AddFormula);

            AddEmptyCompoundAction = ReactiveCommand.Create(() => Unit.Default);
            this.WhenAnyObservable(vm => vm.AddEmptyCompoundAction)
                .Where(_ => AnyChildFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddCompoundActionError"));

            AddAtomicAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(action => action);
            this.WhenAnyObservable(vm => vm.AddAtomicAction)
                .Where(_ => AnyChildFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddActionError"));

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Target.IsFocused)
                .Subscribe(_ => Target = new PlaceholderViewModel());

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !Target.IsFocused)
                .InvokeCommand(this, vm => vm.Target.DeleteFocused);
        }

        /// <summary>
        /// Function used to handle formula insertion based on window focus.
        /// </summary>
        /// <param name="formula">The <see cref="IFormulaViewModel"/> instance to be inserted.</param>
        private void InsertFormula(IFormulaViewModel formula)
        {
            if (Target.IsFocused)
            {
                Target = formula.Accept(Target);
            }
        }

        /// <inheritdoc />
        public IQueryClauseViewModel NewInstance()
        {
            return new AccessibilityQueryViewModel();
        }

        /// <inheritdoc />
        public bool? Result { get; set; }

        /// <inheritdoc />
        public void AcceptResult(QueryResolution results)
        {
            Result = results.AccessibilityQueryResults.First().Item2;
            results.AccessibilityQueryResults.RemoveAt(0);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the underlying query model out of the view model.
        /// </summary>
        /// <returns><see cref="AccessibilityQuery"/> model represented by given view model.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        public AccessibilityQuery ToModel()
        {
            var target = Target?.ToModel();

            if (target == null)
                throw new MemberNotDefinedException("AccessibilityQueryTargetError");

            return new AccessibilityQuery(target);
        }
    }
}