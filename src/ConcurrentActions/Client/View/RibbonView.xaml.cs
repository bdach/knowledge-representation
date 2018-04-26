using System.Windows;
using Client.ViewModel;
using ReactiveUI;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for RibbonView.xaml
    /// </summary>
    public partial class RibbonView : IViewFor<RibbonViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(RibbonViewModel),
                typeof(RibbonView),
                new PropertyMetadata(default(RibbonViewModel))
            );

        public RibbonView()
        {
            InitializeComponent();
            this.BindCommand(ViewModel, vm => vm.CloseWindow, v => v.CloseButton);

            // TODO: hide separators in fluent and action dropdowns if there are no items in galeries
            // TODO: remove context menu when right-clicking fluent ribbon and the possiblity to collapse it (if it's even possible)
        }

        public RibbonViewModel ViewModel
        {
            get => (RibbonViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (RibbonViewModel)value;
        }
    }
}