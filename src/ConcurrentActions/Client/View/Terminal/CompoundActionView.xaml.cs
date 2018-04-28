using System.Windows;
using Client.ViewModel.Terminal;
using ReactiveUI;

namespace Client.View.Terminal
{
    /// <summary>
    /// Interaction logic for CompoundActionView.xaml
    /// </summary>
    public partial class CompoundActionView : IViewFor<CompoundActionViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(CompoundActionViewModel),
                typeof(CompoundActionView),
                new PropertyMetadata(default(CompoundActionViewModel))
            );

        public CompoundActionView()
        {
            InitializeComponent();
        }

        public CompoundActionViewModel ViewModel
        {
            get => (CompoundActionViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CompoundActionViewModel)value;
        }
    }
}