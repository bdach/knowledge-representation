using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for UnconditionalImpossibilityStatementView.xaml
    /// </summary>
    public partial class UnconditionalImpossibilityStatementView : IViewFor<UnconditionalImpossibilityStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(UnconditionalImpossibilityStatementViewModel),
                typeof(UnconditionalImpossibilityStatementView),
                new PropertyMetadata(default(UnconditionalImpossibilityStatementViewModel))
            );

        public UnconditionalImpossibilityStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);
        }

        public UnconditionalImpossibilityStatementViewModel ViewModel
        {
            get => (UnconditionalImpossibilityStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (UnconditionalImpossibilityStatementViewModel)value;
        }
    }
}