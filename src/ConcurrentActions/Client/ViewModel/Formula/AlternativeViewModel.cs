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
    /// View model for <see cref="AlternativeView"/> which represents a logical alternative.
    /// </summary>
    public class AlternativeViewModel : FodyReactiveObject, IFormulaViewModel
    {
        /// <summary>
        /// The logical operator character used for rendering the view.
        /// </summary>
        public string Operator => "\u2228";

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
        /// Initializes a new <see cref="AlternativeViewModel"/> instance.
        /// </summary>
        public AlternativeViewModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="AlternativeViewModel"/> instance
        /// with the supplied <see cref="left"/> formula.
        /// </summary>
        /// <param name="left">Alternative's left formula.</param>
        public AlternativeViewModel(IViewModelFor<IFormula> left)
        {
            Left = left;

            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="AlternativeViewModel"/> instance
        /// with the supplied <see cref="left"/> and <see cref="right"/> formulae.
        /// </summary>
        /// <param name="left">Alternative's left formula.</param>
        /// <param name="right">Alternative's right formula.</param>
        public AlternativeViewModel(IViewModelFor<IFormula> left, IViewModelFor<IFormula> right)
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
        /// <returns><see cref="Alternative"/> model represented by given view model as <see cref="IFormula"/>.</returns>
        public IFormula ToModel()
        {
            var left = Left?.ToModel();
            var right = Left?.ToModel();

            if (left == null)
                throw new MemberNotDefinedException("Left formula of an alternative is not defined");
            if (right == null)
                throw new MemberNotDefinedException("Right formula of an alternative is not defined");

            return new Alternative(left, right);
        }
    }
}