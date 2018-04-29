using System;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.ActionLanguage;
using Client.ViewModel.Terminal;
using Model.ActionLanguage;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    /// <summary>
    /// View model for <see cref="UnconditionalEffectStatementView"/> which represents an unconditional effect statement in the scenario.
    /// </summary>
    public class UnconditionalEffectStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<EffectStatement>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "causes";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"[ ] {Label} [ ]";

        /// <summary>
        /// The <see cref="ActionViewModel"/> instance.
        /// </summary>
        public ActionViewModel Action { get; set; }

        /// <summary>
        /// The postcondition <see cref="IFormulaViewModel"/> instance.
        /// </summary>
        public IFormulaViewModel Postcondition { get; set; }

        /// <summary>
        /// Command adding a new action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="UnconditionalEffectStatementViewModel"/> instance.
        /// </summary>
        public UnconditionalEffectStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotImplementedException());

            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="EffectStatement"/> model represented by given view model.</returns>
        public EffectStatement ToModel()
        {
            if (Action == null)
                throw new MemberNotDefinedException("Action in an unconditional effect statement is not defined");
            if (Postcondition == null)
                throw new MemberNotDefinedException("Postcondition in an unconditional effect statement is not defined");

            return new EffectStatement(Action.ToModel(), Constant.Truth, Postcondition.ToModel());
        }
    }
}