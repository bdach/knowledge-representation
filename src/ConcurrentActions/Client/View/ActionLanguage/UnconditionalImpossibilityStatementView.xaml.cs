using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for UnconditionalImpossibilityStatementView.xaml
    /// </summary>
    public partial class UnconditionalImpossibilityStatementView : IViewFor<UnconditionalImpossibilityStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(UnconditionalImpossibilityStatementViewModel),
                typeof(UnconditionalImpossibilityStatementView),
                new PropertyMetadata(default(UnconditionalImpossibilityStatementViewModel))
            );

        public UnconditionalImpossibilityStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.Action, v => v.Action.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Action.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2)
                .Subscribe(v => Highlight = v);
        }

        public UnconditionalImpossibilityStatementViewModel ViewModel
        {
            get => (UnconditionalImpossibilityStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (UnconditionalImpossibilityStatementViewModel)value;
        }
    }
}