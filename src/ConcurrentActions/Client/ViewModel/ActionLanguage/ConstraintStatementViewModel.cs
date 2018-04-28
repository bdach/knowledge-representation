using System;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.ViewModel.Terminal;
using Model.ActionLanguage;
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    public class ConstraintStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<ConstraintStatement>, IGalleryItem
    {
        public static string Label => "always";

        public string DisplayName => $"{Label} [ ]";

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        public ConstraintStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotApplicableException("Constraint statement does not support adding actions"));

            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        public ConstraintStatement ToModel()
        {
            throw new NotImplementedException();
        }
    }
}