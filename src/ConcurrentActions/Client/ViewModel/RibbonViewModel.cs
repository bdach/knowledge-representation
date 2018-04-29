using System.Reactive;
using Client.Abstract;
using Client.Provider;
using Client.View;
using ReactiveUI;

namespace Client.ViewModel
{
    /// <inheritdoc />
    /// <summary>
    /// ViewModel for <see cref="RibbonView"/> which coordinates all actions within the application.
    /// </summary>
    public class RibbonViewModel : FodyReactiveObject
    {
        /// <summary>
        /// Command closing the application window.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CloseWindow { get; protected set; }

        /// <summary>
        /// Command changing the application language to English.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SetEnglishLocale { get; protected set; }

        /// <summary>
        /// Command changing the application language to Polish.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SetPolishLocale { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="RibbonViewModel"/> instance.
        /// </summary>
        public RibbonViewModel()
        {
            CloseWindow = ReactiveCommand.Create(() => Unit.Default);
            SetEnglishLocale = ReactiveCommand.Create(() => LocalizationProvider.SetLocale(LocalizationProvider.AmericanEnglish));
            SetPolishLocale = ReactiveCommand.Create(() => LocalizationProvider.SetLocale(LocalizationProvider.Polish));
        }
    }
}