using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Model.Forms
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing the two distinguished constant formulae.
    /// </summary>
    public class Constant : IFormula
    {
        /// <summary>
        /// The inner constant value of the formula.
        /// </summary>
        private bool _value;

        /// <summary>
        /// DO NOT USE!
        /// </summary>
        /// <remarks>
        /// This was introduced as a part of workaround to XSerializer ignoring custom
        /// serialization rules issue. This property ensures proper serialization and deserialization
        /// of constants, because implementing IXmlSerializable has no effect for some reason.
        /// </remarks>
        [Obsolete("This property should be used only during deserialization, use comparison to Constant.Truth or Constant.Falsity instead")]
        public bool Value
        {
            get => _value;
            set => _value = value;
        }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public Constant() { }

        /// <summary>
        /// Initializes a new <see cref="Constant"/> instance with the supplied <see cref="value"/>.
        /// </summary>
        /// <param name="value">The inner constant value of the formula.</param>
        private Constant(bool value)
        {
            _value = value;
        }

        /// <inheritdoc />
        public bool Evaluate(IState state)
        {
            return _value;
        }

        public IFormula Accept(IFormulaVisitor visitor)
        {
            return visitor.Visit(this);
        }

        /// <inheritdoc />
        public IEnumerable<Fluent> Fluents => Enumerable.Empty<Fluent>();

        /// <summary>
        /// The truth constant formula.
        /// </summary>
        public static Constant Truth { get; } = new Constant(true);

        /// <summary>
        /// The falsity constant formula.
        /// </summary>
        public static Constant Falsity { get; } = new Constant(false);

        public override string ToString()
        {
            return _value ? "\u22A4" : "\u22A5";
        }

        protected bool Equals(Constant other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Constant) obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}