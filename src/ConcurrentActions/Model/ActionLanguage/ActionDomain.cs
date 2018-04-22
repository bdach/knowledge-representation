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
    }
}