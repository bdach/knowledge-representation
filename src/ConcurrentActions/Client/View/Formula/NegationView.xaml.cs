using System.Windows;
using Client.ViewModel.Formula;
using ReactiveUI;

namespace Client.View.Formula
{
    /// <summary>
    /// Interaction logic for NegationView.xaml
    /// </summary>
    public partial class NegationView : IViewFor<NegationViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(NegationViewModel),
                typeof(NegationView),
                new PropertyMetadata(default(NegationViewModel))
            );

        public NegationView()
        {
            InitializeComponent();
        }

        public NegationViewModel ViewModel
        {
            get => (NegationViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (NegationViewModel)value;
        }
    }
}