using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Xml.Serialization;
using System.Windows.Input;
using Client.Abstract;
using Client.Global;
using Client.View;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model.Forms;
using ReactiveUI;
using Splat;

namespace Client.ViewModel
{
    /// <inheritdoc />
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

            #endregion

            #region Proxying user choices to query area

            this.WhenAnyObservable(vm => vm.RibbonViewModel.SelectFormula)
                .InvokeCommand(QueryAreaViewModel, vm => vm.AddFormula);

            #endregion

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

            RibbonViewModel.PerformCalculations.Subscribe(_ =>
            {
                // TODO: retrieve and process current scenario (filter, add fluent values, etc.) and pass further to DynamicSystem
            });

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
            DeleteFocused.Where(_ => Keyboard.IsKeyDown(Key.Delete)).InvokeCommand(ActionAreaViewModel, vm => vm.DeleteFocused);

            RibbonViewModel.ExportToFile.Subscribe(_ =>
            {
                // TODO: this breaks because XmlSerializer doesn't know how to handle IFormula
                var scenario = GetCurrentScenario();

                var isoDateTimeString = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture)
                    .Replace("-", string.Empty).Replace(":", string.Empty);
                var filepath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}" + $"\\scenario-{isoDateTimeString}.xml";

                var serializer = new XmlSerializer(typeof(Scenario));
                var writer = new StreamWriter(filepath);
                serializer.Serialize(writer, scenario);
                writer.Close();
            });
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
    }
}