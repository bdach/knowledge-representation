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

        public bool IsValid()
        {
            return Conflict(Literals, Literals);
        }

        public bool Conflicts(NaryConjunction other)
        {
            return Conflict(Literals, other.Literals);
        }

        private static bool Conflict(List<Literal> first, List<Literal> second)
        {
            foreach (var a in first)
            {
                foreach (var b in second)
                {
                    if (a.Fluent.Name.Equals(b.Fluent.Name) && a.Negated != b.Negated)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}