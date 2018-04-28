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
    public class AccessibilityQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<AccessibilityQuery>, IGalleryItem
    {
        public static string Label => "accessible";

        public string DisplayName => $"{Label} [ ]";

        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; protected set; }

        public AccessibilityQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel => 
                    throw new NotImplementedException());

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new NotApplicableException("Accessibility query does not support adding programs"));
        }

        public AccessibilityQuery ToModel()
        {
            throw new NotImplementedException();
        }
    }
}