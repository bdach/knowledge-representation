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
    public class ImplicationViewModel : FodyReactiveObject, IViewModelFor<IFormula>
    {
        /// <summary>
        /// The logical operator character used for rendering the view.
        /// </summary>
        public string Operator => "\u2192";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning the antecedent (premise) formula of the implication.
        /// </summary>
        public IViewModelFor<IFormula> Antecedent { get; set; }

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning the consequent formula of the implication.
        /// </summary>
        public IViewModelFor<IFormula> Consequent { get; set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IViewModelFor<IFormula>, Unit> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

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
        public ImplicationViewModel(IViewModelFor<IFormula> antecedent)
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
        public ImplicationViewModel(IViewModelFor<IFormula> antecedent, IViewModelFor<IFormula> consequent)
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
                .Create<IViewModelFor<IFormula>>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        /// <summary>
        /// Gets the underlying formula model out of the view model.
        /// </summary>
        /// <returns><see cref="Implication"/> model represented by given view model as <see cref="IFormula"/>.</returns>
        public IFormula ToModel()
        {
            var antecedent = Antecedent?.ToModel();
            var consequent = Consequent?.ToModel();

            if (antecedent == null)
                throw new MemberNotDefinedException("Antecedent of an implication is not defined");
            if (consequent == null)
                throw new MemberNotDefinedException("Consequent of an implication is not defined");

            return new Implication(antecedent, consequent);
        }
    }
}