using System;
using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Client.ViewModel.Terminal;
using Model.ActionLanguage;
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    public class ConditionalEffectStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<EffectStatement>, IGalleryItem
    {
        public static string LabelLeft => "causes";

        public static string LabelRight => "if";

        public string DisplayName => $"[ ] {LabelLeft} [ ] {LabelRight} [ ]";

        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ConditionalEffectStatementViewModel()
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