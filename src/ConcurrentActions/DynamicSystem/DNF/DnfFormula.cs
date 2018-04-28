using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Forms;

[assembly: InternalsVisibleTo("Test")]

namespace DynamicSystem.DNF
{
    /// <summary>
    /// Default implementation of <see cref="IDnfFormula"/>
    /// </summary>
    internal class DnfFormula : IDnfFormula
    {
        private IFormula Formula { get; }
        public List<NaryConjunction> Conjunctions { get; }

        /// <summary>
        /// Creates new instance of <see cref="DnfFormula"/>
        /// </summary>
        /// <param name="formula">Instance of <see cref="IFormula"/> in DNF</param>
        /// <param name="conjunctions">List of n-ary conjunctions contained in the formula</param>
        public DnfFormula(IFormula formula, List<NaryConjunction> conjunctions)
        {
            Formula = formula;
            Conjunctions = conjunctions;
        }

        /// <inheritdoc />
        public bool Evaluate(IState state)
        {
            return Formula.Evaluate(state);
        }

        /// <inheritdoc />
        public IFormula Accept(IFormulaVisitor visitor)
        {
            return Formula.Accept(visitor);
        }

        /// <inheritdoc />
        public bool Conflicts(IDnfFormula other)
        {
            return Conjunctions.Any(a => other.Conjunctions.Any(a.Conflicts));
        }

        public override string ToString()
        {
            return Formula.ToString();
        }

        protected bool Equals(DnfFormula other)
        {
            var thisConjunctions = new HashSet<NaryConjunction>(Conjunctions);
            var otherConjunctions = new HashSet<NaryConjunction>(other.Conjunctions);
            return Equals(Formula, other.Formula) && thisConjunctions.SetEquals(otherConjunctions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DnfFormula) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Formula != null ? Formula.GetHashCode() : 0) * 397) ^
                       (Conjunctions != null ? Conjunctions.GetHashCode() : 0);
            }
        }
    }
}