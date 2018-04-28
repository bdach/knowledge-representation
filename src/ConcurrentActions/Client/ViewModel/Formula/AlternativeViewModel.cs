using System;
using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.Formula
{
    public class AlternativeViewModel : FodyReactiveObject, IFormulaViewModel, IViewModelFor<Alternative>
    {
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public AlternativeViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        public Alternative ToModel()
        {
            throw new NotImplementedException();
        }
    }
}