using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Client.Global
{
    /// <summary>
    /// Helper class used to enumerate all implementors of a non-generic interface.
    /// </summary>
    public static class ClauseTypes
    {
        /// <summary>
        /// Returns a single instance of all implementors of a given interface type.
        /// </summary>
        /// <typeparam name="T">The interface type for which the implementor instances should be instantiated.</typeparam>
        /// <returns>A <see cref="ReadOnlyCollection{T}"/> of the instantiated types.</returns>
        public static ReadOnlyCollection<T> GetAllImplementors<T>()
        {
            var type = typeof(T);
            if (!type.IsInterface)
            {
                throw new ArgumentException("This method supports interface types only");
            }

            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            var targetTypes = allTypes
                .Where(otherType => otherType.IsClass && otherType.IsPublic && otherType.GetInterfaces().Contains(type))
                .OrderBy(otherType => otherType.Name)
                .Select(otherType => (T)Activator.CreateInstance(otherType))
                .ToList();

            return new ReadOnlyCollection<T>(targetTypes);
        }
    }
}