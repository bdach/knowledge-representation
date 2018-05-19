using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.ViewModel;
using Client.ViewModel.Grammar;
using ReactiveUI;

namespace Client.View.Grammar
{
    /// <summary>
    /// Interaction logic for QueryResultView.xaml
    /// </summary>
    public partial class QueryResultView : IViewFor<QueryResultViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(QueryResultViewModel),
                typeof(QueryResultView),
                new PropertyMetadata(default(QueryResultViewModel))
            );

        public QueryResultView()
        {
            InitializeComponent();
            this.Bind(ViewModel, vm => vm.Query, v => v.Query.Text);
            this.Bind(ViewModel, vm => vm.Result, v => v.Result.Text);

            this.WhenAnyValue(v => v.Result.Text)
                .Where(v => v != null)
                .Subscribe(v => { this.Result.Foreground = v.Equals(true.ToString()) ? Brushes.Green : Brushes.Red; });
        }

        public QueryResultViewModel ViewModel
        {
            get => (QueryResultViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (QueryResultViewModel)value;
        }
    }
}
