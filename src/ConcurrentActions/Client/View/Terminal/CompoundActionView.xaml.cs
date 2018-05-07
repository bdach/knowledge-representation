using System;
using System.Reactive.Linq;
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
            this.OneWayBind(ViewModel, vm => vm.Actions, v => v.Actions.ItemsSource);

            this.WhenAnyValue(v => v.IsFocused)
                .BindTo(this, v => v.ViewModel.IsFocused);
            this.WhenAnyValue(v => v.IsMouseOver, v => v.Actions.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2)
                .Subscribe(v => Highlight = v);
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