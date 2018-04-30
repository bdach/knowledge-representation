using System.Windows;
using Client.ViewModel;
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

            // TODO: hide separators in fluent and action dropdowns if there are no items in galeries
            this.OneWayBind(ViewModel, vm => vm.ScenarioContainer.LiteralViewModels, v => v.FluentsGallery.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.ScenarioContainer.ActionViewModels, v => v.ActionsGallery.ItemsSource);
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