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