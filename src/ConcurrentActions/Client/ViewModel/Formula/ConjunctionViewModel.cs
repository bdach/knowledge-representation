using System;
using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.Formula
{
    public class ConjunctionViewModel : FodyReactiveObject, IFormulaViewModel, IViewModelFor<Conjunction>
    {
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ConjunctionViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        public Conjunction ToModel()
        {
            throw new NotImplementedException();
        }
    }
}