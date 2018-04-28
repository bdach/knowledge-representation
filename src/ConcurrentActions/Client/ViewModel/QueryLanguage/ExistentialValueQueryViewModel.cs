using System;
using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Client.ViewModel.Terminal;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    public class ExistentialValueQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<ExistentialValueQuery>, IGalleryItem
    {
        public static string LabelLeft => "possibly";

        public static string LabelRight => "after";

        public string DisplayName => $"[ ] {LabelLeft} [ ] {LabelRight} [ ]";

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; protected set; }

        public ExistentialValueQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new NotImplementedException());
        }

        public ExistentialValueQuery ToModel()
        {
            throw new NotImplementedException();
        }
    }
}