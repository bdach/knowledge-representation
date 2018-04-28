using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for UnconditionalEffectStatementView.xaml
    /// </summary>
    public partial class UnconditionalEffectStatementView : IViewFor<UnconditionalEffectStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(UnconditionalEffectStatementViewModel),
                typeof(UnconditionalEffectStatementView),
                new PropertyMetadata(default(UnconditionalEffectStatementViewModel))
            );

        public UnconditionalEffectStatementView()
        {
            InitializeComponent();
        }

        public UnconditionalEffectStatementViewModel ViewModel
        {
            get => (UnconditionalEffectStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (UnconditionalEffectStatementViewModel)value;
        }
    }
}