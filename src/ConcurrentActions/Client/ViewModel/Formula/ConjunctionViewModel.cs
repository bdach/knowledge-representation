using System;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.Formula;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.Formula
{
    /// <summary>
    /// View model for <see cref="ConjunctionView"/> which represents a logical conjunction.
    /// </summary>
    public class ConjunctionViewModel : FodyReactiveObject, IFormulaViewModel
    {
        /// <summary>
        /// The logical operator character used for rendering the view.
        /// </summary>
        public string Operator => "\u2227";

        /// <summary>
        /// The left <see cref="IFormulaViewModel"/> instance.
        /// </summary>
        public IFormulaViewModel Left { get; set; }

        /// <summary>
        /// The right <see cref="IFormulaViewModel"/> instance.
        /// </summary>
        public IFormulaViewModel Right { get; set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ConjunctionViewModel"/> instance.
        /// </summary>
        public ConjunctionViewModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="ConjunctionViewModel"/> instance
        /// with the supplied <see cref="left"/> formula.
        /// </summary>
        /// <param name="left">Conjunction's left formula.</param>
        public ConjunctionViewModel(IFormulaViewModel left)
        {
            Left = left;

            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="ConjunctionViewModel"/> instance
        /// with the supplied <see cref="left"/> and <see cref="right"/> formulae.
        /// </summary>
        /// <param name="left">Conjunction's left formula.</param>
        /// <param name="right">Conjunction's right formula.</param>
        public ConjunctionViewModel(IFormulaViewModel left, IFormulaViewModel right)
        {
            Left = left;
            Right = right;

            InitializeComponent();
        }

        /// <summary>
        /// Private method which contains instructions common to all constructors.
        /// </summary>
        private void InitializeComponent()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        /// <summary>
        /// Gets the underlying formula model out of the view model.
        /// </summary>
        /// <returns><see cref="Conjunction"/> model represented by given view model.</returns>
        public IFormula ToModel()
        {
            if (Left == null)
                throw new MemberNotDefinedException("Left formula of a conjunction is not defined");
            if (Right == null)
                throw new MemberNotDefinedException("Right formula of a conjunction is not defined");

            return new Conjunction(Left.ToModel(), Right.ToModel());
        }
    }
}