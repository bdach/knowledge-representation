﻿using System;
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
    internal class DnfFormula : IDnfFormula
    {
        private IFormula Formula { get; }
        public List<NaryConjunction> Conjunctions { get; }

        public DnfFormula(IFormula formula, List<NaryConjunction> conjunctions)
        {
            Formula = formula;
            Conjunctions = conjunctions;
        }

        public bool Evaluate(IState state)
        {
            return Formula.Evaluate(state);
        }

        public IFormula Accept(IFormulaVisitor visitor)
        {
            return Formula.Accept(visitor);
        }

        public bool Conflicts(IDnfFormula other)
        {
            return Conjunctions.Any(a => other.Conjunctions.Any(a.Conflicts));
        }

        public override string ToString()
        {
            return Formula.ToString();
        }
    }
}