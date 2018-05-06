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
    public class NewGenerator
    {
        private ActionDomain _actionDomain;
        private HashSet<Fluent> _fluents;

        public NewGenerator(ActionDomain actionDomain, HashSet<Fluent> fluents)
        {
            _actionDomain = actionDomain;
            _fluents = fluents;
        }

        /// <summary>
        /// Generates New, a set of literals,
        /// where either the inertial fluents changed values between states
        /// or they where released
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="from">State</param>
        /// <param name="to">Next state</param>
        /// <returns>Set of literals</returns>
        public IEnumerable<Literal> GetLiterals(Action action, State from, State to)
        {
            var releaseStatements = _actionDomain.FluentReleaseStatements
              .Where(e => e.Action.Equals(action) && e.Precondition.Evaluate(from)).ToList();

            // for noninertial add literal || for inertial see if value changed
            var literals = _fluents.Where(f => releaseStatements.Any(e => e.Fluent.Equals(f)) || !from[f].Equals(to[f]))
                .Select(f => new Literal(f, !to[f]));

            return literals;
        }

        public IEnumerable<Literal> GetLiterals(IEnumerable<Action> actions, State from, State to)
        {
            var releaseStatements = _actionDomain.FluentReleaseStatements
              .Where(e => actions.Contains(e.Action) && e.Precondition.Evaluate(from)).ToList();

            // for noninertial add literal || for inertial see if value changed
            var literals = _fluents.Where(f => releaseStatements.Any(e => e.Fluent.Equals(f)) || !from[f].Equals(to[f]))
                .Select(f => new Literal(f, !to[f]));

            return literals;
        }

    }
}
