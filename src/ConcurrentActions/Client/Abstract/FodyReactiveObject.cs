using ReactiveUI;

namespace Client.Abstract
{
    /// <summary>
    /// <see cref="ReactiveObject"/> wrapper allowing Fody to seamlessly inject event calls into property setters.
    /// </summary>
    public abstract class FodyReactiveObject : ReactiveObject
    {
        /// <summary>
        /// Custom event invoker required to make Fody work due to ReactiveUI hiding its own calls.
        /// </summary>
        /// <param name="propertyName">Name of changed property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// Custom event invoker required to make Fody work due to ReactiveUI hiding its own calls.
        /// </summary>
        /// <param name="propertyName">Name of property being changed.</param>
        protected void OnPropertyChanging(string propertyName)
        {
            this.RaisePropertyChanging(propertyName);
        }
    }
}
