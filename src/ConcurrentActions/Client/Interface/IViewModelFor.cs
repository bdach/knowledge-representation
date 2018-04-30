namespace Client.Interface
{
    /// <summary>
    /// Generic interface implemented by every view model to retrieve the underlying model.
    /// </summary>
    /// <typeparam name="T">Model type corresponding to the view model.</typeparam>
    public interface IViewModelFor<out T>
    {
        /// <summary>
        /// Gets the underlying model of the view model.
        /// </summary>
        /// <returns>Model represented by given view model.</returns>
        T ToModel();

        /// <summary>
        /// Property determining whether the current node in the view model tree is focused.
        /// </summary>
        bool IsFocused { get; set; }
    }
}