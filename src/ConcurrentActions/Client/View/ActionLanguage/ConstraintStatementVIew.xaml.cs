using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for ConstraintStatementView.xaml
    /// </summary>
    public partial class ConstraintStatementView : IViewFor<ConstraintStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ConstraintStatementViewModel),
                typeof(ConstraintStatementView),
                new PropertyMetadata(default(ConstraintStatementViewModel))
            );

        public ConstraintStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.Constraint, v => v.Constraint.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Constraint.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2)
                .Subscribe(v =>
                {
                    Highlight = v;
                });
        }

        public ConstraintStatementViewModel ViewModel
        {
            get => (ConstraintStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConstraintStatementViewModel)value;
        }
    }
}