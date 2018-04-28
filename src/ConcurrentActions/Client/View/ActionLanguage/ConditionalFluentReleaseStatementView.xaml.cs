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