using System.Windows;
using Client.ViewModel.Modal;
using ReactiveUI;

namespace Client.View.Modal
{
    /// <summary>
    /// Interaction logic for FluentModalView.xaml
    /// </summary>
    public partial class FluentModalView : IViewFor<FluentModalViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(FluentModalViewModel),
                typeof(FluentModalView),
                new PropertyMetadata(default(FluentModalViewModel))
            );

        public FluentModalView()
        {
            InitializeComponent();
        }

        public FluentModalViewModel ViewModel
        {
            get => (FluentModalViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (FluentModalViewModel)value;
        }
    }
}
