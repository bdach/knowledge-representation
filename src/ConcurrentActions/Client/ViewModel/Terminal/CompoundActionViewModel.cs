﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Global;
using Client.Interface;
using Client.View.Terminal;
using Model;
using ReactiveUI;

namespace Client.ViewModel.Terminal
{
    /// <summary>
    /// View model for <see cref="CompoundActionView"/> which represents a compound action from the language signature.
    /// </summary>
    public class CompoundActionViewModel : FodyReactiveObject, IViewModelFor<CompoundAction>, IDisposable
    {
        /// <summary>
        /// List of actions which are part of this compound action.
        /// </summary>
        public ReactiveList<ActionViewModel> Actions { get; set; }

        /// <summary>
        /// Adds an atomic action to this compound action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, ActionViewModel> AddAtomicAction { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <summary>
        /// Determines whether any child of this view model has focus.
        /// </summary>
        public bool AnyChildFocused => IsFocused || Actions.Any(a => a.IsFocused);

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <inheritdoc />
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; protected set; }

        /// <summary>
        /// This list keeps track of which commands higher in the hierarchy are piped into this view model.
        /// They are saved here so as to dispose of them when the view model gets deleted and falls out of scope.
        /// </summary>
        public List<IDisposable> CommandInvocationListeners { get; set; } = new List<IDisposable>();

        /// <summary>
        /// Initializes a new <see cref="CompoundActionViewModel"/> instance.
        /// </summary>
        public CompoundActionViewModel()
        {
            Actions = new ReactiveList<ActionViewModel>();

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => IsFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddFormulaError"));

            InitializeComponent();
        }

        /// <summary>
        /// Private method which contains instructions common to all constructors.
        /// </summary>
        private void InitializeComponent()
        {
            AddAtomicAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(action => action);
            this.WhenAnyObservable(vm => vm.AddAtomicAction)
                .Where(_ => IsFocused)
                .Where(action => action != null && !Actions.Any(other => other.Action.Equals(action.Action)))
                .Subscribe(action => Actions.Add(new ActionViewModel(action.Action)));

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Select(_ => Actions.SingleOrDefault(action => action.IsFocused))
                .Where(action => action != null)
                .Subscribe(action => Actions.Remove(action));
        }

        /// <summary>
        /// Gets the underlying compound action model out of the view model.
        /// </summary>
        /// <returns><see cref="Model.CompoundAction"/> model represented by given view model.</returns>
        public CompoundAction ToModel()
        {
            var actions = Actions.Select(avm => avm.ToModel()).ToList();
            if (actions.Any(a => a == null))
            {
                throw new MemberNotDefinedException("One of the actions in this compound action has not been defined");
            }
            return new CompoundAction(actions);
        }

        public void Dispose()
        {
            DeleteFocused?.Dispose();
            AddAtomicAction?.Dispose();
            foreach (var commandInvocationListener in CommandInvocationListeners)
            {
                commandInvocationListener?.Dispose();
            }
        }
    }
}