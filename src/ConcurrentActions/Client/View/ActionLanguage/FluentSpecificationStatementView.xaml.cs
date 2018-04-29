using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for FluentSpecificationStatementView.xaml
    /// </summary>
    public partial class FluentSpecificationStatementView : IViewFor<FluentSpecificationStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(FluentSpecificationStatementViewModel),
                typeof(FluentSpecificationStatementView),
                new PropertyMetadata(default(FluentSpecificationStatementViewModel))
            );

        public FluentSpecificationStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.Fluent, v => v.Literal.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Literal.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2)
                .Subscribe(v => Highlight = v);
        }

        public FluentSpecificationStatementViewModel ViewModel
        {
            get => (FluentSpecificationStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (FluentSpecificationStatementViewModel)value;
        }
    }
}