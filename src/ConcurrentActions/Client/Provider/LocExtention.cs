using System.Windows.Data;

namespace Client.Provider
{
    /// <summary>
    /// Custom class extending default WPF <see cref="Binding"/> functionality to support runtime localization.
    /// </summary>
    public class LocExtension : Binding
    {
        /// <summary>
        /// Used to bind localized properties.
        /// </summary>
        /// <param name="name">Name to be localized.</param>
        public LocExtension(string name) : base("[" + name + "]")
        {
            Mode = BindingMode.OneWay;
            Source = LocalizationProvider.Instance;
        }
    }
}