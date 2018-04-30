﻿using System.Windows;
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
