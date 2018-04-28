using System.Collections.ObjectModel;
using Client.Abstract;
using Client.Interface;
using Client.View;

namespace Client.ViewModel
{
    /// <summary>
    /// View model for <see cref="ActionAreaView"/> which manages the list of actions.
    /// </summary>
    public class ActionAreaViewModel : FodyReactiveObject
    {
        /// <summary>
        /// Collection containing all Action Language clauses.
        /// </summary>
        public ObservableCollection<IActionClauseViewModel> ActionDomain { get; protected set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ActionAreaViewModel()
        {
            ActionDomain = new ObservableCollection<IActionClauseViewModel>();
        }
    }
}