using System.Windows;
using Client.ViewModel.QueryLanguage;
using ReactiveUI;

namespace Client.View.QueryLanguage
{
    /// <summary>
    /// Interaction logic for ExistentialValueQueryView.xaml
    /// </summary>
    public partial class ExistentialValueQueryView : IViewFor<ExistentialValueQueryViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ExistentialValueQueryViewModel),
                typeof(ExistentialValueQueryView),
                new PropertyMetadata(default(ExistentialValueQueryViewModel))
            );

        public ExistentialValueQueryView()
        {
            InitializeComponent();
        }

        public ExistentialValueQueryViewModel ViewModel
        {
            get => (ExistentialValueQueryViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ExistentialValueQueryViewModel)value;
        }
    }
}