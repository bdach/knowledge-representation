﻿using System.Windows;
using Client.ViewModel.Terminal;
using ReactiveUI;

namespace Client.View.Terminal
{
    /// <summary>
    /// Interaction logic for ActionView.xaml
    /// </summary>
    public partial class ActionView : IViewFor<ActionViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ActionViewModel),
                typeof(ActionView),
                new PropertyMetadata(default(ActionViewModel))
            );

        public ActionView()
        {
            InitializeComponent();
        }

        public ActionViewModel ViewModel
        {
            get => (ActionViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ActionViewModel)value;
        }
    }
}