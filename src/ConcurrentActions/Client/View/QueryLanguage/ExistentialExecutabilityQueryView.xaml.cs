using System;
using System.Reactive.Linq;
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
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.Program, v => v.Program.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Program.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2)
                .Subscribe(v => Highlight = v);
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