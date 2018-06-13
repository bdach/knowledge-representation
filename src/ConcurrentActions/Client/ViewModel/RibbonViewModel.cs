using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Documents;
using Client.Abstract;
using Client.Global;
using Client.Interface;
using Client.Provider;
using Client.View;
using Client.View.Modal;
using Client.ViewModel.Formula;
using Client.ViewModel.Modal;
using Client.ViewModel.Terminal;
using DynamicSystem;
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

        /// <summary>
        /// Command changing the application language to Polish.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SetPolishLocale { get; protected set; }

        /// <summary>
        /// Command used to trigger editor scenario evaluation.
        /// </summary>
        public ReactiveCommand<Unit, QueryResolution> PerformCalculations { get; set; }

        /// <summary>
        /// Command used to cancel editor scenario evaluation.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCalculations { get; set; }

        /// <summary>
        /// Command used to trigger grammar scenario evaluation.
        /// </summary>
        public ReactiveCommand<Unit, Tuple<QueryResolution, Dictionary<object, int>>> PerformGrammarCalculations { get; set; }

        /// <summary>
        /// Command used to trigger import of scenario from a file.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ImportFromFile { get; protected set; }

        /// <summary>
        /// Command used to trigger export of current scenario to file.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ExportToFile { get; protected set; }

        /// <summary>
        /// Command used to determine editor tab selection.
        /// </summary>
        public ReactiveCommand<Unit, Unit> EditTabSelected { get; protected set; }

        /// <summary>
        /// Command used to determine grammar tab selection.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GrammarTabSelected { get; protected set; }

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
        public ReadOnlyCollection<IActionClauseViewModel> ActionClauseTypes { get; } = ClauseTypes.GetAllImplementors<IActionClauseViewModel>();

        /// <summary>
        /// Read-only collection of all available query clause types.
        /// </summary>
        public ReadOnlyCollection<IQueryClauseViewModel> QueryClauseTypes { get; } = ClauseTypes.GetAllImplementors<IQueryClauseViewModel>();

        /// <summary>
        /// Function used to convert clause type names to their localized versions.
        /// </summary>
        public Func<object, string> LocalizeGroupName { get; private set; }

        #endregion

        #region Clause editing commands & properties

        /// <summary>
        /// Contains the last selected action clause type.
        /// </summary>
        public IActionClauseViewModel SelectedActionClauseType { get; set; }

        /// <summary>
        /// Contains the last selected query clause type.
        /// </summary>
        public IQueryClauseViewModel SelectedQueryClauseType { get; set; }

        /// <summary>
        /// Command used to add a new empty compound action to a program.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddEmptyCompoundAction { get; protected set; }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether edit tab is selected
        /// </summary>
        public bool IsEditTabSelected { get; set; }

        /// <summary>
        /// Gets or sets whether grammar tab is selected
        /// </summary>
        public bool IsGrammarTabSelected { get; set; }
  
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
            ImportFromFile = ReactiveCommand.Create(() => Unit.Default);
            ExportToFile = ReactiveCommand.Create(() => Unit.Default);
            EditTabSelected = ReactiveCommand.Create(() => Unit.Default);
            GrammarTabSelected = ReactiveCommand.Create(() => Unit.Default);
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
            AddEmptyCompoundAction = ReactiveCommand.Create(() => Unit.Default);
            LocalizeGroupName = obj => LocalizationProvider.Instance[((IClauseViewModel)obj).ClauseTypeNameKey];
            CancelCalculations = ReactiveCommand.Create(() => Unit.Default);

            // force switch to new language
            LocalizationProvider.Instance.PropertyChanged += (sender, args) =>
            {
                LocalizeGroupName = obj => LocalizationProvider.Instance[((IClauseViewModel)obj).ClauseTypeNameKey];
            };
        }
    }
}