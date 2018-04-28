using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.Formula
{
    public class ConstantViewModel : FodyReactiveObject, IFormulaViewModel, IViewModelFor<Constant>
    {
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public ConstantViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotApplicableException("Constant does not support adding formulae"));
        }

        public Constant ToModel()
        {
            throw new System.NotImplementedException();
        }
    }
}