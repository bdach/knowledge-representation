using System.Reactive;
using Client.Abstract;
using Client.Global;
using Client.Provider;
using Client.View;
using Client.View.Modal;
using Client.ViewModel.Modal;
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
        /// A global container instance holding the fluents currently in the scenario.
        /// </summary>
        public ScenarioContainer ScenarioContainer { get; }

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
        /// Command displaying the modal allowing the user to add a fluent to the current scenario.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowAddFluentModal { get; protected set; }

        /// <summary>
        /// Command displaying the modal allowing the user to add an action to the current scenario.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowAddActionModal { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="RibbonViewModel"/> instance.
        /// </summary>
        /// <param name="scenarioContainer">Scenario container with the current language signature.</param>
        /// <remarks>
        /// Omitting the <see cref="scenarioContainer"/> parameter causes the method to fetch the instance registered in the <see cref="Locator"/>.
        /// </remarks>
        public RibbonViewModel(ScenarioContainer scenarioContainer = null)
        {
            ScenarioContainer = scenarioContainer ?? Locator.Current.GetService<ScenarioContainer>();

            CloseWindow = ReactiveCommand.Create(() => Unit.Default);
            SetEnglishLocale = ReactiveCommand.Create(() => LocalizationProvider.SetLocale(LocalizationProvider.AmericanEnglish));
            SetPolishLocale = ReactiveCommand.Create(() => LocalizationProvider.SetLocale(LocalizationProvider.Polish));

            ShowAddFluentModal = ReactiveCommand.Create(() =>
            {
                var modalView = (FluentModalView)Locator.Current.GetService<IViewFor<FluentModalViewModel>>();
                modalView.ShowDialog();
            });
            ShowAddActionModal = ReactiveCommand.Create(() =>
            {
                var modalView = (ActionModalView)Locator.Current.GetService<IViewFor<ActionModalViewModel>>();
                modalView.ShowDialog();
            });
        }
    }
}