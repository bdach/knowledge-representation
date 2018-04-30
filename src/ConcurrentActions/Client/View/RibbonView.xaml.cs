﻿using System;
using System.Reactive.Linq;
using System.Windows;
using Client.ViewModel;
using ReactiveUI;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for RibbonView.xaml
    /// </summary>
    public partial class RibbonView : IViewFor<RibbonViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(RibbonViewModel),
                typeof(RibbonView),
                new PropertyMetadata(default(RibbonViewModel))
            );

        public RibbonView()
        {
            InitializeComponent();
            this.BindCommand(ViewModel, vm => vm.CloseWindow, v => v.CloseButton);
            this.BindCommand(ViewModel, vm => vm.SetEnglishLocale, v => v.EnglishButton);
            this.BindCommand(ViewModel, vm => vm.SetPolishLocale, v => v.PolishButton);
            this.BindCommand(ViewModel, vm => vm.ShowAddFluentModal, v => v.AddFluentButton);
            this.BindCommand(ViewModel, vm => vm.ShowAddActionModal, v => v.AddActionButton);
            
            this.OneWayBind(ViewModel, vm => vm.ScenarioContainer.LiteralViewModels, v => v.FluentsGallery.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.ScenarioContainer.ActionViewModels, v => v.ActionsGallery.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.ActionClauseTypes, v => v.ActionClauseGallery.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.QueryClauseTypes, v => v.QueryClauseGallery.ItemsSource);
            this.Bind(ViewModel, vm => vm.LocalizeGroupName, v => v.ActionClauseGallery.GroupByAdvanced);
            this.Bind(ViewModel, vm => vm.LocalizeGroupName, v => v.QueryClauseGallery.GroupByAdvanced);

            this.Bind(ViewModel, vm => vm.SelectedActionClauseType, v => v.ActionClauseGallery.SelectedValue);
            this.Bind(ViewModel, vm => vm.SelectedQueryClauseType, v => v.QueryClauseGallery.SelectedValue);

            this.WhenAnyObservable(v => v.ViewModel.ScenarioContainer.LiteralViewModels.CountChanged)
                .Select(GetVisibilityForCount)
                .Subscribe(visibility => FluentSeparator.Visibility = visibility);
            this.WhenAnyObservable(v => v.ViewModel.ScenarioContainer.ActionViewModels.CountChanged)
                .Select(GetVisibilityForCount)
                .Subscribe(visibility => ActionSeparator.Visibility = visibility);
        }

        private Visibility GetVisibilityForCount(int count)
        {
            return count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public RibbonViewModel ViewModel
        {
            get => (RibbonViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (RibbonViewModel)value;
        }
    }
}