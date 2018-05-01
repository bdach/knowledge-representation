using System;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.Formula;
using Client.ViewModel.Terminal;
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
        /// The logical operator character used for rendering the view.
        /// </summary>
        public string Operator => "\u2192";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning the antecedent (premise) formula of the implication.
        /// </summary>
        public IFormulaViewModel Antecedent { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning the consequent formula of the implication.
        /// </summary>
        public IFormulaViewModel Consequent { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; protected set; }

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
            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(
                InsertFormula,
                null,
                RxApp.MainThreadScheduler
            );
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(form => form != null)
                .InvokeCommand(this, vm => vm.Antecedent.AddFormula);
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(form => form != null)
                .InvokeCommand(this, vm => vm.Consequent.AddFormula);
        }

        private IFormulaViewModel InsertFormula(IFormulaViewModel formula)
        {
            if (Antecedent.IsFocused)
            {
                Antecedent = formula.Accept(Antecedent);
                return null;
            }
            if (Consequent.IsFocused)
            {
                Consequent = formula.Accept(Consequent);
                return null;
            }
            return formula;
        }

        /// <inheritdoc />
        public IFormulaViewModel Accept(IFormulaViewModel existingFormula)
        {
            return new ImplicationViewModel(existingFormula);
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