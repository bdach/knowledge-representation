using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ValueStatementView.xaml
    /// </summary>
    public partial class ValueStatementView : IViewFor<ValueStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ValueStatementViewModel),
                typeof(ValueStatementView),
                new PropertyMetadata(default(ValueStatementViewModel))
            );

        public ValueStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Condition, v => v.Condition.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);

            this.WhenAnyValue(v => v.IsFocused)
                .BindTo(this, v => v.ViewModel.IsFocused);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Condition.IsMouseOver, v => v.Action.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2 && !t.Item3)
                .Subscribe(v => Highlight = v);
        }

        public ValueStatementViewModel ViewModel
        {
            get => (ValueStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ValueStatementViewModel)value;
        }
    }
}