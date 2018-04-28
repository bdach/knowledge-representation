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
    public class InitialValueStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<InitialValueStatement>, IGalleryItem
    {
        public static string Label => "initially";

        public string DisplayName => $"{Label} [ ]";

        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public InitialValueStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotApplicableException("Initial value statement does not support adding actions"));

            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        public InitialValueStatement ToModel()
        {
            throw new NotImplementedException();
        }
    }
}