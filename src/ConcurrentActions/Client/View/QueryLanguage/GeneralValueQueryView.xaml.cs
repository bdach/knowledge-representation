using System.Windows;
using Client.ViewModel.QueryLanguage;
using ReactiveUI;

namespace Client.View.QueryLanguage
{
    /// <summary>
    /// Interaction logic for GeneralValueQueryView.xaml
    /// </summary>
    public partial class GeneralValueQueryView : IViewFor<GeneralValueQueryViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(GeneralValueQueryViewModel),
                typeof(GeneralValueQueryView),
                new PropertyMetadata(default(GeneralValueQueryViewModel))
            );

        public GeneralValueQueryView()
        {
            InitializeComponent();
        }

        public GeneralValueQueryViewModel ViewModel
        {
            get => (GeneralValueQueryViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (GeneralValueQueryViewModel)value;
        }
    }
}