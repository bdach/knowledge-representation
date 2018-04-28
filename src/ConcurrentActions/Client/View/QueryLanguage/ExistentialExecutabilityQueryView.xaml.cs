using System.Windows;
using Client.ViewModel.QueryLanguage;
using ReactiveUI;

namespace Client.View.QueryLanguage
{
    /// <summary>
    /// Interaction logic for ExistentialExecutabilityQueryView.xaml
    /// </summary>
    public partial class ExistentialExecutabilityQueryView : IViewFor<ExistentialExecutabilityQueryViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ExistentialExecutabilityQueryViewModel),
                typeof(ExistentialExecutabilityQueryView),
                new PropertyMetadata(default(ExistentialExecutabilityQueryViewModel))
            );

        public ExistentialExecutabilityQueryView()
        {
            InitializeComponent();
        }

        public ExistentialExecutabilityQueryViewModel ViewModel
        {
            get => (ExistentialExecutabilityQueryViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ExistentialExecutabilityQueryViewModel)value;
        }
    }
}