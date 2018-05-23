using Model;
using Model.ActionLanguage;
using Model.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = Model.Action;

namespace DynamicSystem.NewGeneration
{
    /// <summary>
    /// Class for generating sets of literals for New.
    /// </summary>
    internal class NewSetHelper
    {
        private ActionDomain _actionDomain;
        private HashSet<Fluent> _fluents;

        /// <summary>
        /// Initializes an instance of the <see cref="NewSetHelper"/> class.
        /// </summary>
        /// <param name="actionDomain"><see cref="ActionDomain"/> containing fluent release statements.</param>
        /// <param name="fluents">Set of <see cref="Fluent"/>.</param>
        public NewSetHelper(ActionDomain actionDomain, HashSet<Fluent> fluents)
        {
            _actionDomain = actionDomain;
            _fluents = fluents;
        }

        /// <summary>
        /// Generates New, a set of literals,
        /// where either the inertial fluents changed values between states
        /// or they were released.
        /// </summary>
        /// <param name="action"><see cref="Action"/></param>
        /// <param name="from"><see cref="State"/></param>
        /// <param name="to">Next <see cref="State"/></param>
        /// <returns>Set of <see cref="Literal"/></returns>
        public IEnumerable<Literal> GetLiterals(Action action, State from, State to)
        {
            var releaseStatements = _actionDomain.FluentReleaseStatements
              .Where(e => e.Action.Equals(action) && e.Precondition.Evaluate(from)).ToList();

            var literals = _fluents.Where(f =>
                    releaseStatements.Any(e => e.Fluent.Equals(f)) ||
                    (!from[f].Equals(to[f]) && !_actionDomain.FluentSpecificationStatements.Any(e => e.Fluent.Equals(f))))
                .Select(f => new Literal(f, !to[f]));

            return literals;
        }

        /// <summary>
        /// Generates New, a set of literals, 
        /// where either the inertial fluents changed values between states
        /// or they were released.
        /// </summary>
        /// <param name="actions">Set of <see cref="Action"/></param>
        /// <param name="from"><see cref="State"/></param>
        /// <param name="to">Next <see cref="State"/></param>
        /// <returns>Set of <see cref="Literal"/></returns>
        public IEnumerable<Literal> GetLiterals(IEnumerable<Action> actions, State from, State to)
        {
            var releaseStatements = _actionDomain.FluentReleaseStatements
              .Where(e => actions.Contains(e.Action) && e.Precondition.Evaluate(from)).ToList();

            var literals = _fluents.Where(f => releaseStatements.Any(e => e.Fluent.Equals(f)) ||
                (!from[f].Equals(to[f]) && !_actionDomain.FluentSpecificationStatements.Any(e => e.Fluent.Equals(f))))
                .Select(f => new Literal(f, !to[f]));

            return literals;
        }

    }
}
