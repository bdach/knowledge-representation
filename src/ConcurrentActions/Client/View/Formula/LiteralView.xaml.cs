using System.Windows;
using Client.ViewModel.Formula;
using ReactiveUI;

namespace Client.View.Formula
{
    /// <summary>
    /// Interaction logic for LiteralView.xaml
    /// </summary>
    public partial class LiteralView : IViewFor<LiteralViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(LiteralViewModel),
                typeof(LiteralView),
                new PropertyMetadata(default(LiteralViewModel))
            );

        public LiteralView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Fluent.Name, v => v.Name.Text);
        }

        public LiteralViewModel ViewModel
        {
            get => (LiteralViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (LiteralViewModel)value;
        }
    }
}