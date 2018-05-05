using System;
using System.Reactive;
using ReactiveUI;

namespace Client.Global
{
    /// <summary>
    /// Class used to raise and handle user interactions (such as error messages or confirmation dialogs)
    /// </summary>
    public static class Interactions
    {
        /// <summary>
        /// Interaction used to display error message strings in the main view status bar.
        /// </summary>
        public static Interaction<string, Unit> StatusBarError { get; } = new Interaction<string, Unit>();

        /// <summary>
        /// Helper function used to display localized messages in the status bar.
        /// </summary>
        /// <param name="messageId">The key of the localized message to display.</param>
        public static void RaiseStatusBarError(string messageId)
        {
            StatusBarError.Handle(messageId).Subscribe();
        }
    }
}