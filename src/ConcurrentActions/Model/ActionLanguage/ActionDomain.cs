using System.Collections.Generic;

namespace Model.ActionLanguage
{
    /// <summary>
    /// Represents the action domain - a collection of action language statements.
    /// </summary>
    public class ActionDomain
    {
        /// <summary>
        /// List of all <see cref="ConstraintStatement"/>s in the domain.
        /// </summary>
        public List<ConstraintStatement> ConstraintStatements { get; }

        /// <summary>
        /// List of all <see cref="EffectStatement"/>s in the domain.
        /// </summary>
        public List<EffectStatement> EffectStatements { get; }

        /// <summary>
        /// List of all <see cref="FluentReleaseStatement"/>s in the domain.
        /// </summary>
        public List<FluentReleaseStatement> FluentReleaseStatements { get; }

        /// <summary>
        /// List of all <see cref="FluentSpecificationStatement"/>s in the domain.
        /// </summary>
        public List<FluentSpecificationStatement> FluentSpecificationStatements { get; }

        /// <summary>
        /// List of all <see cref="InitialValueStatement"/>s in the domain.
        /// </summary>
        public List<InitialValueStatement> InitialValueStatements { get; }

        /// <summary>
        /// List of all <see cref="ObservationStatement"/>s in the domain.
        /// </summary>
        public List<ObservationStatement> ObservationStatements { get; }

        /// <summary>
        /// List of all <see cref="ValueStatement"/>s in the domain.
        /// </summary>
        public List<ValueStatement> ValueStatements { get; }

        /// <summary>
        /// Initializes a new, empty instance of the <see cref="ActionDomain"/> class.
        /// </summary>
        public ActionDomain()
        {
            ConstraintStatements = new List<ConstraintStatement>();
            EffectStatements = new List<EffectStatement>();
            FluentReleaseStatements = new List<FluentReleaseStatement>();
            FluentSpecificationStatements = new List<FluentSpecificationStatement>();
            InitialValueStatements = new List<InitialValueStatement>();
            ObservationStatements = new List<ObservationStatement>();
            ValueStatements = new List<ValueStatement>();
        }

        /// <summary>
        /// Returns all unique <see cref="Fluent"/> used in this action domain
        /// </summary>
        /// <returns>Collection of unique <see cref="Fluent"/></returns>
        public IEnumerable<Fluent> Fluents()
        {
            var fluents = new HashSet<Fluent>();
            ConstraintStatements.ForEach(stmt => fluents.UnionWith(stmt.Constraint.Fluents));
            EffectStatements.ForEach(e =>
            {
                fluents.UnionWith(e.Postcondition.Fluents);
                fluents.UnionWith(e.Precondition.Fluents);
            });
            FluentReleaseStatements.ForEach(a =>
            {
                fluents.Add(a.Fluent);
                fluents.UnionWith(a.Precondition.Fluents);
            });
            FluentSpecificationStatements.ForEach(stmt => fluents.Add(stmt.Fluent));
            InitialValueStatements.ForEach(stmt => fluents.UnionWith(stmt.InitialCondition.Fluents));
            ObservationStatements.ForEach(stmt => fluents.UnionWith(stmt.Condition.Fluents));
            ValueStatements.ForEach(stmt => fluents.UnionWith(stmt.Condition.Fluents));
            return fluents;
        }

        /// <summary>
        /// Returns all unique <see cref="Action"/> used in this action domain
        /// </summary>
        /// <returns>Collection of unique <see cref="Action"/></returns>
        public IEnumerable<Action> Actions()
        {
            var actions = new HashSet<Action>();
            EffectStatements.ForEach(e => actions.Add(e.Action));
            FluentReleaseStatements.ForEach(a => actions.Add(a.Action));
            ObservationStatements.ForEach(stmt => actions.Add(stmt.Action));
            ValueStatements.ForEach(stmt => actions.Add(stmt.Action));
            return actions;
        }
    }
}