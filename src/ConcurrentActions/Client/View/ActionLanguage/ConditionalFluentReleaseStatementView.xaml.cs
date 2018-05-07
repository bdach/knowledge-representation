using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ConditionalFluentReleaseStatementView.xaml
    /// </summary>
    public partial class ConditionalFluentReleaseStatementView : IViewFor<ConditionalFluentReleaseStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ConditionalFluentReleaseStatementViewModel),
                typeof(ConditionalFluentReleaseStatementView),
                new PropertyMetadata(default(ConditionalFluentReleaseStatementViewModel))
            );

        public ConditionalFluentReleaseStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelLeft, v => v.LabelLeft.Text);
            this.OneWayBind(ViewModel, vm => vm.Fluent, v => v.Literal.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelRight, v => v.LabelRight.Text);
            this.OneWayBind(ViewModel, vm => vm.Precondition, v => v.Precondition.ViewModel);

            this.WhenAnyValue(v => v.IsFocused)
                .BindTo(this, v => v.ViewModel.IsFocused);

            this.WhenAnyValue(vm => vm.IsMouseOver, vm => vm.Action.IsMouseOver, vm => vm.Literal.IsMouseOver,
                    vm => vm.Precondition.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2 && !t.Item3 && !t.Item4)
                .Subscribe(v => Highlight = v);
        }

        public ConditionalFluentReleaseStatementViewModel ViewModel
        {
            get => (ConditionalFluentReleaseStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConditionalFluentReleaseStatementViewModel)value;
        }
    }
}