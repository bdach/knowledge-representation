using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Client.Abstract;
using Client.Global;
using Client.View;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Microsoft.Win32;
using Model.Forms;
using ReactiveUI;
using Splat;
using XSerializer;

namespace Client.ViewModel
{
    /// <summary>
    /// View model for <see cref="T:Client.View.ShellView" /> which is the root view of the application.
    /// </summary>
    public class ShellViewModel : FodyReactiveObject
    {
        /// <summary>
        /// View model of nested <see cref="RibbonView"/>.
        /// </summary>
        public RibbonViewModel RibbonViewModel { get; set; }

        /// <summary>
        /// View model of nested <see cref="ActionAreaView"/>.
        /// </summary>
        public ActionAreaViewModel ActionAreaViewModel { get; set; }

        /// <summary>
        /// View model of nested <see cref="QueryAreaView"/>.
        /// </summary>
        public QueryAreaViewModel QueryAreaViewModel { get; set; }

        /// <summary>
        /// Command triggered by delete key used to delete currently selected clause element.
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ShellViewModel"/> instance.
        /// </summary>
        public ShellViewModel()
        {
            RibbonViewModel = new RibbonViewModel();
            ActionAreaViewModel = new ActionAreaViewModel();
            QueryAreaViewModel = new QueryAreaViewModel();

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
            RibbonViewModel.PerformCalculations.Subscribe(_ =>
            {
                // TODO: retrieve and process current scenario (filter, add fluent values, etc.) and pass further to DynamicSystem
            });

            #region Proxying user choices to action area

            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedQueryClauseType)
                .Where(vm => vm != null)
                .Subscribe(t => QueryAreaViewModel.QuerySet.Add(t.NewInstance()));
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedActionClauseType)
                .Where(vm => vm != null)
                .Subscribe(t => ActionAreaViewModel.ActionDomain.Add(t.NewInstance()));
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedFluent)
                .Where(vm => vm != null)
                .Select(fl => new LiteralViewModel(fl.Fluent))
                .InvokeCommand(ActionAreaViewModel, vm => vm.AddFluent);
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedAction)
                .Where(vm => vm != null)
                .Select(ac => new ActionViewModel(ac.Action))
                .InvokeCommand(ActionAreaViewModel, vm => vm.AddAction);
            this.WhenAnyObservable(vm => vm.RibbonViewModel.SelectFormula)
                .InvokeCommand(ActionAreaViewModel, vm => vm.AddFormula);

            DeleteFocused.Where(_ => Keyboard.IsKeyDown(Key.Delete)).InvokeCommand(ActionAreaViewModel, vm => vm.DeleteFocused);

            #endregion

            #region Proxying user choices to query area

            this.WhenAnyObservable(vm => vm.RibbonViewModel.SelectFormula)
                .InvokeCommand(QueryAreaViewModel, vm => vm.AddFormula);
            this.WhenAnyObservable(vm => vm.RibbonViewModel.AddEmptyCompoundAction)
                .InvokeCommand(QueryAreaViewModel, vm => vm.AddEmptyCompoundAction);
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedAction)
                .InvokeCommand(QueryAreaViewModel, vm => vm.AddAtomicAction);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .InvokeCommand(QueryAreaViewModel, vm => vm.DeleteFocused);

            #endregion

            #region Backstage support

            RibbonViewModel.Clear.Subscribe(_ =>
            {
                ActionAreaViewModel.ActionDomain.Clear();
                QueryAreaViewModel.QuerySet.Clear();

                var currentSignature = Locator.Current.GetService<LanguageSignature>();
                currentSignature.ActionViewModels.Clear();
                currentSignature.CompoundActionViewModels.Clear();
                currentSignature.LiteralViewModels.Clear();
                currentSignature.ProgramViewModels.Clear();
            });

            RibbonViewModel.ImportFromFile.Subscribe(_ =>
            {
                var filepath = OpenFileDialog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                if (!string.IsNullOrEmpty(filepath))
                {
                    var serializer = new XmlSerializer<Scenario>();
                    var reader = new StreamReader(filepath);
                    var scenario = serializer.Deserialize(reader);
                    // TODO: recreate ViewModels based on models in scenario object
                }
            });

            RibbonViewModel.ExportToFile.Subscribe(_ =>
            {
                var scenario = GetCurrentScenario();

                var isoDateTimeString = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture)
                    .Replace("-", string.Empty).Replace(":", string.Empty);
                var filepath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}" + $"\\scenario-{isoDateTimeString}.xml";

                var serializer = new XmlSerializer<Scenario>();
                var writer = new StreamWriter(filepath);
                serializer.Serialize(writer, scenario);
                writer.Close();
            });

            #endregion
        }

        /// <summary>
        /// Retrieves the currently defined scenario along with the language signature.
        /// </summary>
        /// <returns><see cref="Scenario"/> instance which is a full description of currently defined scenario.</returns>
        private Scenario GetCurrentScenario()
        {
            var currentSignature = Locator.Current.GetService<LanguageSignature>();
            return new Scenario
            {
                Actions = currentSignature.ActionViewModels.Select(x => x.ToModel()).ToList(),
                CompoundActions = currentSignature.CompoundActionViewModels.Select(x => x.ToModel()).ToList(),
                Literals = currentSignature.LiteralViewModels.Select(x => (Literal)x.ToModel()).ToList(),
                Programs = currentSignature.ProgramViewModels.Select(x => x.ToModel()).ToList(),
                ActionDomain = ActionAreaViewModel.GetActionDomainModel(),
                QuerySet = QueryAreaViewModel.GetQuerySetModel()
            };
        }

        /// <summary>
        /// Opens file dialog allowing user to select a file.
        /// </summary>
        /// <param name="defaultPath">Default path with which the modal window will open.</param>
        /// <returns>PAth to selected file.</returns>
        private string OpenFileDialog(string defaultPath)
        {
            // TODO: should this be done more MVVM-ish?
            var dialog = new OpenFileDialog { InitialDirectory = defaultPath };
            dialog.ShowDialog();
            return dialog.FileName;
        }
    }
}