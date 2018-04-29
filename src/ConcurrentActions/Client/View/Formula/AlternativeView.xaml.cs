using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.Formula;
using ReactiveUI;

namespace Client.View.Formula
{
    /// <summary>
    /// Interaction logic for AlternativeView.xaml
    /// </summary>
    public partial class AlternativeView : IViewFor<AlternativeViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(AlternativeViewModel),
                typeof(AlternativeView),
                new PropertyMetadata(default(AlternativeViewModel))
            );

        public AlternativeView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Left, v => v.LeftOperand.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.Operator, v => v.Operator.Text);
            this.OneWayBind(ViewModel, vm => vm.Right, v => v.RightOperand.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.LeftOperand.IsMouseOver, v => v.RightOperand.IsMouseOver)
                .Select(v => v.Item1 && !v.Item2 && !v.Item3)
                .Subscribe(v => Highlight = v);
        }

        public AlternativeViewModel ViewModel
        {
            get => (AlternativeViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AlternativeViewModel)value;
        }
    }
}