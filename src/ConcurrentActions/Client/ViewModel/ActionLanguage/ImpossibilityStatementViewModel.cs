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
    public class ImpossibilityStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<EffectStatement>, IGalleryItem
    {
        public static string Label => "imppossible";

        public string DisplayName => $"{Label} [ ]";

        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ImpossibilityStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotApplicableException("Impossibility statement does not support adding actions"));

            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        public EffectStatement ToModel()
        {
            throw new NotImplementedException();
        }
    }
}