using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using Client.Global;
using Client.ViewModel.Modal;
using Splat;

namespace Client.View.Modal.Validation
{
    /// <summary>
    /// Validation rule used by <see cref="FluentModalView"/> to test if fluent name is valid.
    /// </summary>
    public class UniqueNotEmptyFluentValidationRule : ValidationRule
    {
        /// <summary>
        /// Processes current <see cref="value"/> and decides if it's valid.
        /// </summary>
        /// <param name="value">Value being checked.</param>
        /// <param name="cultureInfo">Current culture information.</param>
        /// <returns><see cref="ValidationResult"/> telling whether the supplied <see cref="value"/> is valid.</returns>
        /// <remarks>
        /// <see cref="value"/> is not being used as a part of workaround to WPF x ReactiveUI binding issue.
        /// Instead, the fluent name is retrievied from a singleton instance of <see cref="FluentModalViewModel"/>.
        /// </remarks>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var fluentModalViewModel = Locator.Current.GetService<FluentModalViewModel>();
            var name = fluentModalViewModel.FluentName;

            if (string.IsNullOrEmpty(name))
            {
                return new ValidationResult(false, "This field cannot be empty!");
            }

            var currentScenario = Locator.Current.GetService<ScenarioContainer>();
            return currentScenario.LiteralViewModels.Any(vm => vm.Fluent.Name.Equals(name))
                ? new ValidationResult(false, "Fluent name has to be unique!")
                : new ValidationResult(true, null);
        }
    }
}