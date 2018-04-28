using System;
using System.Globalization;
using System.Reflection;
using Client.Abstract;
using Client.View;
using Client.ViewModel;
using Fluent;
using Fluent.Localization.Languages;
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
            SetLocale();
            RegisterMainWindow();
            RegisterViews();
        }

        /// <summary>
        /// Changes locale settings of Fluent Ribbon.
        /// </summary>
        private void SetLocale()
        {
            // TODO: change this if something becomes annoying AF (e.g. commas in doubles/decimals)
            // TODO: also consider translating the whole UI to english (?)
            RibbonLocalization.Current.Culture = new CultureInfo("pl-PL", false);
            RibbonLocalization.Current.Localization = new Polish();
        }

        /// <summary>
        /// Initializes the main application window with <see cref="ShellView"/> instance
        /// and corresponding <see cref="ShellViewModel"/>.
        /// </summary>
        private void RegisterMainWindow()
        {
            // TODO: we might not need this since there's no routing in the application
            //Locator.CurrentMutable.RegisterLazySingleton(() => this, typeof(IScreen)); 

            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellViewModel(), typeof(ShellViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellView(Locator.Current.GetService<ShellViewModel>()), typeof(IViewFor<ShellViewModel>));
        }

        /// <summary>
        /// Performs a runtime assembly search looking for classes implementing <see cref="IViewFor{T}"/> interface
        /// excluding <see cref="ShellView"/> that then get registered to their corresponding view models.
        /// </summary>
        private void RegisterViews()
        {
            // TODO: for some reason LINQ doesn't work with these collection (won't execute, assigns null straight away)
            // TODO: maybe someone else can get it to work
            var classTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var classType in classTypes)
            {
                var interfaceTypes = classType.GetInterfaces();
                foreach (var interfaceType in interfaceTypes)
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IViewFor<>))
                    {
                        var genericType = interfaceType.GetGenericArguments()[0];
                        if (genericType != typeof(ShellViewModel))
                        {
                            var viewInstance = Activator.CreateInstance(classType);
                            var registerViewModelType = typeof(IViewFor<>);
                            Type[] typeArgs = { genericType };
                            Locator.CurrentMutable.Register(() => viewInstance, registerViewModelType.MakeGenericType(typeArgs));
                        }
                    }
                }
            }
        }
    }
}