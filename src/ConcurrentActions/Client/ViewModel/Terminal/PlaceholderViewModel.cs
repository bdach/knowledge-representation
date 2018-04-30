using Client.Abstract;
using Client.Interface;
using Model;
using Model.Forms;

namespace Client.ViewModel.Terminal
{
    /// <summary>
    /// Placeholder view model, used as a blank in unfilled statements.
    /// Always returns nulls when calling <see cref="IViewModelFor{T}.ToModel"/>.
    /// </summary>
    public class PlaceholderViewModel : FodyReactiveObject, IViewModelFor<Action>, IViewModelFor<Model.Fluent>, IViewModelFor<IFormula>
    {

        /// <inheritdoc />
        public bool IsFocused { get; set; }

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