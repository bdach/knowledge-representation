using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel;
using ReactiveUI;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : IViewFor<ShellViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ShellViewModel),
                typeof(ShellView),
                new PropertyMetadata(default(ShellViewModel))
            );

        public ShellView(ShellViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.OneWayBind(ViewModel, vm => vm.RibbonViewModel, v => v.Ribbon.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.ActionAreaViewModel, v => v.ActionArea.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.QueryAreaViewModel, v => v.QueryArea.ViewModel);
            this.BindCommand(ViewModel, vm => vm.DeleteFocused, v => v.ShellWindow, "KeyDown");
            this.OneWayBind(ViewModel, vm => vm.StatusBarMessage, v => v.ErrorMessage.Text);
            ViewModel.RibbonViewModel.CloseWindow.Subscribe(_ => Close());

            this.WhenAnyValue(v => v.ViewModel.StatusBarMessage)
                .Select(msg => string.IsNullOrEmpty(msg) ? Visibility.Collapsed : Visibility.Visible)
                .BindTo(this, v => v.ErrorContent.Visibility);

            this.WhenAnyValue(v => v.ViewModel.StatusBarMessage)
                .Throttle(TimeSpan.FromSeconds(5), RxApp.MainThreadScheduler)
                .Subscribe(_ => ErrorContent.Visibility = Visibility.Collapsed);
        }

        public ShellViewModel ViewModel
        {
            get => (ShellViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ShellViewModel)value;
        }
    }
}
