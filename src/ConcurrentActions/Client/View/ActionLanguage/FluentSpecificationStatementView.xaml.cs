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
            this.OneWayBind(ViewModel, vm => vm.Literal, v => v.Literal.ViewModel);
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