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