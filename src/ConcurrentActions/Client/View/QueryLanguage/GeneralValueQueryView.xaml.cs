using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel.QueryLanguage;
using ReactiveUI;

namespace Client.View.QueryLanguage
{
    /// <summary>
    /// Interaction logic for GeneralValueQueryView.xaml
    /// </summary>
    public partial class GeneralValueQueryView : IViewFor<GeneralValueQueryViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(GeneralValueQueryViewModel),
                typeof(GeneralValueQueryView),
                new PropertyMetadata(default(GeneralValueQueryViewModel))
            );

        public GeneralValueQueryView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.LabelLeft, v => v.LabelLeft.Text);
            this.OneWayBind(ViewModel, vm => vm.Target, v => v.Target.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.LabelRight, v => v.LabelRight.Text);
            this.OneWayBind(ViewModel, vm => vm.Program, v => v.Program.ViewModel);

            this.WhenAnyValue(v => v.IsFocused)
                .BindTo(this, v => v.ViewModel.IsFocused);

            this.WhenAnyValue(v => v.IsMouseOver, v => v.Target.IsMouseOver, v => v.Program.IsMouseOver)
                .Select(t => t.Item1 && !t.Item2 && !t.Item3)
                .Subscribe(v => Highlight = v);
        }

        public GeneralValueQueryViewModel ViewModel
        {
            get => (GeneralValueQueryViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (GeneralValueQueryViewModel)value;
        }
    }
}