using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for UnconditionalEffectStatementView.xaml
    /// </summary>
    public partial class UnconditionalEffectStatementView : IViewFor<UnconditionalEffectStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(UnconditionalEffectStatementViewModel),
                typeof(UnconditionalEffectStatementView),
                new PropertyMetadata(default(UnconditionalEffectStatementViewModel))
            );

        public UnconditionalEffectStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.Postcondition, v => v.Postcondition.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Action.IsMouseOver, v => v.Postcondition.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2 && !t.Item3)
                .Subscribe(v => Highlight = v);
        }

        public UnconditionalEffectStatementViewModel ViewModel
        {
            get => (UnconditionalEffectStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (UnconditionalEffectStatementViewModel)value;
        }
    }
}