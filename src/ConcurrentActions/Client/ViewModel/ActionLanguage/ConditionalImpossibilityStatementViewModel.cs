﻿using System;
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
    /// View model for <see cref="ConditionalImpossibilityStatementView"/> which represents a conditional impossibility statement in the scenario.
    /// </summary>
    public class ConditionalImpossibilityStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<EffectStatement>, IGalleryItem
    {
        /// <summary>
        /// First keyword describing the clause.
        /// </summary>
        public static string LabelLeft => "imppossible";

        /// <summary>
        /// Second keyword describing the query.
        /// </summary>
        public static string LabelRight => "if";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{LabelLeft} [ ] {LabelRight} [ ]";

        /// <summary>
        /// The <see cref="ActionViewModel"/> instance.
        /// </summary>
        public ActionViewModel Action { get; set; }

        /// <summary>
        /// The precondition <see cref="IFormulaViewModel"/> instance.
        /// </summary>
        public IFormulaViewModel Precondition { get; set; }

        /// <summary>
        /// Command adding a new action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ConditionalImpossibilityStatementViewModel"/> instance.
        /// </summary>
        public ConditionalImpossibilityStatementViewModel()
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
                throw new MemberNotDefinedException("Action in a conditional impossibility statement is not defined");
            if (Precondition == null)
                throw new MemberNotDefinedException("Precondtition in a conditional impossibility statement is not defined");

            return new EffectStatement(Action.ToModel(), Precondition.ToModel(), Constant.Falsity);
        }
    }
}