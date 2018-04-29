using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.Formula;
using ReactiveUI;

namespace Client.View.Formula
{
    /// <summary>
    /// Interaction logic for NegationView.xaml
    /// </summary>
    public partial class NegationView : IViewFor<NegationViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(NegationViewModel),
                typeof(NegationView),
                new PropertyMetadata(default(NegationViewModel))
            );

        public NegationView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Prefix, v => v.Prefix.Text);
            this.OneWayBind(ViewModel, vm => vm.Formula, v => v.Formula.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.Suffix, v => v.Suffix.Text);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Formula.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2)
                .Subscribe(v => Highlight = v);
        }

        public NegationViewModel ViewModel
        {
            get => (NegationViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (NegationViewModel)value;
        }
    }
}