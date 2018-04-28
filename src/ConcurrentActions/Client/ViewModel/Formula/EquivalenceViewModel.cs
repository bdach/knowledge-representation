using System;
using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.Formula
{
    public class EquivalenceViewModel : FodyReactiveObject, IFormulaViewModel, IViewModelFor<Equivalence>
    {
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public EquivalenceViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        public Equivalence ToModel()
        {
            throw new NotImplementedException();
        }
    }
}