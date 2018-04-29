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
    public class NegationViewModel : FodyReactiveObject, IFormulaViewModel
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
        /// The <see cref="IFormulaViewModel"/> instance to negate.
        /// </summary>
        public IFormulaViewModel Formula { get; set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

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
        public NegationViewModel(IFormulaViewModel formula)
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
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        /// <summary>
        /// Gets the underlying formula model out of the view model.
        /// </summary>
        /// <returns><see cref="Negation"/> model represented by given view model.</returns>
        public IFormula ToModel()
        {
            if (Formula == null)
                throw new MemberNotDefinedException("Formula inside a negation is not defined");

            return new Negation(Formula.ToModel());
        }
    }
}