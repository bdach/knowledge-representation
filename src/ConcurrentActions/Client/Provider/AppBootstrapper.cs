using System;
using System.Reflection;
using Client.Abstract;
using Client.Global;
using Client.View;
using Client.ViewModel;
using Client.ViewModel.Modal;
using ReactiveUI;
using Splat;

namespace Client.Provider
{
    /// <summary>
    /// Class responsible for app initialization and dependency injection setup.
    /// </summary>
    public class AppBootstrapper : FodyReactiveObject
    {
        /// <summary>
        /// Initializes a new <see cref="AppBootstrapper"/> instance.
        /// </summary>
        public AppBootstrapper()
        {
            LocalizationProvider.SetLocale(LocalizationProvider.Polish);
            RegisterMainWindow();
            RegisterModalViewModels();
            RegisterLanguageSignature();
            RegisterViews();
        }

        /// <summary>
        /// Initializes the main application window with <see cref="ShellView"/> instance
        /// and corresponding <see cref="ShellViewModel"/>.
        /// </summary>
        private void RegisterMainWindow()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellViewModel(), typeof(ShellViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellView(Locator.Current.GetService<ShellViewModel>()), typeof(IViewFor<ShellViewModel>));
        }

        /// <summary>
        /// Registers view models associated with modal windows in the <see cref="Locator"/>.
        /// </summary>
        /// <remarks>
        /// These were changed to singletons as a part of workaround to WPF x ReactiveUI binding issue.
        /// </remarks>
        private void RegisterModalViewModels()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new FluentModalViewModel(), typeof(FluentModalViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ActionModalViewModel(), typeof(ActionModalViewModel));
        }

        /// <summary>
        /// Initializes the application language signature container.
        /// </summary>
        private void RegisterLanguageSignature()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new LanguageSignature(), typeof(LanguageSignature));
        }

        /// <summary>
        /// Performs a runtime assembly search looking for classes implementing <see cref="IViewFor{T}"/> interface
        /// excluding <see cref="ShellView"/> that then get registered to their corresponding view models.
        /// </summary>
        private void RegisterViews()
        {
            var registerType = typeof(IViewFor<>);
            foreach (var classType in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var interfaceType in classType.GetInterfaces())
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == registerType)
                    {
                        var genericType = interfaceType.GetGenericArguments()[0];
                        if (genericType != typeof(ShellViewModel))
                        {
                            Locator.CurrentMutable.Register(() => Activator.CreateInstance(classType), registerType.MakeGenericType(genericType));
                        }
                    }
                }
            }
        }
    }
}