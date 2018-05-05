using Client.Exception;
using Client.ViewModel.Terminal;

namespace Client.Interface
{
    /// <summary>
    /// Generic interface implemented by every view model to retrieve the underlying model.
    /// </summary>
    /// <typeparam name="T">Model type corresponding to the view model.</typeparam>
    public interface IViewModelFor<out T> : IEditorViewModel
    {
        /// <summary>
        /// Gets the underlying model of the view model.
        /// </summary>
        /// <returns>Model represented by given view model.</returns>
        /// <remarks>
        /// Should throw <see cref="MemberNotDefinedException"/> if one of the members
        /// is null or a <see cref="PlaceholderViewModel"/> instance.
        /// </remarks>
        T ToModel();
    }
}