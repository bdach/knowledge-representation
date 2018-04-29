using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ConditionalFluentReleaseStatementView.xaml
    /// </summary>
    public partial class ConditionalFluentReleaseStatementView : IViewFor<ConditionalFluentReleaseStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ConditionalFluentReleaseStatementViewModel),
                typeof(ConditionalFluentReleaseStatementView),
                new PropertyMetadata(default(ConditionalFluentReleaseStatementViewModel))
            );

        public ConditionalFluentReleaseStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelLeft, v => v.LabelLeft.Text);
            this.OneWayBind(ViewModel, vm => vm.Literal, v => v.Literal.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelRight, v => v.LabelRight.Text);
            this.OneWayBind(ViewModel, vm => vm.Precondition, v => v.Precondition.ViewModel);
        }

        public ConditionalFluentReleaseStatementViewModel ViewModel
        {
            get => (ConditionalFluentReleaseStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConditionalFluentReleaseStatementViewModel)value;
        }
    }
}