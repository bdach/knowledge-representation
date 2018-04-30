using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.QueryLanguage;
using ReactiveUI;

namespace Client.View.QueryLanguage
{
    /// <summary>
    /// Interaction logic for GeneralExecutabilityQueryView.xaml
    /// </summary>
    public partial class GeneralExecutabilityQueryView : IViewFor<GeneralExecutabilityQueryViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(GeneralExecutabilityQueryViewModel),
                typeof(GeneralExecutabilityQueryView),
                new PropertyMetadata(default(GeneralExecutabilityQueryViewModel))
            );

        public GeneralExecutabilityQueryView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.Program, v => v.Program.ViewModel);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Program.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2)
                .Subscribe(v => Highlight = v);
        }

        public GeneralExecutabilityQueryViewModel ViewModel
        {
            get => (GeneralExecutabilityQueryViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (GeneralExecutabilityQueryViewModel)value;
        }
    }
}