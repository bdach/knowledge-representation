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
    /// View model for <see cref="ConjunctionView"/> which represents a logical conjunction.
    /// </summary>
    public class ConjunctionViewModel : FodyReactiveObject, IFormulaViewModel
    {
        /// <summary>
        /// The logical operator character used for rendering the view.
        /// </summary>
        public string Operator => "\u2227";

        /// <summary>
        /// The left <see cref="IViewModelFor{T}"/> instance returning a formula.
        /// </summary>
        public IFormulaViewModel Left { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The right <see cref="IViewModelFor{T}"/> instance returning a formula.
        /// </summary>
        public IFormulaViewModel Right { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

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
            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);

            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.Left.AddFormula);
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.Right.AddFormula);

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
            DeleteFocused.Where(_ => Left.IsFocused).Subscribe(_ => Left = new PlaceholderViewModel());
            DeleteFocused.Where(_ => Right.IsFocused).Subscribe(_ => Right = new PlaceholderViewModel());

            DeleteFocused.Where(_ => !(Left.IsFocused || Right.IsFocused))
                .InvokeCommand(this, vm => vm.Left.DeleteFocused);
            DeleteFocused.Where(_ => !(Left.IsFocused || Right.IsFocused))
                .InvokeCommand(this, vm => vm.Right.DeleteFocused);
        }

        private void InsertFormula(IFormulaViewModel formula)
        {
            if (Left.IsFocused)
            {
                Left = formula.Accept(Left);
            }
            if (Right.IsFocused)
            {
                Right = formula.Accept(Right);
            }
        }

        /// <inheritdoc />
        public IFormulaViewModel Accept(IFormulaViewModel existingFormula)
        {
            existingFormula.IsFocused = false;
            return new ConjunctionViewModel(existingFormula);
        }

        /// <summary>
        /// Gets the underlying formula model out of the view model.
        /// </summary>
        /// <returns><see cref="Conjunction"/> model represented by given view model as <see cref="IFormula"/>.</returns>
        public IFormula ToModel()
        {
            var left = Left?.ToModel();
            var right = Left?.ToModel();

            if (left == null)
                throw new MemberNotDefinedException("Left formula of a conjunction is not defined");
            if (right == null)
                throw new MemberNotDefinedException("Right formula of a conjunction is not defined");

            return new Conjunction(left, right);
        }
    }
}