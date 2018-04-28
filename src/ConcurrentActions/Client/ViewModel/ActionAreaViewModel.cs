using System.Collections.ObjectModel;
using Client.Abstract;
using Client.Interface;
using Client.View;

namespace Client.ViewModel
{
    /// <inheritdoc />
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
        /// Initializes a new <see cref="ActionAreaViewModel"/> instance.
        /// </summary>
        public ActionAreaViewModel()
        {
            ActionDomain = new ObservableCollection<IActionClauseViewModel>();
        }
    }
}