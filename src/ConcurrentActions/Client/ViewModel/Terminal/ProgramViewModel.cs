using Client.Abstract;
using Client.Interface;
using Model;

namespace Client.ViewModel.Terminal
{
    public class ProgramViewModel : FodyReactiveObject, IViewModelFor<Program>
    {
        public ProgramViewModel()
        {
            
        }

        public Program ToModel()
        {
            throw new System.NotImplementedException();
        }
    }
}