using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ObservationStatementView.xaml
    /// </summary>
    public partial class ObservationStatementView : IViewFor<ObservationStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ObservationStatementViewModel),
                typeof(ObservationStatementView),
                new PropertyMetadata(default(ObservationStatementViewModel))
            );

        public ObservationStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.LabelLeft, v => v.LabelLeft.Text);
            this.OneWayBind(ViewModel, vm => vm.Condition, v => v.Condition.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelRight, v => v.LabelRight.Text);
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);
        }

        public ObservationStatementViewModel ViewModel
        {
            get => (ObservationStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ObservationStatementViewModel)value;
        }
    }
}