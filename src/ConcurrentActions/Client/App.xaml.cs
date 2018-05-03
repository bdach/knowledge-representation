using System.Windows;
using Client.AppStart;
using Client.View;
using Client.ViewModel;
using ReactiveUI;
using Splat;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Instance of <see cref="AppBootstrapper"/> class used to manage
        /// the initialization of the application and dependency injections.
        /// </summary>
        public static AppBootstrapper Bootstrapper { get; protected set; }

        /// <summary>
        /// Instance of <see cref="ShellView"/> which is the root view
        /// of the application.
        /// </summary>
        public static ShellView ShellView { get; protected set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public App()
        {
            Bootstrapper = new AppBootstrapper();
        }

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ShellView = (ShellView)Locator.Current.GetService<IViewFor<ShellViewModel>>();
            ShellView.Show();

            // TODO: remove this if it's not going to be useful
            // fix for multiple notifications firing ReactiveList.AddRange
            RxApp.SupportsRangeNotifications = false;
        }
    }
}
