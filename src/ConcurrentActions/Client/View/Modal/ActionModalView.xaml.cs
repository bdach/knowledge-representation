﻿using System;
using System.Windows;
using Client.ViewModel.Modal;
using ReactiveUI;
using Splat;

namespace Client.View.Modal
{
    /// <summary>
    /// Interaction logic for ActionModalView.xaml
    /// </summary>
    public partial class ActionModalView : IViewFor<ActionModalViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ActionModalViewModel),
                typeof(ActionModalView),
                new PropertyMetadata(default(ActionModalViewModel))
            );

        public ActionModalView()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<ActionModalViewModel>();
            this.BindCommand(ViewModel, vm => vm.AddAction, v => v.AddActionConfirmButton);
            this.BindCommand(ViewModel, vm => vm.CloseModal, v => v.AddActionCancelButton);
            this.Bind(ViewModel, vm => vm.ActionName, v => v.ActionNameBox.Text);
            this.Bind(ViewModel, vm => vm.ActionName, v => v.ActionNameBox.DataContext);
            ViewModel.CloseModal.Subscribe(_ => Close());
        }

        public ActionModalViewModel ViewModel
        {
            get => (ActionModalViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ActionModalViewModel)value;
        }
    }
}
