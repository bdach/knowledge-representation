using System.Reactive;
using Client.Abstract;
using Client.View;
using ReactiveUI;
using Splat;

namespace Client.ViewModel
{
    /// <summary>
    /// ViewModel for <see cref="RibbonView"/> which coordinates all actions within the application.
    /// </summary>
    public class RibbonViewModel : FodyReactiveObject
    {
        /// <summary>
        /// Command to close the application window.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CloseWindow { get; protected set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RibbonViewModel()
        {
            CloseWindow = ReactiveCommand.Create(
                () => Locator.Current.GetService<ShellView>().ShellWindow.Close() // TODO: wywala się jak siemano
                //System.Windows.Application.Current.Shutdown();
            );
        }
    }
}