using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using Client.Global;
using Client.ViewModel.Modal;
using Splat;

namespace Client.View.Modal.Validation
{
    /// <summary>
    /// Validation rule used by <see cref="ActionModalView"/> to test if action name is valid.
    /// </summary>
    public class UniqueNotEmptyActionValidationRule : ValidationRule
    {
        /// <summary>
        /// Processes current <see cref="value"/> and decides if it's valid.
        /// </summary>
        /// <param name="value">Value being checked.</param>
        /// <param name="cultureInfo">Current culture information.</param>
        /// <returns><see cref="ValidationResult"/> telling whether the supplied <see cref="value"/> is valid.</returns>
        /// <remarks>
        /// <see cref="value"/> is not being used as a part of workaround to WPF x ReactiveUI binding issue.
        /// Instead, the action name is retrievied from a singleton instance of <see cref="ActionModalViewModel"/>.
        /// </remarks>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var actionModalViewModel = Locator.Current.GetService<ActionModalViewModel>();
            var name = actionModalViewModel.ActionName;

            if (string.IsNullOrEmpty(name))
            {
                return new ValidationResult(false, "This field cannot be empty!");
            }

            var currentScenario = Locator.Current.GetService<ScenarioContainer>();
            return currentScenario.ActionViewModels.Any(vm => vm.Action.Name.Equals(name))
                ? new ValidationResult(false, "Action name has to be unique!")
                : new ValidationResult(true, null);
        }
    }
}