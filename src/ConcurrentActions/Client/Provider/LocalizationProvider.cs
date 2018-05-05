using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Threading;
using Fluent;

namespace Client.Provider
{
    /// <summary>
    /// Custom translation provider which allows for runtime language change.
    /// </summary>
    // TODO: decide whether this should be a true singleton, static or whatever
    public class LocalizationProvider : INotifyPropertyChanged
    {
        /// <summary>
        /// String representing American English culture information.
        /// </summary>
        public static string AmericanEnglish = "en-US";

        /// <summary>
        /// String representing Polish culture information.
        /// </summary>
        public static string Polish = "pl-PL";

        /// <summary>
        /// Private instance of currently used culture encapsulated by <see cref="CurrentCulture"/>.
        /// </summary>
        private CultureInfo _currentCulture;

        /// <summary>
        /// Private instance of resource manager used for localization support.
        /// </summary>
        private readonly ResourceManager _resourceManager = Localization.Locale.ResourceManager;

        /// <summary>
        /// Instance 
        /// </summary>
        public static LocalizationProvider Instance { get; } = new LocalizationProvider();

        /// <summary>
        /// Localized key value accessor.
        /// </summary>
        /// <param name="key">Character string to be localized.</param>
        /// <returns>Localized version of given key.</returns>
        public string this[string key] => _resourceManager.GetString(key, _currentCulture);

        /// <summary>
        /// Represents current culture set in the application.
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (!Equals(_currentCulture, value))
                {
                    _currentCulture = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                }
            }
        }

        /// <summary>
        /// Changes locale settings of the application.
        /// </summary>
        public static void SetLocale(string cultureName)
        {

            // create a new object representing selected culture
            var culture = CultureInfo.CreateSpecificCulture(cultureName);

            // localize FLuent Ribbon
            RibbonLocalization.Current.Culture = culture;

            // localize data formats and WPF user interface on the main thread
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // localize data formats and WPF user interface on all other threads
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // trigger runtime locale reload
            Instance.CurrentCulture = culture;
        }

        /// <summary>
        /// <see cref="INotifyPropertyChanged"/> implementation.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}