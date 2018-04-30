using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.QueryLanguage;
using ReactiveUI;

namespace Client.View.QueryLanguage
{
    /// <summary>
    /// Interaction logic for ExistentialValueQueryView.xaml
    /// </summary>
    public partial class ExistentialValueQueryView : IViewFor<ExistentialValueQueryViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ExistentialValueQueryViewModel),
                typeof(ExistentialValueQueryView),
                new PropertyMetadata(default(ExistentialValueQueryViewModel))
            );

        public ExistentialValueQueryView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.LabelLeft, v => v.LabelLeft.Text);
            this.OneWayBind(ViewModel, vm => vm.Target, v => v.Target.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelRight, v => v.LabelRight.Text);
            this.OneWayBind(ViewModel, vm => vm.Program, v => v.Program.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Target.IsMouseOver, v => v.Program.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2 && !t.Item3)
                .Subscribe(v => Highlight = v);
        }

        public ExistentialValueQueryViewModel ViewModel
        {
            get => (ExistentialValueQueryViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ExistentialValueQueryViewModel)value;
        }
    }
}