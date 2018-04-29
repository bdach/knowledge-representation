﻿using System.Windows;
using Client.ViewModel.ActionLanguage;
using ReactiveUI;

namespace Client.View.ActionLanguage
{
    /// <summary>
    /// Interaction logic for InitialValueStatementView.xaml
    /// </summary>
    public partial class InitialValueStatementView : IViewFor<InitialValueStatementViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(InitialValueStatementViewModel),
                typeof(InitialValueStatementView),
                new PropertyMetadata(default(InitialValueStatementViewModel))
            );

        public InitialValueStatementView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Label, v => v.Label.Text);
            this.OneWayBind(ViewModel, vm => vm.InitialCondition, v => v.InitialCondition.ViewModel);
        }

        public InitialValueStatementViewModel ViewModel
        {
            get => (InitialValueStatementViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (InitialValueStatementViewModel)value;
        }
    }
}