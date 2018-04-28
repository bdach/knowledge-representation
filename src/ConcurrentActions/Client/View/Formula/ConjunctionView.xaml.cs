using System.Windows;
using Client.ViewModel.Formula;
using ReactiveUI;

namespace Client.View.Formula
{
    /// <summary>
    /// Interaction logic for ConjunctionView.xaml
    /// </summary>
    public partial class ConjunctionView : IViewFor<ConjunctionViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ConjunctionViewModel),
                typeof(ConjunctionView),
                new PropertyMetadata(default(ConjunctionViewModel))
            );

        public ConjunctionView()
        {
            InitializeComponent();
        }

        public ConjunctionViewModel ViewModel
        {
            get => (ConjunctionViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConjunctionViewModel)value;
        }
    }
}