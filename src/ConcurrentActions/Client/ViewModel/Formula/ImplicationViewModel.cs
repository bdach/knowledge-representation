using System;
using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.Formula
{
    public class ImplicationViewModel : FodyReactiveObject, IFormulaViewModel, IViewModelFor<Implication>
    {
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ImplicationViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        public Implication ToModel()
        {
            throw new NotImplementedException();
        }
    }
}