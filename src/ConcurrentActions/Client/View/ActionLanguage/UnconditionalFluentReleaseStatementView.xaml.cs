using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for UnconditionalFluentReleaseStatementView.xaml
    /// </summary>
    public partial class UnconditionalFluentReleaseStatementView : IViewFor<UnconditionalFluentReleaseStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(UnconditionalFluentReleaseStatementViewModel),
                typeof(UnconditionalFluentReleaseStatementView),
                new PropertyMetadata(default(UnconditionalFluentReleaseStatementViewModel))
            );

        public UnconditionalFluentReleaseStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.Literal, v => v.Literal.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Action.IsMouseOver, v => v.Literal.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2 && !t.Item3)
                .Subscribe(v => Highlight = v);
        }

        public UnconditionalFluentReleaseStatementViewModel ViewModel
        {
            get => (UnconditionalFluentReleaseStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (UnconditionalFluentReleaseStatementViewModel)value;
        }
    }
}