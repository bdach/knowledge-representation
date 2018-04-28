using Client.Abstract;
using Client.Interface;
using Model;

namespace Client.ViewModel.Terminal
{
    public class CompoundActionViewModel : FodyReactiveObject, IViewModelFor<CompoundAction>
    {
        public CompoundActionViewModel()
        {
            
        }
        public CompoundAction ToModel()
        {
            throw new System.NotImplementedException();
        }
    }
}