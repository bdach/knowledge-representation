using System.Windows;
using Client.ViewModel;
using ReactiveUI;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for ActionAreaView.xaml
    /// </summary>
    public partial class ActionAreaView : IViewFor<ActionAreaViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ActionAreaViewModel),
                typeof(ActionAreaView),
                new PropertyMetadata(default(ActionAreaViewModel))
            );

        public ActionAreaView()
        {
            InitializeComponent();
        }

        public ActionAreaViewModel ViewModel
        {
            get => (ActionAreaViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ActionAreaViewModel)value;
        }
    }
}
