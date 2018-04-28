using System.Windows;
using Client.ViewModel.Terminal;
using ReactiveUI;

namespace Client.View.Terminal
{
    /// <summary>
    /// Interaction logic for ProgramView.xaml
    /// </summary>
    public partial class ProgramView : IViewFor<ProgramViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ProgramViewModel),
                typeof(ProgramView),
                new PropertyMetadata(default(ProgramViewModel))
            );

        public ProgramView()
        {
            InitializeComponent();
        }

        public ProgramViewModel ViewModel
        {
            get => (ProgramViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ProgramViewModel)value;
        }
    }
}