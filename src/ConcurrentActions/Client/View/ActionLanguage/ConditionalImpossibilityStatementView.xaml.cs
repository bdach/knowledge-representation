using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ConditionalImpossibilityStatementView.xaml
    /// </summary>
    public partial class ConditionalImpossibilityStatementView : IViewFor<ConditionalImpossibilityStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ConditionalImpossibilityStatementViewModel),
                typeof(ConditionalImpossibilityStatementView),
                new PropertyMetadata(default(ConditionalImpossibilityStatementViewModel))
            );

        public ConditionalImpossibilityStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.LabelLeft, v => v.LabelLeft.Text);
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelRight, v => v.LabelRight.Text);
            this.OneWayBind(ViewModel, vm => vm.Precondition, v => v.Precondition.ViewModel);

            this.WhenAnyValue(v => v.IsFocused)
                .BindTo(this, v => v.ViewModel.IsFocused);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Action.IsMouseOver, v => v.Precondition.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2 && !t.Item3)
                .Subscribe(v => Highlight = v);
        }

        public ConditionalImpossibilityStatementViewModel ViewModel
        {
            get => (ConditionalImpossibilityStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConditionalImpossibilityStatementViewModel)value;
        }
    }
}