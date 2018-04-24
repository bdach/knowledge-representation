using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Forms;

namespace DynamicSystem.DNF
{
    public class NaryConjunction
    {
        public List<Constant> Constants { get; }
        public List<Literal> Literals { get; }

        public NaryConjunction(List<Literal> literals, List<Constant>  constants)
        {
            Literals = literals;
            Constants = constants;
        }

        public bool IsInvalid()
        {
            return Conflict(Literals, Literals);
        }

        public bool Conflicts(NaryConjunction other)
        {
            return Conflict(Literals, other.Literals);
        }

        private static bool Conflict(List<Literal> first, List<Literal> second)
        {
            return first.Any(a => second.Any(b => a.Fluent.Equals(b.Fluent) && a.Negated != b.Negated));
        }

    }
}