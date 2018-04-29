using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.Formula;
using ReactiveUI;

namespace Client.View.Formula
{
    /// <summary>
    /// Interaction logic for EquivalenceView.xaml
    /// </summary>
    public partial class EquivalenceView : IViewFor<EquivalenceViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(EquivalenceViewModel),
                typeof(EquivalenceView),
                new PropertyMetadata(default(EquivalenceViewModel))
            );

        public EquivalenceView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Left, v => v.LeftOperand.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.Operator, v => v.Operator.Text);
            this.OneWayBind(ViewModel, vm => vm.Right, v => v.RightOperand.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.LeftOperand.IsMouseOver, v => v.RightOperand.IsMouseOver)
                .Select(v => v.Item1 && !v.Item2 && !v.Item3)
                .Subscribe(v => Highlight = v);
        }

        public EquivalenceViewModel ViewModel
        {
            get => (EquivalenceViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (EquivalenceViewModel)value;
        }
    }
}