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
    /// View model for <see cref="EquivalenceView"/> which represents a logical equivalence.
    /// </summary>
    public class EquivalenceViewModel : FodyReactiveObject, IFormulaViewModel
    {
        /// <summary>
        /// The logical operator character used for rendering the view.
        /// </summary>
        public string Operator => "\u2261";

        /// <summary>
        /// The left <see cref="IViewModelFor{T}"/> instance returning a formula.
        /// </summary>
        public IViewModelFor<IFormula> Left { get; set; }

        /// <summary>
        /// The right <see cref="IViewModelFor{T}"/> instance returning a formula.
        /// </summary>
        public IViewModelFor<IFormula> Right { get; set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IViewModelFor<IFormula>, Unit> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <summary>
        /// Initializes a new <see cref="EquivalenceViewModel"/> instance.
        /// </summary>
        public EquivalenceViewModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="EquivalenceViewModel"/> instance
        /// with the supplied <see cref="left"/> formula.
        /// </summary>
        /// <param name="left">Equivalence's left formula.</param>
        public EquivalenceViewModel(IViewModelFor<IFormula> left)
        {
            Left = left;

            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="EquivalenceViewModel"/> instance
        /// with the supplied <see cref="left"/> and <see cref="right"/> formulae.
        /// </summary>
        /// <param name="left">Equivalence's left formula.</param>
        /// <param name="right">Equivalence's right formula.</param>
        public EquivalenceViewModel(IViewModelFor<IFormula> left, IViewModelFor<IFormula> right)
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
                .Create<IViewModelFor<IFormula>>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        /// <inheritdoc />
        public IFormulaViewModel Accept(IFormulaViewModel existingFormula)
        {
            Left = existingFormula;
            return this;
        }

        /// <summary>
        /// Gets the underlying formula model out of the view model.
        /// </summary>
        /// <returns><see cref="Equivalence"/> model represented by given view model as <see cref="IFormula"/>.</returns>
        public IFormula ToModel()
        {
            var left = Left?.ToModel();
            var right = Left?.ToModel();

            if (left == null)
                throw new MemberNotDefinedException("Left formula of an equivalence is not defined");
            if (right == null)
                throw new MemberNotDefinedException("Right formula of an equivalence is not defined");

            return new Equivalence(left, right);
        }
    }
}