using System;
using System.Windows;
using Client.ViewModel;
using ReactiveUI;

namespace Client.View
{
    /// <summary>
    /// InterQuery logic for QueryAreaView.xaml
    /// </summary>
    public partial class QueryAreaView : IViewFor<QueryAreaViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(QueryAreaViewModel),
                typeof(QueryAreaView),
                new PropertyMetadata(default(QueryAreaViewModel))
            );

        public QueryAreaView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.QuerySet, v => v.QueryListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.QuerySetInput, v => v.QueryTextBox.Text);
            this.OneWayBind(ViewModel, vm => vm.EvaluationResults, v => v.QueryResultListView.ItemsSource);

            this.WhenAnyValue(v => v.ViewModel.GrammarMode)
                .Subscribe(grammarMode =>
                {
                    QueryListView.Visibility = grammarMode ? Visibility.Hidden : Visibility.Visible;
                    QueryTextBox.Visibility = grammarMode && !ViewModel.GrammarViewResults? Visibility.Visible : Visibility.Hidden;
                    QueryResults.Visibility = grammarMode &&
                                              ViewModel.GrammarViewResults
                        ? Visibility.Visible
                        : Visibility.Hidden;
                });
            this.WhenAnyValue(v => v.ViewModel.GrammarViewResults)
                .Subscribe(grammarResults =>
                    {
                        if (!ViewModel.GrammarMode)
                        {
                            QueryTextBox.Visibility = Visibility.Hidden;
                            QueryResults.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            QueryTextBox.Visibility = grammarResults ? Visibility.Hidden : Visibility.Visible;
                            QueryResults.Visibility = grammarResults ? Visibility.Visible : Visibility.Hidden;
                        }
                    });
            this.BindCommand(ViewModel, vm => vm.CloseQueryResults, v => v.CloseButton);

        }

        public QueryAreaViewModel ViewModel
        {
            get => (QueryAreaViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (QueryAreaViewModel)value;
        }
    }
}
