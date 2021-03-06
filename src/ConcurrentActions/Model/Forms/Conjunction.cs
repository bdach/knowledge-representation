﻿using System.Collections.Generic;
using System.Linq;

namespace Model.Forms
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing a logical conjunction of two formulae.
    /// </summary>
    public class Conjunction : IFormula
    {
        /// <summary>
        /// The left <see cref="IFormula"/> instance.
        /// </summary>
        public IFormula Left { get; set; }

        /// <summary>
        /// The right <see cref="IFormula"/> instance.
        /// </summary>
        public IFormula Right { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public Conjunction() { }

        /// <summary>
        /// Initializes a new <see cref="Conjunction"/> instance, representing a logical conjunction of the supplied <see cref="IFormula"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="IFormula"/> instance.</param>
        /// <param name="right">The right <see cref="IFormula"/> instance.</param>
        public Conjunction(IFormula left, IFormula right)
        {
            Left = left;
            Right = right;
        }

        /// <inheritdoc />
        public bool Evaluate(IState state)
        {
            return Left.Evaluate(state) && Right.Evaluate(state);
        }

        public IFormula Accept(IFormulaVisitor visitor)
        {
            return visitor.Visit(this);
        }

        /// <inheritdoc />
        public IEnumerable<Fluent> Fluents => Left.Fluents.Concat(Right.Fluents).Distinct();

        public override string ToString()
        {
            return $"({Left} \u2227 {Right})";
        }

        protected bool Equals(Conjunction other)
        {
            return Equals(Left, other.Left) && Equals(Right, other.Right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Conjunction) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Left != null ? Left.GetHashCode() : 0) * 397) ^ (Right != null ? Right.GetHashCode() : 0);
            }
        }
    }
}