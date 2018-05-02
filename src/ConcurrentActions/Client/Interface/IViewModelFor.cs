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
        T ToModel();
    }
}