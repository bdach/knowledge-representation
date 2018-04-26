using System.Globalization;
using Client.Abstract;
using Client.View;
using Client.ViewModel;
using Fluent;
using Fluent.Localization.Languages;
using ReactiveUI;
using Splat;

namespace Client.Provider
{
    /// <summary>
    /// Class responsible for app initialization and dependency injection setup.
    /// </summary>
    public class AppBootstrapper : FodyReactiveObject
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AppBootstrapper()
        {
            RibbonLocalization.Current.Culture = new CultureInfo("pl-PL", false); // TODO: change this if something becomes annoying AF (e.g. commas in doubles/decimals)
            RibbonLocalization.Current.Localization = new Polish();

            // TODO: we might not need this since there's no routing in the application
            //Locator.CurrentMutable.RegisterLazySingleton(() => this, typeof(IScreen)); 

            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellViewModel(), typeof(ShellViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellView(Locator.Current.GetService<ShellViewModel>()), typeof(IViewFor<ShellViewModel>));

            // TODO: switch the following to assembly search agaist IViewFor<> (excluding ShellViewModel)
            Locator.CurrentMutable.Register(() => new RibbonView(), typeof(IViewFor<RibbonViewModel>));
            Locator.CurrentMutable.Register(() => new ActionAreaView(), typeof(IViewFor<ActionAreaViewModel>));
            Locator.CurrentMutable.Register(() => new QueryAreaView(), typeof(IViewFor<QueryAreaViewModel>));
        }
    }
}