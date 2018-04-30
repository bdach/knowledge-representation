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
    /// View model for <see cref="NegationView"/> which represents a logical negation.
    /// </summary>
    public class NegationViewModel : FodyReactiveObject, IViewModelFor<IFormula>
    {
        /// <summary>
        /// Prefix used for rendering the view.
        /// </summary>
        public string Prefix => "\u00AC(";

        /// <summary>
        /// Suffix used for rendering the view.
        /// </summary>
        public string Suffix => ")";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a formula to negate.
        /// </summary>
        public IViewModelFor<IFormula> Formula { get; set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IViewModelFor<IFormula>, Unit> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <summary>
        /// Initializes a new <see cref="NegationViewModel"/> instance.
        /// </summary>
        public NegationViewModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="NegationViewModel"/> instance
        /// with the supplied <see cref="formula"/>.
        /// </summary>
        /// <param name="formula">Formula to negate.</param>
        public NegationViewModel(IViewModelFor<IFormula> formula)
        {
            Formula = formula;

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

        /// <summary>
        /// Gets the underlying formula model out of the view model.
        /// </summary>
        /// <returns><see cref="Negation"/> model represented by given view model as <see cref="IFormula"/>.</returns>
        public IFormula ToModel()
        {
            var formula = Formula?.ToModel();

            if (formula == null)
                throw new MemberNotDefinedException("Formula inside a negation is not defined");

            return new Negation(formula);
        }
    }
}