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