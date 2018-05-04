using System.Reactive;
using Client.Abstract;
using Client.Interface;
using Model;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.Terminal
{
    /// <summary>
    /// Placeholder view model, used as a blank in unfilled statements.
    /// Always returns nulls when calling <see cref="IViewModelFor{T}.ToModel"/>.
    /// </summary>
    public class PlaceholderViewModel : FodyReactiveObject, IViewModelFor<Action>, IViewModelFor<Model.Fluent>, IFormulaViewModel
    {
        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <inheritdoc />
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; }

        public PlaceholderViewModel()
        {
            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
        }

        /// <inheritdoc />
        public IFormulaViewModel Accept(IFormulaViewModel existingFormula)
        {
            return this;
        }

        /// <inheritdoc />
        Action IViewModelFor<Action>.ToModel()
        {
            return null;
        }

        /// <inheritdoc />
        Model.Fluent IViewModelFor<Model.Fluent>.ToModel()
        {
            return null;
        }

        /// <inheritdoc />
        IFormula IViewModelFor<IFormula>.ToModel()
        {
            return null;
        }
    }
}