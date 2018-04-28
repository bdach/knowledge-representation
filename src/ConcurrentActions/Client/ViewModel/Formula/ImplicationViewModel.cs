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
    /// View model for <see cref="ImplicationView"/> which represents a logical implication.
    /// </summary>
    public class ImplicationViewModel : FodyReactiveObject, IFormulaViewModel
    {
        /// <summary>
        /// The antecedent (premise) of the implication.
        /// </summary>
        public IFormulaViewModel Antecedent { get; set; }

        /// <summary>
        /// The consequent of the implication.
        /// </summary>
        public IFormulaViewModel Consequent { get; set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ImplicationViewModel"/> instance.
        /// </summary>
        public ImplicationViewModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="ImplicationViewModel"/> instance
        /// with the supplied <see cref="antecedent"/> formula.
        /// </summary>
        /// <param name="antecedent">Implication's left formula.</param>
        public ImplicationViewModel(IFormulaViewModel antecedent)
        {
            Antecedent = antecedent;

            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="ImplicationViewModel"/> instance
        /// with the supplied <see cref="antecedent"/> and <see cref="consequent"/> formulae.
        /// </summary>
        /// <param name="antecedent">Implication's left formula.</param>
        /// <param name="consequent">Implication's right formula.</param>
        public ImplicationViewModel(IFormulaViewModel antecedent, IFormulaViewModel consequent)
        {
            Antecedent = antecedent;
            Consequent = consequent;

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
        /// <returns><see cref="Implication"/> model represented by given view model.</returns>
        public IFormula ToModel()
        {
            if (Antecedent == null)
                throw new MemberNotDefinedException("Antecedent of an implication is not defined");
            if (Consequent == null)
                throw new MemberNotDefinedException("Consequent of an implication is not defined");

            return new Implication(Antecedent.ToModel(), Consequent.ToModel());
        }
    }
}