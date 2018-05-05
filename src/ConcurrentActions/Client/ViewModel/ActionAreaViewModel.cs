using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model.ActionLanguage;
using ReactiveUI;

namespace Client.ViewModel
{
    /// <inheritdoc />
    /// <summary>
    /// View model for <see cref="ActionAreaView"/> which manages the list of actions.
    /// </summary>
    public class ActionAreaViewModel : FodyReactiveObject
    {
        /// <summary>
        /// Collection containing all Action Language clauses.
        /// </summary>
        public ReactiveList<IActionClauseViewModel> ActionDomain { get; protected set; }

        /// <summary>
        /// String bound to user input from the ActionAreaBox.
        /// </summary>
        public string ActionDomainInput { get; set; } = "";

        /// <summary>
        /// Command used for adding an action to an item of this collection.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction;

        /// <summary>
        /// Command used for adding a fluent to an item of this collection.
        /// </summary>
        public ReactiveCommand<LiteralViewModel, Unit> AddFluent;

        /// <summary>
        /// Command used for adding a formula to an item of this collection.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula;

        /// <summary>
        /// Command triggered by delete key used to delete currently selected clause element.
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Indicates whether the grammar tab is selected.
        /// </summary>
        public bool GrammarMode { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ActionAreaViewModel"/> instance.
        /// </summary>
        public ActionAreaViewModel()
        {
            ActionDomain = new ReactiveList<IActionClauseViewModel>();

            AddAction = ReactiveCommand.Create((ActionViewModel ac) =>
            {
                foreach (var viewModel in ActionDomain)
                {
                    Observable.Start(() => ac).InvokeCommand(viewModel.AddAction);
                }
            }, null, RxApp.MainThreadScheduler);  // WARNING: do not remove this, lest you get threading errors

            AddFluent = ReactiveCommand.Create((LiteralViewModel fl) =>
            {
                foreach (var viewModel in ActionDomain)
                {
                    Observable.Start(() => fl).InvokeCommand(viewModel.AddFluent);
                }
            }, null, RxApp.MainThreadScheduler);

            AddFormula = ReactiveCommand.Create((IFormulaViewModel form) =>
            {
                foreach (var viewModel in ActionDomain)
                {
                    Observable.Start(() => form).InvokeCommand(viewModel.AddFormula);
                }
            }, null, RxApp.MainThreadScheduler);

            DeleteFocused = ReactiveCommand.Create(() =>
            {
                foreach (var viewModel in ActionDomain)
                {
                    if (viewModel.IsFocused)
                    {
                        ActionDomain.Remove(viewModel);
                        return;
                    }
                }

                // these foreach loops are separated to make sure that we propagate deletion only if no clauses are selected
                foreach (var viewModel in ActionDomain)
                {
                    Observable.Start(() => Unit.Default).InvokeCommand(viewModel.DeleteFocused);
                }
            }, null, RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// Converts all action clauses to corresponding models and combines
        /// them into <see cref="ActionDomain"/> instance.
        /// </summary>
        /// <returns><see cref="ActionDomain"/> instance with all action clauses from the current scenario.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        public ActionDomain GetActionDomainModel()
        {
            var actionDomain = new ActionDomain();

            foreach (var actionClause in ActionDomain)
            {
                switch (actionClause)
                {
                    case IViewModelFor<ConstraintStatement> constraintStatement:
                        actionDomain.ConstraintStatements.Add(constraintStatement.ToModel());
                        break;
                    case IViewModelFor<EffectStatement> effectStatement:
                        actionDomain.EffectStatements.Add(effectStatement.ToModel());
                        break;
                    case IViewModelFor<FluentReleaseStatement> fluentReleaseStatement:
                        actionDomain.FluentReleaseStatements.Add(fluentReleaseStatement.ToModel());
                        break;
                    case IViewModelFor<FluentSpecificationStatement> fluentSpecificationStatement:
                        actionDomain.FluentSpecificationStatements.Add(fluentSpecificationStatement.ToModel());
                        break;
                    case IViewModelFor<InitialValueStatement> initialValueStatement:
                        actionDomain.InitialValueStatements.Add(initialValueStatement.ToModel());
                        break;
                    case IViewModelFor<ObservationStatement> observationStatement:
                        actionDomain.ObservationStatements.Add(observationStatement.ToModel());
                        break;
                    case IViewModelFor<ValueStatement> valueStatement:
                        actionDomain.ValueStatements.Add(valueStatement.ToModel());
                        break;
                }
            }

            return actionDomain;
        }
    }
}