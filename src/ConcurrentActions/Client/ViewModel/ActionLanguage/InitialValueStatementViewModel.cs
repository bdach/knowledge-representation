﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.ActionLanguage;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model.ActionLanguage;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    /// <summary>
    /// View model for <see cref="InitialValueStatementView"/> which represents an initial value statement in the scenario.
    /// </summary>
    public class InitialValueStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<InitialValueStatement>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "initially";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{Label} [ ]";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "InitialValueStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning an initial condition.
        /// </summary>
        public IFormulaViewModel InitialCondition { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new fluent.
        /// </summary>
        public ReactiveCommand<LiteralViewModel, Unit> AddFluent { get; protected set; }

        /// <summary>
        /// Command adding a new action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <summary>
        /// Initializes a new <see cref="InitialValueStatementViewModel"/> instance.
        /// </summary>
        public InitialValueStatementViewModel()
        {
            // TODO: display error?
            AddAction = ReactiveCommand.Create<ActionViewModel>(action => { });

            AddFluent = ReactiveCommand.Create<LiteralViewModel>(fluent => { });

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(
                InsertFormula,
                null,
                RxApp.MainThreadScheduler
            );
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(form => form != null)
                .InvokeCommand(this, vm => vm.InitialCondition.AddFormula);
        }

        private IFormulaViewModel InsertFormula(IFormulaViewModel formula)
        {
            if (!InitialCondition.IsFocused)
            {
                return formula;
            }
            InitialCondition = formula.Accept(InitialCondition);
            return null;
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new InitialValueStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="InitialValueStatement"/> model represented by given view model.</returns>
        public InitialValueStatement ToModel()
        {
            var initialCondition = InitialCondition?.ToModel();

            if (initialCondition == null)
                throw new MemberNotDefinedException("Initial condition in an initial value statement is not defined");

            return new InitialValueStatement(initialCondition);
        }
    }
}