using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.QueryLanguage;
using ReactiveUI;

namespace Client.View.QueryLanguage
{
    /// <summary>
    /// Interaction logic for AccessibilityQueryView.xaml
    /// </summary>
    public partial class AccessibilityQueryView : IViewFor<AccessibilityQueryViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(AccessibilityQueryViewModel),
                typeof(AccessibilityQueryView),
                new PropertyMetadata(default(AccessibilityQueryViewModel))
            );

        public AccessibilityQueryView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.Target, v => v.Target.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Target.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2)
                .Subscribe(v => Highlight = v);
        }

        public AccessibilityQueryViewModel ViewModel
        {
            get => (AccessibilityQueryViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AccessibilityQueryViewModel)value;
        }
    }
}