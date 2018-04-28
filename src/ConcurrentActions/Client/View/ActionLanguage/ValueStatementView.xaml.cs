using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ValueStatementView.xaml
    /// </summary>
    public partial class ValueStatementView : IViewFor<ValueStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ValueStatementViewModel),
                typeof(ValueStatementView),
                new PropertyMetadata(default(ValueStatementViewModel))
            );

        public ValueStatementView()
        {
            InitializeComponent();
        }

        public ValueStatementViewModel ViewModel
        {
            get => (ValueStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ValueStatementViewModel)value;
        }
    }
}