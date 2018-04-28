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
    public class FluentSpecificationStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<FluentSpecificationStatement>, IGalleryItem
    {
        public static string Label => "noninertial";

        public string DisplayName => $"{Label} [ ]";

        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public FluentSpecificationStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotApplicableException("Fluent specification statement does not support adding actions"));

            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        public FluentSpecificationStatement ToModel()
        {
            throw new NotImplementedException();
        }
    }
}