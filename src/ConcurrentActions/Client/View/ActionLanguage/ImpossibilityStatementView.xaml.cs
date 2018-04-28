using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ImpossibilityStatementView.xaml
    /// </summary>
    public partial class ImpossibilityStatementView : IViewFor<ImpossibilityStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ImpossibilityStatementViewModel),
                typeof(ImpossibilityStatementView),
                new PropertyMetadata(default(ImpossibilityStatementViewModel))
            );

        public ImpossibilityStatementView()
        {
            InitializeComponent();
        }

        public ImpossibilityStatementViewModel ViewModel
        {
            get => (ImpossibilityStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ImpossibilityStatementViewModel)value;
        }
    }
}