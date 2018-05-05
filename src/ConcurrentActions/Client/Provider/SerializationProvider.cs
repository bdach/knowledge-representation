using System;
using System.Collections.Generic;
using System.Linq;
using Client.DataTransfer;
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
        /// public interface is going to be used.
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
        /// <exception cref="InvalidOperationException">Thrown when scenario owner or serializer instances are not defined.</exception>
        public void SerializeScenario()
        {
            ValidateInstances();

            try
            {
                var filepath = _scenarioSerializer.PromptSaveFile();
                if (string.IsNullOrEmpty(filepath)) return;
                var scenario = _scenarioOwner.GetCurrentScenario();
                _scenarioSerializer.Serialize(scenario, filepath);
            }
            catch (SerializationException ex)
            {
                Interactions.RaiseStatusBarError(ex.Message);
            }
        }

        /// <summary>
        /// Handles complete deserialization of scenario defined in a file
        /// and populates the <see cref="IScenarioOwner"/> implementor instance accordingly.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when scenario owner or serializer instances are not defined.</exception>
        public void DeserializeScenario()
        {
            ValidateInstances();

            Scenario scenario = null;
            LanguageSignature languageSignature = null;
            IEnumerable<IActionClauseViewModel> actionClauses = null;
            IEnumerable<IQueryClauseViewModel> queryClauses = null;

            try
            {
                var filepath = _scenarioSerializer.PromptOpenFile();
                if (string.IsNullOrEmpty(filepath)) return;
                scenario = _scenarioSerializer.Deserialize(filepath);

                languageSignature = GetLanguageSignature(scenario);
                actionClauses = GetActionClauseViewModels(scenario);
                queryClauses = GetQueryClauseViewModels(scenario);
            }
            catch (SerializationException ex)
            {
                Interactions.RaiseStatusBarError(ex.Message);
                return;
            }

            var currentSignature = Locator.Current.GetService<LanguageSignature>();
            currentSignature.Clear();
            currentSignature.Extend(languageSignature);

            _scenarioOwner.ClearActionClauses();
            _scenarioOwner.ExtendActionClauses(actionClauses, scenario.ActionDomainInput);

            _scenarioOwner.ClearQueryClauses();
            _scenarioOwner.ExtendQueryClauses(queryClauses, scenario.QuerySetInput);
        }

        /// <summary>
        /// Checks whether this <see cref="SerializationProvider"/> instance can be used
        /// for serialization and deserialization.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when scenario owner or serializer instances are not defined.</exception>
        private void ValidateInstances()
        {
            if (_scenarioOwner == null)
            {
                throw new InvalidOperationException("IScenarioOwner implementor instance is not defined");
            }

            if (_scenarioSerializer == null)
            {
                throw new InvalidOperationException("ScenarioSerializer instance is not defined");
            }
        }

        /// <inheritdoc />
        /// <exception cref="SerializationException">Thrown when rebuilding of the scenario from provided
        /// <see cref="Scenario"/> instance failed or is not possible.</exception>
        public LanguageSignature GetLanguageSignature(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new SerializationException("Rebuilding of the provided scenario is not possible");
            }

            return _languageSignature ?? (_languageSignature = RebuildLanguageSignature(scenario));
        }

        /// <summary>
        /// Rebuilds the language signature based on given <see cref="scenario"/>.
        /// </summary>
        /// <param name="scenario">Scenario instance to be processed.</param>
        /// <returns><see cref="LanguageSignature"/> instance.</returns>
        /// <exception cref="SerializationException">Thrown when rebuilding of the scenario from provided
        /// <see cref="Scenario"/> instance failed or is not possible.</exception>
        private LanguageSignature RebuildLanguageSignature(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new SerializationException("Rebuilding of the provided scenario is not possible");
            }

            var languageSignature = new LanguageSignature();
            languageSignature.ActionViewModels.AddRange(scenario.Actions.Select(action => new ActionViewModel(action)));
            languageSignature.LiteralViewModels.AddRange(scenario.Fluents.Select(fluent => new LiteralViewModel(fluent)));
            return languageSignature;
        }

        /// <inheritdoc />
        /// <exception cref="SerializationException">Thrown when rebuilding of the scenario from provided
        /// <see cref="Scenario"/> instance failed or is not possible.</exception>
        public IEnumerable<IActionClauseViewModel> GetActionClauseViewModels(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new SerializationException("Rebuilding of the provided scenario is not possible");
            }

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
        /// <exception cref="SerializationException">Thrown when rebuilding of the scenario from provided
        ///  <see cref="Scenario"/> instance failed or is not possible.</exception>
        public IEnumerable<IQueryClauseViewModel> GetQueryClauseViewModels(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new SerializationException("Rebuilding of the provided scenario is not possible");
            }

            var languageSignature = GetLanguageSignature(scenario);
            var queryClauses = new List<IQueryClauseViewModel>();
            queryClauses.AddRange(scenario.QuerySet.AccessibilityQueries.Select(query => Visit(query, languageSignature)));
            queryClauses.AddRange(scenario.QuerySet.ExistentialExecutabilityQueries.Select(query => Visit(query, languageSignature)));
            queryClauses.AddRange(scenario.QuerySet.ExistentialValueQueries.Select(query => Visit(query, languageSignature)));
            queryClauses.AddRange(scenario.QuerySet.GeneralExecutabilityQueries.Select(query => Visit(query, languageSignature)));
            queryClauses.AddRange(scenario.QuerySet.GeneralValueQueries.Select(query => Visit(query, languageSignature)));
            return queryClauses;
        }

        #region ActionDomain visits

        /// <summary>
        /// Visits provided <see cref="ConstraintStatement"/> instance and converts it
        /// to a corresponding view model preserving references to actions and fluents
        /// from the supplied <see cref="languageSignature"/>.
        /// </summary>
        /// <param name="constraintStatement">Statement to visit.</param>
        /// <param name="languageSignature">Scenario's language signature.</param>
        /// <returns>Appropriate <see cref="IActionClauseViewModel"/> implementor instance.</returns>
        /// <exception cref="SerializationException">Thrown when one of the statement members is missing or cannot be found in the language signature.</exception>
        private IActionClauseViewModel Visit(ConstraintStatement constraintStatement, LanguageSignature languageSignature)
        {
            if (constraintStatement.Constraint == null)
            {
                throw new SerializationException("Constraint condition in a constraint statement is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the statement members is missing or cannot be found in the language signature.</exception>
        private IActionClauseViewModel Visit(EffectStatement effectStatement, LanguageSignature languageSignature)
        {
            if (effectStatement.Action == null)
            {
                throw new SerializationException("Action in an effect statement is not defined");
            }

            if (effectStatement.Precondition == null)
            {
                throw new SerializationException("Precondition in an effect statement is not defined");
            }

            if (effectStatement.Postcondition == null)
            {
                throw new SerializationException("Postcondition in an effect statement is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the statement members is missing or cannot be found in the language signature.</exception>
        private IActionClauseViewModel Visit(FluentReleaseStatement fluentReleaseStatement, LanguageSignature languageSignature)
        {
            if (fluentReleaseStatement.Action == null)
            {
                throw new SerializationException("Action in a fluent release statement is not defined");
            }

            if (fluentReleaseStatement.Fluent == null)
            {
                throw new SerializationException("Fluent in a fluent release statement is not defined");
            }

            if (fluentReleaseStatement.Precondition == null)
            {
                throw new SerializationException("Precondition in a fluent release statement is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the statement members is missing or cannot be found in the language signature.</exception>
        private IActionClauseViewModel Visit(FluentSpecificationStatement fluentSpecificationStatement, LanguageSignature languageSignature)
        {
            if (fluentSpecificationStatement.Fluent == null)
            {
                throw new SerializationException("Fluent in a fluent specification statement is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the statement members is missing or cannot be found in the language signature.</exception>
        private IActionClauseViewModel Visit(InitialValueStatement initialValueStatement, LanguageSignature languageSignature)
        {
            if (initialValueStatement.InitialCondition == null)
            {
                throw new SerializationException("Initial condition in an initial statement is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the statement members is missing or cannot be found in the language signature.</exception>
        private IActionClauseViewModel Visit(ObservationStatement observationStatement, LanguageSignature languageSignature)
        {
            if (observationStatement.Action == null)
            {
                throw new SerializationException("Action in an observation statement is not defined");
            }

            if (observationStatement.Condition == null)
            {
                throw new SerializationException("Condition in an observation statement is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the statement members is missing or cannot be found in the language signature.</exception>
        private IActionClauseViewModel Visit(ValueStatement valueStatement, LanguageSignature languageSignature)
        {
            if (valueStatement.Action == null)
            {
                throw new SerializationException("Action in a value statement is not defined");
            }

            if (valueStatement.Condition == null)
            {
                throw new SerializationException("Condition in a value statement is not defined");
            }

            var actionViewModel = languageSignature.ActionViewModels.FirstOrDefault(vm => vm.Action.Name.Equals(valueStatement.Action.Name));
            if (actionViewModel == null)
            {
                throw new SerializationException($"Action with name \"{valueStatement.Action.Name}\" was not found in the language signature");
            }

            return new ValueStatementViewModel
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
        /// <exception cref="SerializationException">Thrown when one of the query members is missing or cannot be found in the language signature.</exception>
        private IQueryClauseViewModel Visit(AccessibilityQuery accessibilityQuery, LanguageSignature languageSignature)
        {
            if (accessibilityQuery.Target == null)
            {
                throw new SerializationException("Target in an accessibility query is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the query members is missing or cannot be found in the language signature.</exception>
        private IQueryClauseViewModel Visit(ExistentialExecutabilityQuery existentialExecutabilityQuery, LanguageSignature languageSignature)
        {
            if (existentialExecutabilityQuery.Program == null)
            {
                throw new SerializationException("Program in an existential executability query is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the query members is missing or cannot be found in the language signature.</exception>
        private IQueryClauseViewModel Visit(ExistentialValueQuery existentialValueQuery, LanguageSignature languageSignature)
        {
            if (existentialValueQuery.Target == null)
            {
                throw new SerializationException("Target in an existential value query is not defined");
            }

            if (existentialValueQuery.Program == null)
            {
                throw new SerializationException("Program in an existential value query is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the query members is missing or cannot be found in the language signature.</exception>
        private IQueryClauseViewModel Visit(GeneralExecutabilityQuery generalExecutabilityQuery, LanguageSignature languageSignature)
        {
            if (generalExecutabilityQuery.Program == null)
            {
                throw new SerializationException("Program in an general executability query is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the query members is missing or cannot be found in the language signature.</exception>
        private IQueryClauseViewModel Visit(GeneralValueQuery generalValueQuery, LanguageSignature languageSignature)
        {
            if (generalValueQuery.Target == null)
            {
                throw new SerializationException("Target in a general value query is not defined");
            }

            if (generalValueQuery.Program == null)
            {
                throw new SerializationException("Program in a general value query is not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the formula members is missing or cannot be found in the language signature.</exception>
        private IFormulaViewModel Visit(IFormula formula, LanguageSignature languageSignature)
        {
            switch (formula)
            {
                case Alternative alternative:
                    if (alternative.Left == null)
                    {
                        throw new SerializationException("Left formula of an alternative is not defined");
                    }

                    if (alternative.Right == null)
                    {
                        throw new SerializationException("Right formula of an alternative is not defined");
                    }

                    return new AlternativeViewModel
                    {
                        Left = Visit(alternative.Left, languageSignature),
                        Right = Visit(alternative.Right, languageSignature)
                    };
                case Conjunction conjunction:
                    if (conjunction.Left == null)
                    {
                        throw new SerializationException("Left formula of a conjunction is not defined");
                    }

                    if (conjunction.Right == null)
                    {
                        throw new SerializationException("Right formula of a conjunction is not defined");
                    }

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
                    if (equivalence.Left == null)
                    {
                        throw new SerializationException("Left formula of an equivalence is not defined");
                    }

                    if (equivalence.Right == null)
                    {
                        throw new SerializationException("Right formula of an equivalence is not defined");
                    }

                    return new EquivalenceViewModel
                    {
                        Left = Visit(equivalence.Left, languageSignature),
                        Right = Visit(equivalence.Right, languageSignature)
                    };
                case Implication implication:
                    if (implication.Antecedent == null)
                    {
                        throw new SerializationException("Antecedent of an implication is not defined");
                    }
                    
                    if (implication.Consequent == null)
                    {
                        throw new SerializationException("Consequent of an implication is not defined");
                    }

                    return new ImplicationViewModel
                    {
                        Antecedent = Visit(implication.Antecedent, languageSignature),
                        Consequent = Visit(implication.Consequent, languageSignature)
                    };
                case Literal literal:
                    if (literal.Fluent == null)
                    {
                        throw new SerializationException("Literal does not have any fluent assigned");
                    }

                    var literalViewModel = languageSignature.LiteralViewModels.FirstOrDefault(vm => vm.Fluent.Name.Equals(literal.Fluent.Name));
                    if (literalViewModel == null)
                    {
                        throw new SerializationException($"Fluent with name \"{literal.Fluent.Name}\" was not found in the language signature");
                    }

                    return new LiteralViewModel(literalViewModel.Fluent);
                case Negation negation:
                    if (negation.Formula == null)
                    {
                        throw new SerializationException("Formula inside a negation is not defined");
                    }

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
        /// <exception cref="SerializationException">Thrown when one of the program members is missing or cannot be found in the language signature.</exception>
        private ProgramViewModel Visit(Program program, LanguageSignature languageSignature)
        {
            if (program.Actions == null)
            {
                throw new SerializationException("Compound actions of a program are not defined");
            }

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
        /// <exception cref="SerializationException">Thrown when one of the compound action members is missing or cannot be found in the language signature.</exception>
        private CompoundActionViewModel Visit(CompoundAction compoundAction, LanguageSignature languageSignature)
        {
            if (compoundAction.Actions == null)
            {
                throw new SerializationException("Actions of a compound action are not defined");
            }

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