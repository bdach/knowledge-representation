using Client.Abstract;
using ReactiveUI;
using Splat;

namespace Client.Providers
{
    /// <summary>
    /// Class responsible for app initialization and dependency injection setup.
    /// </summary>
    public class AppBootstrapper : FodyReactiveObject, IScreen
    {
        /// <summary>
        /// Router instance used within the application.
        /// </summary>
        public RoutingState Router { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AppBootstrapper()
        {
            Router = new RoutingState();
            
            Locator.CurrentMutable.RegisterLazySingleton(() => this, typeof(IScreen));
        }
    }
}