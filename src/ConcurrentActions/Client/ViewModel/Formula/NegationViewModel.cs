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
        /// The <see cref="IViewModelFor{T}"/> instance returning a formula to negate.
        /// </summary>
        public IFormulaViewModel Formula { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public bool AnyChildFocused => IsFocused || Formula.AnyChildFocused;

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

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
            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);

            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.Formula.AddFormula);

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Formula.IsFocused)
                .Subscribe(_ => Formula = new PlaceholderViewModel());

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !Formula.IsFocused)
                .InvokeCommand(this, vm => vm.Formula.DeleteFocused);
        }

        /// <summary>
        /// Function used to handle formula insertion based on window focus.
        /// </summary>
        /// <param name="formula">The <see cref="IFormulaViewModel"/> instance to be inserted.</param>
        private void InsertFormula(IFormulaViewModel formula)
        {
            if (Formula.IsFocused)
            {
                Formula = formula.Accept(Formula);
            }
        }

        /// <inheritdoc />
        public IFormulaViewModel Accept(IFormulaViewModel existingFormula)
        {
            existingFormula.IsFocused = false;
            return new NegationViewModel(existingFormula);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the underlying formula model out of the view model.
        /// </summary>
        /// <returns><see cref="Negation"/> model represented by given view model as <see cref="IFormula"/>.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        public IFormula ToModel()
        {
            var formula = Formula?.ToModel();

            if (formula == null)
                throw new MemberNotDefinedException("NegationFormulaError");

            return new Negation(formula);
        }
    }
}