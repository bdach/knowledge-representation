using System;
using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Client.ViewModel.Terminal;
using Model.ActionLanguage;
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    public class UnconditionalEffectStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<EffectStatement>, IGalleryItem
    {
        public static string Label => "causes";

        public string DisplayName => $"[ ] {Label} [ ]";

        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public UnconditionalEffectStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotImplementedException());

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