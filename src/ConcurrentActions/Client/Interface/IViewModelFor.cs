namespace Client.Interface
{
    /// <summary>
    /// Generic interface implemented by every view model to retrieve the underlying model.
    /// </summary>
    /// <typeparam name="T">model type corresponding to the view model</typeparam>
    public interface IViewModelFor<out T>
    {
        /// <summary>
        /// Gets the underlying model of the view model.
        /// </summary>
        /// <returns></returns>
        T ToModel();
    }
}