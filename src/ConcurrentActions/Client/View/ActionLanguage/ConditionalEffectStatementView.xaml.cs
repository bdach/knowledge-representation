using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ConditionalEffectStatementView.xaml
    /// </summary>
    public partial class ConditionalEffectStatementView : IViewFor<ConditionalEffectStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ConditionalEffectStatementViewModel),
                typeof(ConditionalEffectStatementView),
                new PropertyMetadata(default(ConditionalEffectStatementViewModel))
            );

        public ConditionalEffectStatementView()
        {
            InitializeComponent();
        }

        public ConditionalEffectStatementViewModel ViewModel
        {
            get => (ConditionalEffectStatementViewModel) GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConditionalEffectStatementViewModel) value;
        }
    }
}