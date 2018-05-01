using System;
using System.Windows;
using Client.ViewModel.Terminal;
using ReactiveUI;

namespace Client.View.Terminal
{
    /// <summary>
    /// Interaction logic for PlaceholderView.xaml
    /// </summary>
    public partial class PlaceholderView : IViewFor<PlaceholderViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(PlaceholderViewModel), typeof(PlaceholderView), new PropertyMetadata(default(PlaceholderViewModel)));

        public PlaceholderViewModel ViewModel
        {
            get => (PlaceholderViewModel) GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public PlaceholderView()
        {
            InitializeComponent();

            this.WhenAnyValue(v => v.IsFocused)
                .BindTo(this, v => v.ViewModel.IsFocused);

            this.WhenAnyValue(v => v.IsMouseOver)
                .Subscribe(v => Highlight = v);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PlaceholderViewModel) value;
        }
    }
}
