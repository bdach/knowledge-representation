using System.Reactive;
using Client.Abstract;
using Client.View;
using ReactiveUI;
using Splat;

namespace Client.ViewModel
{
    /// <inheritdoc />
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
            CloseWindow = ReactiveCommand.Create(() => Unit.Default);
        }
    }
}