using System.Windows;
using Client.ViewModel.Formula;
using ReactiveUI;

namespace Client.View.Formula
{
    /// <summary>
    /// Interaction logic for ConstantView.xaml
    /// </summary>
    public partial class ConstantView : IViewFor<ConstantViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ConstantViewModel),
                typeof(ConstantView),
                new PropertyMetadata(default(ConstantViewModel))
            );

        public ConstantView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Constant, v => v.Constant.Text);
        }

        public ConstantViewModel ViewModel
        {
            get => (ConstantViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConstantViewModel)value;
        }
    }
}