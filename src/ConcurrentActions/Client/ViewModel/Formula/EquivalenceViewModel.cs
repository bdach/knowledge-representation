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
        public bool AnyChildFocused => IsFocused || Left.AnyChildFocused || Right.AnyChildFocused;

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

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
        public EquivalenceViewModel(IFormulaViewModel left)
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
        public EquivalenceViewModel(IFormulaViewModel left, IFormulaViewModel right)
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

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Left.IsFocused)
                .Subscribe(_ => Left = new PlaceholderViewModel());
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Right.IsFocused)
                .Subscribe(_ => Right = new PlaceholderViewModel());

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Left.IsFocused || Right.IsFocused))
                .InvokeCommand(this, vm => vm.Left.DeleteFocused);
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Left.IsFocused || Right.IsFocused))
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
            return new EquivalenceViewModel(existingFormula);
        }

        /// <summary>
        /// Gets the underlying formula model out of the view model.
        /// </summary>
        /// <returns><see cref="Equivalence"/> model represented by given view model as <see cref="IFormula"/>.</returns>
        public IFormula ToModel()
        {
            var left = Left?.ToModel();
            var right = Right?.ToModel();

            if (left == null)
                throw new MemberNotDefinedException("Left formula of an equivalence is not defined");
            if (right == null)
                throw new MemberNotDefinedException("Right formula of an equivalence is not defined");

            return new Equivalence(left, right);
        }
    }
}