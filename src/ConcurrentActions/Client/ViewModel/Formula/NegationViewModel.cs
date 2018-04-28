using System;
using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.Formula
{
    public class NegationViewModel : FodyReactiveObject, IFormulaViewModel, IViewModelFor<Negation>
    {
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public NegationViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        public Negation ToModel()
        {
            throw new NotImplementedException();
        }
    }
}