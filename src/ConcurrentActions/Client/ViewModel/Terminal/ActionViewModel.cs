using Client.Abstract;
using Client.Interface;
using Model;

namespace Client.ViewModel.Terminal
{
    public class ActionViewModel : FodyReactiveObject, IViewModelFor<Action>
    {
        public ActionViewModel()
        {
            

        }
        public Action ToModel()
        {
            throw new System.NotImplementedException();
        }
    }
}