using System;
using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Client.ViewModel.Terminal;
using Model.ActionLanguage;
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    public class ObservationStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<ObservationStatement>, IGalleryItem
    {
        public static string LabelLeft => "observable";

        public static string LabelRight => "after";

        public string DisplayName => $"{LabelLeft} [ ] {LabelRight} [ ]";

        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ObservationStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotImplementedException());

            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        public ObservationStatement ToModel()
        {
            throw new NotImplementedException();
        }
    }
}