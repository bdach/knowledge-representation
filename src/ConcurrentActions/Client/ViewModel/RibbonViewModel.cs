using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Client.Abstract;
using Client.Global;
using Client.Interface;
using Client.Provider;
using Client.View;
using Client.View.Modal;
using Client.ViewModel.Formula;
using Client.ViewModel.Modal;
using Client.ViewModel.Terminal;
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
        /// A global container instance holding the current language signature.
        /// </summary>
        public LanguageSignature LanguageSignature { get; }

        #region General commands

        /// <summary>
        /// Command used to clear current language signature.
        /// </summary>
        public ReactiveCommand<Unit, Unit> Clear { get; protected set; }

        /// <summary>
        /// Command closing the application window.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CloseWindow { get; protected set; }

        /// <summary>
        /// Command changing the application language to English.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SetEnglishLocale { get; protected set; }

        // TODO: consider changing locale commands to one parameter-based
        /// <summary>
        /// Command changing the application language to Polish.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SetPolishLocale { get; protected set; }

        /// <summary>
        /// Command used to trigger scenario evaluation.
        /// </summary>
        public ReactiveCommand<Unit, Unit> PerformCalculations { get; protected set; }

        /// <summary>
        /// Command used to trigger export of current scenario to file.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ExportToFile { get; protected set; }

        #endregion

        #region Modal-related commands

        /// <summary>
        /// Command displaying the modal allowing the user to add a fluent to the current scenario.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowAddFluentModal { get; protected set; }

        /// <summary>
        /// Command displaying the modal allowing the user to add an action to the current scenario.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowAddActionModal { get; protected set; }

        #endregion

        #region Fluent, action and logical connective choices

        /// <summary>
        /// Contains the last selected action.
        /// </summary>
        public ActionViewModel SelectedAction { get; set; }

        /// <summary>
        /// Contains the last selected fluent.
        /// </summary>
        public LiteralViewModel SelectedFluent { get; set; }

        /// <summary>
        /// Command invoked when a formula has been selected for adding by the user.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> SelectFormula { get; set; }

        #endregion

        #region Clause dropdown display properties

        /// <summary>
        /// Read-only collection of all available action clause types.
        /// </summary>
        /// <remarks>
        /// Fun fact: I tried to leave this out and bind onto a static property in the view.
        /// Unfortunately ReactiveUI shat itself when I tried to do that, but this works, so here you go.
        /// Potential TODO: get this outta here
        /// </remarks>
        public ReadOnlyCollection<IActionClauseViewModel> ActionClauseTypes { get; } = ClauseTypes.GetAllImplementors<IActionClauseViewModel>();

        /// <summary>
        /// Read-only collection of all available query clause types.
        /// </summary>
        /// <remarks>
        /// Fun fact: I tried to leave this out and bind onto a static property in the view.
        /// Unfortunately ReactiveUI shat itself when I tried to do that, but this works, so here you go.
        /// Potential TODO: get this outta here
        /// </remarks>
        public ReadOnlyCollection<IQueryClauseViewModel> QueryClauseTypes { get; } = ClauseTypes.GetAllImplementors<IQueryClauseViewModel>();

        /// <summary>
        /// Function used to convert clause type names to their localized versions.
        /// </summary>
        public Func<object, string> LocalizeGroupName { get; private set; }

        #endregion

        #region Clause dropdown choices

        /// <summary>
        /// Contains the last selected action clause type.
        /// </summary>
        public IActionClauseViewModel SelectedActionClauseType { get; set; }

        /// <summary>
        /// Contains the last selected query clause type.
        /// </summary>
        public IQueryClauseViewModel SelectedQueryClauseType { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="RibbonViewModel"/> instance.
        /// </summary>
        /// <param name="languageSignature">Container with the current language signature.</param>
        /// <remarks>
        /// Omitting the <see cref="languageSignature"/> parameter causes the method to fetch the instance registered in the <see cref="Locator"/>.
        /// </remarks>
        public RibbonViewModel(LanguageSignature languageSignature = null)
        {
            LanguageSignature = languageSignature ?? Locator.Current.GetService<LanguageSignature>();

            Clear = ReactiveCommand.Create(() => Unit.Default);
            CloseWindow = ReactiveCommand.Create(() => Unit.Default);
            SetEnglishLocale = ReactiveCommand.Create(() => LocalizationProvider.SetLocale(LocalizationProvider.AmericanEnglish));
            SetPolishLocale = ReactiveCommand.Create(() => LocalizationProvider.SetLocale(LocalizationProvider.Polish));
            PerformCalculations = ReactiveCommand.Create(() => Unit.Default);
            ExportToFile = ReactiveCommand.Create(() => Unit.Default);

            ShowAddFluentModal = ReactiveCommand.Create(() =>
            {
                var modalView = (FluentModalView)Locator.Current.GetService<IViewFor<FluentModalViewModel>>();
                var modalViewModel = Locator.Current.GetService<FluentModalViewModel>();
                modalViewModel.ResetViewModel();
                modalView.ShowDialog();
            });
            ShowAddActionModal = ReactiveCommand.Create(() =>
            {
                var modalView = (ActionModalView)Locator.Current.GetService<IViewFor<ActionModalViewModel>>();
                var modalViewModel = Locator.Current.GetService<ActionModalViewModel>();
                modalViewModel.ResetViewModel();
                modalView.ShowDialog();
            });

            SelectFormula = ReactiveCommand.Create((IFormulaViewModel vm) => vm);

            LocalizeGroupName = obj => LocalizationProvider.Instance[((IClauseViewModel)obj).ClauseTypeNameKey];
            // Force switch to new language
            // I know, this is way too ugly to live, but I don't really want to spend more time on this
            // TODO: fix maybe?
            LocalizationProvider.Instance.PropertyChanged += (sender, args) =>
            {
                LocalizeGroupName = obj => LocalizationProvider.Instance[((IClauseViewModel)obj).ClauseTypeNameKey];
            };
        }
    }
}