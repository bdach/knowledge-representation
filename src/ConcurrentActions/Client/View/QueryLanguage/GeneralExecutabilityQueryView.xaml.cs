using System.Windows;
using Client.ViewModel.QueryLanguage;
using ReactiveUI;

namespace Client.View.QueryLanguage
{
    /// <summary>
    /// Interaction logic for GeneralExecutabilityQueryView.xaml
    /// </summary>
    public partial class GeneralExecutabilityQueryView : IViewFor<GeneralExecutabilityQueryViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(GeneralExecutabilityQueryViewModel),
                typeof(GeneralExecutabilityQueryView),
                new PropertyMetadata(default(GeneralExecutabilityQueryViewModel))
            );

        public GeneralExecutabilityQueryView()
        {
            InitializeComponent();
        }

        public GeneralExecutabilityQueryViewModel ViewModel
        {
            get => (GeneralExecutabilityQueryViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (GeneralExecutabilityQueryViewModel)value;
        }
    }
}