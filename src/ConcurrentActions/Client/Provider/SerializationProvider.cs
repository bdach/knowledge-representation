using System;
using System.Collections.Generic;
using System.Linq;
using Client.Exception;
using Client.Global;
using Client.Interface;
using Client.ViewModel;
using Client.ViewModel.ActionLanguage;
using Client.ViewModel.Formula;
using Client.ViewModel.QueryLanguage;
using Client.ViewModel.Terminal;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using Model.QueryLanguage;
using Splat;

namespace Client.Provider
{
    /// <inheritdoc />
    /// <summary>
    /// Custom serialization helper used to serialize and deserialize <see cref="Scenario"/>
    /// instances. Can be also used as a plain converter from <see cref="Scenario"/> object
    /// to corresponding view models used by the editor via <see cref="IScenarioConverter"/> interface.
    /// </summary>
    /// <remarks>
    /// Instance of this class is used as an object that releases dependencies between
    /// <see cref="ShellViewModel"/> referenced with <see cref="IScenarioOwner"/> interface 
    /// and <see cref="ScenarioSerializer"/> and handles full conversion of a <see cref="Scenario"/>
    /// instance by implementing both mediator and visitor design patterns.
    /// 
    /// In case this object is going to be used as a converter only, <see cref="IScenarioOwner"/>
    /// implementor  and <see cref="ScenarioSerializer"/> instances can be set to null explicitly
    /// or by use of default (parameterless) constructor.
    /// </remarks>
    public class SerializationProvider : IScenarioConverter
    {
        /// <summary>
        /// Private <see cref="IScenarioOwner"/> implementor instance.
        /// </summary>
        private readonly IScenarioOwner _scenarioOwner;

        /// <summary>
        /// Private <see cref="ScenarioSerializer"/> instance.
        /// </summary>
        private readonly ScenarioSerializer _scenarioSerializer;

        /// <summary>
        /// Private <see cref="LanguageSignature"/> instance used during scenario conversion
        /// to ensure proper action and fluent reference links.
        /// </summary>
        /// <remarks>
        /// Each visit receives this field as a parameter in order to ensure
        /// easy modifications in the future.
        /// </remarks>
        private LanguageSignature _languageSignature;

        /// <summary>
        /// Initializes a new <see cref="SerializationProvider"/> instance.
        /// </summary>
        /// <remarks>
        /// Should be used only if just the <see cref="IScenarioConverter"/>
        /// public interface is goind to be used.
        /// </remarks>
        public SerializationProvider() { }

        /// <summary>
        /// Initializes a new <see cref="SerializationProvider"/> instance
        /// with the supplied <see cref="scenarioOwner"/> instance
        /// and a default <see cref="ScenarioSerializer"/> instance.
        /// </summary>
        /// <param name="scenarioOwner">Current <see cref="IScenarioOwner"/> implementor instance.</param>
        public SerializationProvider(IScenarioOwner scenarioOwner)
        {
            _scenarioOwner = scenarioOwner;
            _scenarioSerializer = new ScenarioSerializer();
        }

        /// <summary>
        /// Initializes a new <see cref="SerializationProvider"/> instance
        /// with the supplied <see cref="scenarioOwner"/> and <see cref="scenarioSerializer"/> instances.
        /// </summary>
        /// <param name="scenarioOwner">Current <see cref="IScenarioOwner"/> implementor instance.</param>
        /// <param name="scenarioSerializer"><see cref="ScenarioSerializer"/> instance.</param>
        public SerializationProvider(IScenarioOwner scenarioOwner, ScenarioSerializer scenarioSerializer)
        {
            _scenarioOwner = scenarioOwner;
            _scenarioSerializer = scenarioSerializer;
        }

        /// <summary>
        /// Handles complete serialization of the currently defined scenario.
        /// </summary>
        public void SerializeScenario()
        {
            ValidateInstances();

            var filepath = _scenarioSerializer.PromptSaveFile();
            if (string.IsNullOrEmpty(filepath)) return;
            var scenario = _scenarioOwner.GetCurrentScenario();
            _scenarioSerializer.Serialize(scenario, filepath);
        }

        /// <summary>
        /// Handles complete deserialization of scenario defined in a file
        /// and populates the <see cref="IScenarioOwner"/> implementor instance accordingly.
        /// </summary>
        public void DeserializeScenario()
        {
            ValidateInstances();

            var filepath = _scenarioSerializer.PromptOpenFile();
            if (string.IsNullOrEmpty(filepath)) return;
            var scenario = _scenarioSerializer.Deserialize(filepath);

            var languageSignature = GetLanguageSignature(scenario);
            var actionClauses = GetActionClauseViewModels(scenario);
            var queryClauses = GetQueryClauseViewModels(scenario);

            var currentSignature = Locator.Current.GetService<LanguageSignature>();
            currentSignature.Clear();
            currentSignature.Extend(languageSignature);

            _scenarioOwner.ClearActionClauses();
            _scenarioOwner.ExtendActionClauses(actionClauses);

            _scenarioOwner.ClearQueryClauses();
            _scenarioOwner.ExtendQueryClauses(queryClauses);
        }

        /// <summary>
        /// Checks whether this <see cref="SerializationProvider"/> instance can be used
        /// for serialization and deserialization.
        /// </summary>
        private void ValidateInstances()
        {
            if (_scenarioOwner == null)
            {
                throw new ArgumentException("IScenarioOwner implementor instance is not defined");
            }

            if (_scenarioSerializer == null)
            {
                throw new ArgumentException("ScenarioSerializer instance is not defined");
            }
        }

        /// <inheritdoc />
        public LanguageSignature GetLanguageSignature(Scenario scenario)
        {
            return _languageSignature ?? (_languageSignature = RebuildLanguageSignature(scenario));
        }

        /// <summary>
        /// Rebuilds the language signature based on given <see cref="scenario"/>.
        /// </summary>
        /// <param name="scenario">Scenario instance to be processed.</param>
        /// <returns><see cref="LanguageSignature"/> instance.</returns>
        private LanguageSignature RebuildLanguageSignature(Scenario scenario)
        {
            var languageSignature = new LanguageSignature();
            languageSignature.ActionViewModels.AddRange(scenario.Actions.Select(action => new ActionViewModel(action)));
            languageSignature.LiteralViewModels.AddRange(scenario.Fluents.Select(fluent => new LiteralViewModel(fluent)));
            return languageSignature;
        }

        /// <inheritdoc />
        public IEnumerable<IActionClauseViewModel> GetActionClauseViewModels(Scenario scenario)
        {
            var languageSignature = GetLanguageSignature(scenario);
            var actionClauses = new List<IActionClauseViewModel>();
            actionClauses.AddRange(scenario.ActionDomain.ConstraintStatements.Select(statement => Visit(statement, languageSignature)));
            actionClauses.AddRange(scenario.ActionDomain.EffectStatements.Select(statement => Visit(statement, languageSignature)));
            actionClauses.AddRange(scenario.ActionDomain.FluentReleaseStatements.Select(statement => Visit(statement, languageSignature)));
            actionClauses.AddRange(scenario.ActionDomain.FluentSpecificationStatements.Select(statement => Visit(statement, languageSignature)));
            actionClauses.AddRange(scenario.ActionDomain.InitialValueStatements.Select(statement => Visit(statement, languageSignature)));
            actionClauses.AddRange(scenario.ActionDomain.ObservationStatements.Select(statement => Visit(statement, languageSignature)));
            actionClauses.AddRange(scenario.ActionDomain.ValueStatements.Select(statement => Visit(statement, languageSignature)));
            return actionClauses;
        }

        /// <inheritdoc />
        public IEnumerable<IQueryClauseViewModel> GetQueryClauseViewModels(Scenario scenario)
        {
            var languageSignature = GetLanguageSignature(scenario);
            var queryClauses = new List<IQueryClauseViewModel>();
            queryClauses.AddRange(scenario.QuerySet.AccessibilityQueries.Select(query => Visit(query, languageSignature)));
            queryClauses.AddRange(scenario.QuerySet.ExistentialExecutabilityQueries.Select(query => Visit(query, languageSignature)));
            queryClauses.AddRange(scenario.QuerySet.ExistentialValueQueries.Select(query => Visit(query, languageSignature)));
            queryClauses.AddRange(scenario.QuerySet.GeneralExecutabilityQueries.Select(query => Visit(query, languageSignature)));
            queryClauses.AddRange(scenario.QuerySet.GeneralValueQueries.Select(query => Visit(query, languageSignature)));
            return queryClauses;
        }

        // TODO: check for nulls during visits (in case someone edited xml so that it's not invalid, but the scenario is not complete)

        #region ActionDomain visits

        /// <summary>
        /// Visits provided <see cref="ConstraintStatement"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="constraintStatement">Statement to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IActionClauseViewModel"/> implementor instance.</returns>
        private IActionClauseViewModel Visit(ConstraintStatement constraintStatement, LanguageSignature languageSignature)
        {
            return new ConstraintStatementViewModel
            {
                Constraint = Visit(constraintStatement.Constraint, languageSignature)
            };
        }

        /// <summary>
        /// Visits provided <see cref="EffectStatement"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="effectStatement">Statement to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IActionClauseViewModel"/> implementor instance.</returns>
        private IActionClauseViewModel Visit(EffectStatement effectStatement, LanguageSignature languageSignature)
        {
            var actionViewModel = languageSignature.ActionViewModels.FirstOrDefault(vm => vm.Action.Name.Equals(effectStatement.Action.Name));
            if (actionViewModel == null)
            {
                throw new SerializationException($"Action with name \"{effectStatement.Action.Name}\" was not found in the language signature");
            }

            if (effectStatement.Postcondition == Constant.Falsity)
            {
                if (effectStatement.Precondition == Constant.Truth)
                {
                    return new UnconditionalImpossibilityStatementViewModel
                    {
                        Action = new ActionViewModel(actionViewModel.Action)
                    };
                }
                else // precondition is defined
                {
                    return new ConditionalImpossibilityStatementViewModel
                    {
                        Action = new ActionViewModel(actionViewModel.Action),
                        Precondition = Visit(effectStatement.Precondition, languageSignature)
                    };
                }
            }
            else // postcondition is defined
            {
                if (effectStatement.Precondition == Constant.Truth)
                {
                    return new UnconditionalEffectStatementViewModel
                    {
                        Action = new ActionViewModel(actionViewModel.Action),
                        Postcondition = Visit(effectStatement.Postcondition, languageSignature)
                    };
                }
                else // precondition is defined
                {
                    return new ConditionalEffectStatementViewModel
                    {
                        Action = new ActionViewModel(actionViewModel.Action),
                        Precondition = Visit(effectStatement.Precondition, languageSignature),
                        Postcondition = Visit(effectStatement.Postcondition, languageSignature)
                    };
                }
            }
        }

        /// <summary>
        /// Visits provided <see cref="FluentReleaseStatement"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="fluentReleaseStatement">Statement to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IActionClauseViewModel"/> implementor instance.</returns>
        private IActionClauseViewModel Visit(FluentReleaseStatement fluentReleaseStatement, LanguageSignature languageSignature)
        {
            var actionViewModel = languageSignature.ActionViewModels.FirstOrDefault(vm => vm.Action.Name.Equals(fluentReleaseStatement.Action.Name));
            if (actionViewModel == null)
            {
                throw new SerializationException($"Action with name \"{fluentReleaseStatement.Action.Name}\" was not found in the language signature");
            }

            var literalViewModel = languageSignature.LiteralViewModels.FirstOrDefault(vm => vm.Fluent.Name.Equals(fluentReleaseStatement.Fluent.Name));
            if (literalViewModel == null)
            {
                throw new SerializationException($"Fluent with name \"{fluentReleaseStatement.Fluent.Name}\" was not found in the language signature");
            }

            if (fluentReleaseStatement.Precondition == Constant.Truth)
            {
                return new UnconditionalFluentReleaseStatementViewModel
                {
                    Action = new ActionViewModel(actionViewModel.Action),
                    Fluent = new LiteralViewModel(literalViewModel.Fluent)
                };
            }
            else // precondition is defined
            {
                return new ConditionalFluentReleaseStatementViewModel
                {
                    Action = new ActionViewModel(actionViewModel.Action),
                    Fluent = new LiteralViewModel(literalViewModel.Fluent),
                    Precondition = Visit(fluentReleaseStatement.Precondition, languageSignature)
                };
            }
        }

        /// <summary>
        /// Visits provided <see cref="FluentSpecificationStatement"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="fluentSpecificationStatement">Statement to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IActionClauseViewModel"/> implementor instance.</returns>
        private IActionClauseViewModel Visit(FluentSpecificationStatement fluentSpecificationStatement, LanguageSignature languageSignature)
        {
            var literalViewModel = languageSignature.LiteralViewModels.FirstOrDefault(vm => vm.Fluent.Name.Equals(fluentSpecificationStatement.Fluent.Name));
            if (literalViewModel == null)
            {
                throw new SerializationException($"Fluent with name \"{fluentSpecificationStatement.Fluent.Name}\" was not found in the language signature");
            }

            return new FluentSpecificationStatementViewModel
            {
                Fluent = new LiteralViewModel(literalViewModel.Fluent)
            };
        }

        /// <summary>
        /// Visits provided <see cref="InitialValueStatement"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="initialValueStatement">Statement to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IActionClauseViewModel"/> implementor instance.</returns>
        private IActionClauseViewModel Visit(InitialValueStatement initialValueStatement, LanguageSignature languageSignature)
        {
            return new InitialValueStatementViewModel
            {
                InitialCondition = Visit(initialValueStatement.InitialCondition, languageSignature)
            };
        }

        /// <summary>
        /// Visits provided <see cref="ObservationStatement"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="observationStatement">Statement to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IActionClauseViewModel"/> implementor instance.</returns>
        private IActionClauseViewModel Visit(ObservationStatement observationStatement, LanguageSignature languageSignature)
        {
            var actionViewModel = languageSignature.ActionViewModels.FirstOrDefault(vm => vm.Action.Name.Equals(observationStatement.Action.Name));
            if (actionViewModel == null)
            {
                throw new SerializationException($"Action with name \"{observationStatement.Action.Name}\" was not found in the language signature");
            }

            return new ObservationStatementViewModel
            {
                Action = new ActionViewModel(actionViewModel.Action),
                Condition = Visit(observationStatement.Condition, languageSignature)
            };
        }

        /// <summary>
        /// Visits provided <see cref="ValueStatement"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="valueStatement">Statement to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IActionClauseViewModel"/> implementor instance.</returns>
        private IActionClauseViewModel Visit(ValueStatement valueStatement, LanguageSignature languageSignature)
        {
            var actionViewModel = languageSignature.ActionViewModels.FirstOrDefault(vm => vm.Action.Name.Equals(valueStatement.Action.Name));
            if (actionViewModel == null)
            {
                throw new SerializationException($"Action with name \"{valueStatement.Action.Name}\" was not found in the language signature");
            }

            return new ObservationStatementViewModel
            {
                Action = new ActionViewModel(actionViewModel.Action),
                Condition = Visit(valueStatement.Condition, languageSignature)
            };
        }

        #endregion

        #region QuerySet visits

        /// <summary>
        /// Visits provided <see cref="AccessibilityQuery"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="accessibilityQuery">Query to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IQueryClauseViewModel"/> instance.</returns>
        private IQueryClauseViewModel Visit(AccessibilityQuery accessibilityQuery, LanguageSignature languageSignature)
        {
            return new AccessibilityQueryViewModel
            {
                Target = Visit(accessibilityQuery.Target, languageSignature)
            };
        }

        /// <summary>
        /// Visits provided <see cref="ExistentialExecutabilityQuery"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="existentialExecutabilityQuery">Query to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IQueryClauseViewModel"/> instance.</returns>
        private IQueryClauseViewModel Visit(ExistentialExecutabilityQuery existentialExecutabilityQuery, LanguageSignature languageSignature)
        {
            return new ExistentialExecutabilityQueryViewModel
            {
                Program = Visit(existentialExecutabilityQuery.Program, languageSignature)
            };
        }

        /// <summary>
        /// Visits provided <see cref="ExistentialValueQuery"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="existentialValueQuery">Query to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IQueryClauseViewModel"/> instance.</returns>
        private IQueryClauseViewModel Visit(ExistentialValueQuery existentialValueQuery, LanguageSignature languageSignature)
        {
            return new ExistentialValueQueryViewModel
            {
                Program = Visit(existentialValueQuery.Program, languageSignature),
                Target = Visit(existentialValueQuery.Target, languageSignature)
            };
        }

        /// <summary>
        /// Visits provided <see cref="GeneralExecutabilityQuery"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="generalExecutabilityQuery">Query to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IQueryClauseViewModel"/> instance.</returns>
        private IQueryClauseViewModel Visit(GeneralExecutabilityQuery generalExecutabilityQuery, LanguageSignature languageSignature)
        {
            return new GeneralExecutabilityQueryViewModel
            {
                Program = Visit(generalExecutabilityQuery.Program, languageSignature)
            };
        }

        /// <summary>
        /// Visits provided <see cref="GeneralValueQuery"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="generalValueQuery">Query to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IQueryClauseViewModel"/> instance.</returns>
        private IQueryClauseViewModel Visit(GeneralValueQuery generalValueQuery, LanguageSignature languageSignature)
        {
            return new GeneralValueQueryViewModel
            {
                Program = Visit(generalValueQuery.Program, languageSignature),
                Target = Visit(generalValueQuery.Target, languageSignature)
            };
        }

        #endregion

        #region Other visits

        /// <summary>
        /// Visits provided <see cref="IFormula"/> implementor instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="formula">Formula to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IFormulaViewModel"/> implementor instance.</returns>
        private IFormulaViewModel Visit(IFormula formula, LanguageSignature languageSignature)
        {
            switch (formula)
            {
                case Alternative alternative:
                    return new AlternativeViewModel
                    {
                        Left = Visit(alternative.Left, languageSignature),
                        Right = Visit(alternative.Right, languageSignature)
                    };
                case Conjunction conjunction:
                    return new ConjunctionViewModel
                    {
                        Left = Visit(conjunction.Left, languageSignature),
                        Right = Visit(conjunction.Right, languageSignature)
                    };
                case Constant constant:
                    return new ConstantViewModel
                    {
#pragma warning disable 618
                        // TODO: fix this during refactoring of Model.Constant
                        Constant = constant.Value ? Constant.Truth : Constant.Falsity
#pragma warning restore 618
                    };
                case Equivalence equivalence:
                    return new EquivalenceViewModel
                    {
                        Left = Visit(equivalence.Left, languageSignature),
                        Right = Visit(equivalence.Right, languageSignature)
                    };
                case Implication implication:
                    return new ImplicationViewModel
                    {
                        Antecedent = Visit(implication.Antecedent, languageSignature),
                        Consequent = Visit(implication.Consequent, languageSignature)
                    };
                case Literal literal:
                    var literalViewModel = languageSignature.LiteralViewModels.FirstOrDefault(vm => vm.Fluent.Name.Equals(literal.Fluent.Name));
                    if (literalViewModel == null)
                    {
                        throw new SerializationException($"Fluent with name \"{literal.Fluent.Name}\" was not found in the language signature");
                    }
                    return new LiteralViewModel(literalViewModel.Fluent);
                case Negation negation:
                    return new NegationViewModel
                    {
                        Formula = Visit(negation.Formula, languageSignature)
                    };
                default:
                    throw new SerializationException($"IFormula implementor of type \"{formula.GetType()}\" was not recognized");
            }
        }

        /// <summary>
        /// Visits provided <see cref="Program"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="program">Program to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="ProgramViewModel"/> instance.</returns>
        private ProgramViewModel Visit(Program program, LanguageSignature languageSignature)
        {
            var programViewModel = new ProgramViewModel();
            programViewModel.CompoundActions.AddRange(program.Actions.Select(compoundAction => Visit(compoundAction, languageSignature)));
            return programViewModel;
        }

        /// <summary>
        /// Visits provided <see cref="CompoundAction"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="compoundAction">Compound action to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="CompoundActionViewModel"/> instance.</returns>
        // BUG: after deserialization actions cannot be deleted from within a compound action (probably some hashset issue)
        private CompoundActionViewModel Visit(CompoundAction compoundAction, LanguageSignature languageSignature)
        {
            var compoundActionViewModel = new CompoundActionViewModel();
            compoundActionViewModel.Actions.AddRange(compoundAction.Actions.Select(action =>
            {
                var actionViewModel = languageSignature.ActionViewModels.FirstOrDefault(vm => vm.Action.Name.Equals(action.Name));
                if (actionViewModel == null)
                {
                    throw new SerializationException($"Action with name \"{action.Name}\" was not found in the language signature");
                }
                return new ActionViewModel(actionViewModel.Action);
            }));
            return compoundActionViewModel;
        }

        #endregion
    }
}