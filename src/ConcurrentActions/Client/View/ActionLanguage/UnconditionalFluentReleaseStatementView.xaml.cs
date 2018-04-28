using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for UnconditionalFluentReleaseStatementView.xaml
    /// </summary>
    public partial class UnconditionalFluentReleaseStatementView : IViewFor<UnconditionalFluentReleaseStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(UnconditionalFluentReleaseStatementViewModel),
                typeof(UnconditionalFluentReleaseStatementView),
                new PropertyMetadata(default(UnconditionalFluentReleaseStatementViewModel))
            );

        public UnconditionalFluentReleaseStatementView()
        {
            InitializeComponent();
        }

        public UnconditionalFluentReleaseStatementViewModel ViewModel
        {
            get => (UnconditionalFluentReleaseStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (UnconditionalFluentReleaseStatementViewModel)value;
        }
    }
}