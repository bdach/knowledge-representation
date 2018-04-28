using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.Formula
{
    public class LiteralViewModel : FodyReactiveObject, IFormulaViewModel, IViewModelFor<Literal>
    {
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        public LiteralViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotApplicableException("Literal does not support adding formulae"));
        }

        public Literal ToModel()
        {
            throw new System.NotImplementedException();
        }
    }
}