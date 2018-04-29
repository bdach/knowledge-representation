using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ConditionalImpossibilityStatementView.xaml
    /// </summary>
    public partial class ConditionalImpossibilityStatementView : IViewFor<ConditionalImpossibilityStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ConditionalImpossibilityStatementViewModel),
                typeof(ConditionalImpossibilityStatementView),
                new PropertyMetadata(default(ConditionalImpossibilityStatementViewModel))
            );

        public ConditionalImpossibilityStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.LabelLeft, v => v.LabelLeft.Text);
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelRight, v => v.LabelRight.Text);
            this.OneWayBind(ViewModel, vm => vm.Precondition, v => v.Precondition.ViewModel);
        }

        public ConditionalImpossibilityStatementViewModel ViewModel
        {
            get => (ConditionalImpossibilityStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConditionalImpossibilityStatementViewModel)value;
        }
    }
}