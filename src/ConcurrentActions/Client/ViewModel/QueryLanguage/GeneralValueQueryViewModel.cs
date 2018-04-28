using System;
using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Client.ViewModel.Terminal;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    public class GeneralValueQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<GeneralValueQuery>, IGalleryItem
    {
        public static string LabelLeft => "necessary";

        public static string LabelRight => "after";

        public string DisplayName => $"[ ] {LabelLeft} [ ] {LabelRight}";

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; protected set; }

        public GeneralValueQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new NotImplementedException());
        }

        public GeneralValueQuery ToModel()
        {
            throw new NotImplementedException();
        }
    }
}