using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.ViewModel.Terminal;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    public class ExistentialExecutabilityQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<ExistentialExecutabilityQuery>, IGalleryItem
    {
        public static string Label => "executable sometimes";

        public string DisplayName => $"{Label} [ ]";

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; protected set; }


        public ExistentialExecutabilityQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotApplicableException("Existential executability query does not support adding formulae"));

            AddProgram = ReactiveCommand.Create<ProgramViewModel>(programViewModel =>
                throw new System.NotImplementedException());
        }

        public ExistentialExecutabilityQuery ToModel()
        {
            throw new System.NotImplementedException();
        }
    }
}