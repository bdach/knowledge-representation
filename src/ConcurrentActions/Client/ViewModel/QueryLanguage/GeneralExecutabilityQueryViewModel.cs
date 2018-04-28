using System;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.ViewModel.Terminal;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    public class GeneralExecutabilityQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<GeneralExecutabilityQuery>, IGalleryItem
    {
        public static string Label => "executable always";

        public string DisplayName => $"{Label} [ ]";

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; protected set; }

        public GeneralExecutabilityQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotApplicableException("General executability query does not support adding formulae"));

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new NotImplementedException());
        }

        public GeneralExecutabilityQuery ToModel()
        {
            throw new NotImplementedException();
        }
    }
}