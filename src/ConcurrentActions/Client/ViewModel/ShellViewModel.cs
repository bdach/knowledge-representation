using Client.Abstract;
using Client.View;

namespace Client.ViewModel
{
    /// <summary>
    /// ViewModel for <see cref="ShellView"/> which is the root view of the application.
    /// </summary>
    public class ShellViewModel : FodyReactiveObject
    {
        /// <summary>
        /// ViewModel of nested <see cref="RibbonView"/>.
        /// </summary>
        public RibbonViewModel RibbonViewModel { get; set; }

        /// <summary>
        /// ViewModel of nested <see cref="ActionAreaView"/>.
        /// </summary>
        public ActionAreaViewModel ActionAreaViewModel { get; set; }

        /// <summary>
        /// ViewModel of nested <see cref="QueryAreaView"/>.
        /// </summary>
        public QueryAreaViewModel QueryAreaViewModel { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShellViewModel()
        {
            RibbonViewModel = new RibbonViewModel();
            ActionAreaViewModel = new ActionAreaViewModel();
            QueryAreaViewModel = new QueryAreaViewModel();
        }
    }
}