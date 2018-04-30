﻿using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Global;
using Client.View.Modal;
using Client.ViewModel.Formula;
using ReactiveUI;
using Splat;

namespace Client.ViewModel.Modal
{
    /// <summary>
    /// View model for <see cref="FluentModalView"/> which handles modal adding.
    /// </summary>
    public class FluentModalViewModel : FodyReactiveObject
    {
        /// <summary>
        /// A global container instance holding the fluents currently in the scenario.
        /// </summary>
        private readonly ScenarioContainer _scenarioContainer;

        /// <summary>
        /// Backing <see cref="Model.Fluent"/> instance.
        /// </summary>
        private readonly Model.Fluent _fluent;

        /// <summary>
        /// The name of the fluent being added.
        /// </summary>
        /// <remarks>
        /// Make note that this is the property we're binding to in the view - we're not binding to the backing fluent property.
        /// This is because <see cref="Model.Fluent"/> does not implement <see cref="System.ComponentModel.INotifyPropertyChanged"/>.
        /// </remarks>
        public string FluentName
        {
            get => _fluent.Name;
            set => _fluent.Name = value;
        }

        /// <summary>
        /// Command responsible for adding the currently edited fluent into the collection.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddFluent { get; }

        /// <summary>
        /// Command responsible for closing the modal.
        /// </summary>
        /// <remarks>
        /// This command does nothing in the view model.
        /// The view will subscribe to invocations of the command and close the window.
        /// </remarks>
        public ReactiveCommand<Unit, Unit> CloseModal { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="FluentModalViewModel"/>.
        /// </summary>
        /// <param name="scenarioContainer">Scenario container with the current language signature.</param>
        /// <remarks>
        /// Omitting the <see cref="scenarioContainer"/> parameter causes the method to fetch the instance registered in the <see cref="Locator"/>.
        /// </remarks>
        public FluentModalViewModel(ScenarioContainer scenarioContainer = null)
        {
            _scenarioContainer = scenarioContainer ?? Locator.Current.GetService<ScenarioContainer>();
            _fluent = new Model.Fluent("");

            var canAddFluent = this.WhenAnyValue(vm => vm.FluentName, vm => vm._scenarioContainer.LiteralViewModels)
                .Select(t => !string.IsNullOrEmpty(t.Item1) && !t.Item2.Any(literal => literal.Fluent.Name.Equals(t.Item1)));

            CloseModal = ReactiveCommand.Create(() => Unit.Default);

            AddFluent = ReactiveCommand.Create(
                () => _scenarioContainer.LiteralViewModels.Add(new LiteralViewModel(_fluent)),
                canAddFluent
            );

            // chain adding fluents with closing the modal
            AddFluent.InvokeCommand(CloseModal);
        }
    }
}