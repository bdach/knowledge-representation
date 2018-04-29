using System.Windows;
using Client.ViewModel.Formula;
using ReactiveUI;

namespace Client.View.Formula
{
    /// <summary>
    /// Interaction logic for ImplicationView.xaml
    /// </summary>
    public partial class ImplicationView : IViewFor<ImplicationViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ImplicationViewModel),
                typeof(ImplicationView),
                new PropertyMetadata(default(ImplicationViewModel))
            );

        public ImplicationView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Antecedent, v => v.Antecedent.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.Operator, v => v.Operator.Text);
            this.OneWayBind(ViewModel, vm => vm.Consequent, v => v.Consequent.ViewModel);
        }

        public ImplicationViewModel ViewModel
        {
            get => (ImplicationViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ImplicationViewModel)value;
        }
    }
}