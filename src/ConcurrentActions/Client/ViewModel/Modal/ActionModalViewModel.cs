using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Global;
using Client.View.Modal;
using Client.ViewModel.Terminal;
using ReactiveUI;
using Splat;

namespace Client.ViewModel.Modal
{
    /// <summary>
    /// View model for <see cref="ActionModalView"/> which handles modal adding.
    /// </summary>
    public class ActionModalViewModel : FodyReactiveObject
    {
        /// <summary>
        /// A global container instance holding the actions currently defined in the language.
        /// </summary>
        private readonly LanguageSignature _languageSignature;

        /// <summary>
        /// Backing <see cref="Model.Action"/> instance.
        /// </summary>
        private Model.Action _action;

        /// <summary>
        /// The name of the Action being added.
        /// </summary>
        /// <remarks>
        /// Make note that this is the property we're binding to in the view - we're not binding to the backing Action property.
        /// This is because <see cref="Model.Action"/> does not implement <see cref="System.ComponentModel.INotifyPropertyChanged"/>.
        /// </remarks>
        public string ActionName
        {
            get => _action.Name;
            set => _action.Name = value;
        }

        /// <summary>
        /// Command responsible for adding the currently edited action into the collection.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddAction { get; }

        /// <summary>
        /// Command responsible for closing the modal.
        /// </summary>
        /// <remarks>
        /// This command does nothing in the view model.
        /// The view will subscribe to invocations of the command and close the window.
        /// </remarks>
        public ReactiveCommand<Unit, Unit> CloseModal { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ActionModalViewModel"/>.
        /// </summary>
        /// <param name="languageSignature">Container with the current language signature.</param>
        /// <remarks>
        /// Omitting the <see cref="languageSignature"/> parameter causes the method to fetch the instance registered in the <see cref="Locator"/>.
        /// </remarks>
        public ActionModalViewModel(LanguageSignature languageSignature = null)
        {
            _languageSignature = languageSignature ?? Locator.Current.GetService<LanguageSignature>();
            ResetViewModel();

             var canAddAction = this.WhenAnyValue(vm => vm.ActionName, vm => vm._languageSignature.ActionViewModels)
                .Select(t => !string.IsNullOrEmpty(t.Item1) && !t.Item2.Any(action => action.Action.Name.Equals(t.Item1)));

            CloseModal = ReactiveCommand.Create(() => Unit.Default);

            AddAction = ReactiveCommand.Create(
                () => _languageSignature.ActionViewModels.Add(new ActionViewModel(_action)),
                canAddAction
            );

            // chain adding actions with closing the modal
            AddAction.InvokeCommand(CloseModal);
        }

        /// <summary>
        /// Resets the current instance of <see cref="ActionModalViewModel"/>.
        /// </summary>
        /// <remarks>
        /// This was introduced as a part of workaround to WPF x ReactiveUI binding issue.
        /// </remarks>
        public void ResetViewModel()
        {
            _action = new Model.Action("");

            // since the underlying field has been changed to avoid ghost references, the event has to be raised manually
            this.RaisePropertyChanged($"ActionName");
        }
    }
}