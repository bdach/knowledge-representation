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
    /// View model for <see cref="ConstantView"/> which represents a boolean constant.
    /// </summary>
    public class ConstantViewModel : FodyReactiveObject, IFormulaViewModel
    {
        /// <summary>
        /// Symbol used to represent the constant.
        /// </summary>
        public string Symbol => Constant.ToString();

        /// <summary>
        /// Underlying constant instance.
        /// </summary>
        public Constant Constant { get; set; }

        /// <summary>
        /// Constant label to be displayed in the application;
        /// </summary>
        public string Label => Constant.ToString();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public bool AnyChildFocused => IsFocused;

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ConstantViewModel"/> instance.
        /// </summary>
        public ConstantViewModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="ConstantViewModel"/> instance
        /// with the supplied <see cref="constant"/>.
        /// </summary>
        /// <param name="constant">Constant instance.</param>
        public ConstantViewModel(Constant constant)
        {
            Constant = constant;

            InitializeComponent();
        }

        /// <summary>
        /// Private method which contains instructions common to all constructors.
        /// </summary>
        private void InitializeComponent()
        {
            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
        }

        /// <inheritdoc />
        public IFormulaViewModel Accept(IFormulaViewModel existingFormula)
        {
            return new ConstantViewModel(Constant);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the underlying constant model out of the view model.
        /// </summary>
        /// <returns><see cref="Model.Forms.Constant"/> model represented by given view model as <see cref="IFormula"/>.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        public IFormula ToModel()
        {
            if (Constant == null)
                throw new MemberNotDefinedException("ConstantValueError");

            return Constant;
        }
    }
}