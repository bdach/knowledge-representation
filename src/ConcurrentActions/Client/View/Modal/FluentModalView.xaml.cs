using System;
using System.Windows;
using Client.ViewModel.Modal;
using ReactiveUI;
using Splat;

namespace Client.View.Modal
{
    /// <summary>
    /// Interaction logic for FluentModalView.xaml
    /// </summary>
    public partial class FluentModalView : IViewFor<FluentModalViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(FluentModalViewModel),
                typeof(FluentModalView),
                new PropertyMetadata(default(FluentModalViewModel))
            );

        public FluentModalView()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<FluentModalViewModel>();
            this.BindCommand(ViewModel, vm => vm.AddFluent, v => v.AddFluentConfirmButton);
            this.BindCommand(ViewModel, vm => vm.CloseModal, v => v.AddFluentCancelButton);
            this.Bind(ViewModel, vm => vm.FluentName, v => v.FluentNameBox.Text);
            this.Bind(ViewModel, vm => vm.FluentName, v => v.FluentNameBox.DataContext);
            ViewModel.CloseModal.Subscribe(_ => Close());
        }

        public FluentModalViewModel ViewModel
        {
            get => (FluentModalViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (FluentModalViewModel)value;
        }
    }
}
