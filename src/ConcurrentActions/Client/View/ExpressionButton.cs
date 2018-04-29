using System.Windows;
using System.Windows.Controls.Primitives;

namespace Client.View
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for all controls representing an expression.
    /// </summary>
    public abstract class ExpressionButton : ButtonBase
    {
        /// <summary>
        /// Dependency property, indicating whether the button should be highlighted (upon hover).
        /// </summary>
        private static readonly DependencyPropertyKey HighlightPropertyKey = DependencyProperty.RegisterReadOnly(
            "Highlight",
            typeof(bool),
            typeof(ExpressionButton),
            new FrameworkPropertyMetadata(default(bool))
        );

        private static readonly DependencyProperty HighlightProperty = HighlightPropertyKey.DependencyProperty;

        /// <summary>
        /// Property indicating whether the button should be highlighted (upon hover).
        /// Classes inheriting from <see cref="ExpressionButton"/> should make sure to set the value of this property themselves whenever they should be highlighted.
        /// </summary>
        public bool Highlight
        {
            get => (bool) GetValue(HighlightProperty);
            protected set => SetValue(HighlightPropertyKey, value);
        }
    }
}