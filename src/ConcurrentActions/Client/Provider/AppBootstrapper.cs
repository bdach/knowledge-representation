using System;
using System.Reflection;
using Client.Abstract;
using Client.Global;
using Client.View;
using Client.ViewModel;
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
            RegisterScenarioContainer();
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
        /// Initializes the application scenario container.
        /// </summary>
        private void RegisterScenarioContainer()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new ScenarioContainer(), typeof(ScenarioContainer));
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