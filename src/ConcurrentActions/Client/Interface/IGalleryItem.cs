namespace Client.Interface
{
    /// <summary>
    /// Base interface for items to be displayed in Fluent Galleries.
    /// </summary>
    public interface IGalleryItem
    {
        // TODO: get rid of this interface to have the VMs be drawn with their corresponding views in the dropdowns

        /// <summary>
        /// Gets the name to be displayed.
        /// </summary>
        string DisplayName { get; }
    }
}