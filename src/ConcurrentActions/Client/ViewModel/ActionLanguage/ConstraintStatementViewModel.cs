using System;
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
    /// View model for <see cref="ConditionalEffectStatementView"/> which represents a constraint statement in the scenario.
    /// </summary>
    public class ConstraintStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<ConstraintStatement>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "always";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{Label} [ ]";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "ConstraintStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a constraint.
        /// </summary>
        public IFormulaViewModel Constraint { get; set; } = new PlaceholderViewModel();

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
        /// Initializes a new <see cref="ConstraintStatementViewModel"/> instance.
        /// </summary>
        public ConstraintStatementViewModel()
        {
            // TODO: display error?
            AddAction = ReactiveCommand.Create<ActionViewModel>(action => {});

            AddFluent = ReactiveCommand.Create<LiteralViewModel>(fluent => { });

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(
                InsertFormula,
                null,
                RxApp.MainThreadScheduler
            );
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(form => form != null)
                .InvokeCommand(this, vm => vm.Constraint.AddFormula);
        }

        private IFormulaViewModel InsertFormula(IFormulaViewModel formula)
        {
            if (!Constraint.IsFocused)
            {
                return formula;
            }
            Constraint = formula.Accept(Constraint);
            return null;
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new ConstraintStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="ConstraintStatement"/> model represented by given view model.</returns>
        public ConstraintStatement ToModel()
        {
            var constraint = Constraint?.ToModel();

            if (constraint == null)
                throw new MemberNotDefinedException("Constraint condition in a constraint statement is not defined");

            return new ConstraintStatement(constraint);
        }
    }
}