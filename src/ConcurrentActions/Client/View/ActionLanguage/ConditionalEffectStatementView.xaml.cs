using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ConditionalEffectStatementView.xaml
    /// </summary>
    public partial class ConditionalEffectStatementView : IViewFor<ConditionalEffectStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ConditionalEffectStatementViewModel),
                typeof(ConditionalEffectStatementView),
                new PropertyMetadata(default(ConditionalEffectStatementViewModel))
            );

        public ConditionalEffectStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelLeft, v => v.LabelLeft.Text);
            this.OneWayBind(ViewModel, vm => vm.Postcondition, v => v.Postcondition.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelRight, v => v.LabelRight.Text);
            this.OneWayBind(ViewModel, vm => vm.Precondition, v => v.Precondition.ViewModel);

            this.WhenAnyValue(vm => vm.IsMouseOver, vm => vm.Action.IsMouseOver, vm => vm.Postcondition.IsMouseOver,
                    vm => vm.Precondition.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2 && !t.Item3 && !t.Item4)
                .Subscribe(v => Highlight = v);
        }

        public ConditionalEffectStatementViewModel ViewModel
        {
            get => (ConditionalEffectStatementViewModel) GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConditionalEffectStatementViewModel) value;
        }
    }
}