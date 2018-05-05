using System;
using System.Reactive;
using ReactiveUI;

namespace Client.Global
{
    // TODO: docs
    public static class Interactions
    {
        public static Interaction<string, Unit> StatusBarError { get; } = new Interaction<string, Unit>();

        public static void RaiseStatusBarError(string messageId)
        {
            StatusBarError.Handle(messageId).Subscribe();
        }
    }
}