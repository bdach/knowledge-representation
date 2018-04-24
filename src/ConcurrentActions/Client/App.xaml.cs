using System.Windows;
using Client.Providers;
using ReactiveUI;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Instance of <see cref="AppBootstrapper"/> class used to manage
        /// the initialization of the application and dependency injections.
        /// </summary>
        public static AppBootstrapper Bootstrapper { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public App()
        {
            Bootstrapper = new AppBootstrapper();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // TODO: remove this if it's not going to be useful
            // Fix for multiple notifications firing ReactiveList.AddRange
            RxApp.SupportsRangeNotifications = false;
        }
    }
}
