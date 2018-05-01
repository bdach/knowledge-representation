using System;
using System.Reactive.Linq;
using System.Windows;
using Client.Interface;
using Client.ViewModel;
using Client.ViewModel.Formula;
using ReactiveUI;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for RibbonView.xaml
    /// </summary>
    public partial class RibbonView : IViewFor<RibbonViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(RibbonViewModel),
                typeof(RibbonView),
                new PropertyMetadata(default(RibbonViewModel))
            );

        public RibbonView()
        {
            InitializeComponent();
            this.BindCommand(ViewModel, vm => vm.CloseWindow, v => v.CloseButton);
            this.BindCommand(ViewModel, vm => vm.SetEnglishLocale, v => v.EnglishButton);
            this.BindCommand(ViewModel, vm => vm.SetPolishLocale, v => v.PolishButton);
            this.BindCommand(ViewModel, vm => vm.ShowAddFluentModal, v => v.AddFluentButton);
            this.BindCommand(ViewModel, vm => vm.ShowAddActionModal, v => v.AddActionButton);
            
            this.OneWayBind(ViewModel, vm => vm.ScenarioContainer.LiteralViewModels, v => v.FluentsGallery.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.ScenarioContainer.ActionViewModels, v => v.ActionsGallery.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.ActionClauseTypes, v => v.ActionClauseGallery.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.QueryClauseTypes, v => v.QueryClauseGallery.ItemsSource);
            this.Bind(ViewModel, vm => vm.LocalizeGroupName, v => v.ActionClauseGallery.GroupByAdvanced);
            this.Bind(ViewModel, vm => vm.LocalizeGroupName, v => v.QueryClauseGallery.GroupByAdvanced);

            this.Bind(ViewModel, vm => vm.SelectedAction, v => v.ActionsGallery.SelectedValue);
            this.Bind(ViewModel, vm => vm.SelectedFluent, v => v.FluentsGallery.SelectedValue);
            this.Bind(ViewModel, vm => vm.SelectedActionClauseType, v => v.ActionClauseGallery.SelectedValue);
            this.Bind(ViewModel, vm => vm.SelectedQueryClauseType, v => v.QueryClauseGallery.SelectedValue);

            this.BindCommand(ViewModel, vm => vm.SelectFormula, v => v.ConjunctionButton, Observable.Start(() => new ConjunctionViewModel()));
            this.BindCommand(ViewModel, vm => vm.SelectFormula, v => v.AlternativeButton, Observable.Start(() => new AlternativeViewModel()));
            this.BindCommand(ViewModel, vm => vm.SelectFormula, v => v.ImplicationButton, Observable.Start(() => new ImplicationViewModel()));
            this.BindCommand(ViewModel, vm => vm.SelectFormula, v => v.EquivalenceButton, Observable.Start(() => new EquivalenceViewModel()));
            this.BindCommand(ViewModel, vm => vm.SelectFormula, v => v.NegationButton, Observable.Start(() => new NegationViewModel()));
            this.WhenAnyValue(v => v.ViewModel.SelectedFluent)
                .Where(v => v != null)
                .Select(v => new LiteralViewModel(v.Fluent))
                .InvokeCommand<IFormulaViewModel, RibbonView>(this, v => v.ViewModel.SelectFormula);

            this.WhenAnyValue(v => v.FluentDropDown.IsDropDownOpen)
                .Where(open => open)
                .Subscribe(_ => FluentsGallery.SelectedValue = null);
            this.WhenAnyValue(v => v.ActionDropDown.IsDropDownOpen)
                .Where(open => open)
                .Subscribe(_ => ActionsGallery.SelectedValue = null);
            this.WhenAnyValue(v => v.AddActionClauseDropDown.IsDropDownOpen)
                .Where(open => open)
                .Subscribe(_ => ActionClauseGallery.SelectedValue = null);
            this.WhenAnyValue(v => v.AddQueryClauseDropDown.IsDropDownOpen)
                .Where(open => open)
                .Subscribe(_ => QueryClauseGallery.SelectedValue = null);

            this.WhenAnyObservable(v => v.ViewModel.ScenarioContainer.LiteralViewModels.CountChanged)
                .Select(GetVisibilityForCount)
                .Subscribe(visibility => FluentSeparator.Visibility = visibility);
            this.WhenAnyObservable(v => v.ViewModel.ScenarioContainer.ActionViewModels.CountChanged)
                .Select(GetVisibilityForCount)
                .Subscribe(visibility => ActionSeparator.Visibility = visibility);
        }

        private Visibility GetVisibilityForCount(int count)
        {
            return count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public RibbonViewModel ViewModel
        {
            get => (RibbonViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (RibbonViewModel)value;
        }
    }
}