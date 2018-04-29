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