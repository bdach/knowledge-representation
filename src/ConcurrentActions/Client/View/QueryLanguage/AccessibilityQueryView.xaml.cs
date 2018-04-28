using System.Windows;
using Client.ViewModel.QueryLanguage;
using ReactiveUI;

namespace Client.View.QueryLanguage
{
    /// <summary>
    /// Interaction logic for AccessibilityQueryView.xaml
    /// </summary>
    public partial class AccessibilityQueryView : IViewFor<AccessibilityQueryViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(AccessibilityQueryViewModel),
                typeof(AccessibilityQueryView),
                new PropertyMetadata(default(AccessibilityQueryViewModel))
            );

        public AccessibilityQueryView()
        {
            InitializeComponent();
        }

        public AccessibilityQueryViewModel ViewModel
        {
            get => (AccessibilityQueryViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AccessibilityQueryViewModel)value;
        }
    }
}