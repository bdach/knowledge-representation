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
    /// View model for <see cref="LiteralView"/> which represents a fluent.
    /// </summary>
    public class LiteralViewModel : FodyReactiveObject, IFormulaViewModel, IViewModelFor<Model.Fluent>
    {
        /// <summary>
        /// The <see cref="Model.Fluent"/> with which the literal instance is associated.
        /// </summary>
        public Model.Fluent Fluent { get; set; }

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
        /// Initializes a new <see cref="LiteralViewModel"/> instance.
        /// </summary>
        public LiteralViewModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="LiteralViewModel"/> instance
        /// with the supplied <see cref="fluent"/>.
        /// </summary>
        /// <param name="fluent"></param>
        public LiteralViewModel(Model.Fluent fluent)
        {
            Fluent = fluent;

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
            return new LiteralViewModel(Fluent);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the underlying literal model out of the view model.
        /// </summary>
        /// <returns><see cref="Literal"/> model represented by given view model as <see cref="IFormula"/>.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        public IFormula ToModel()
        {
            if (Fluent == null)
                throw new MemberNotDefinedException("LiteralFluentError");

            return new Literal(Fluent, false);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the underlying fluent model out of the view model.
        /// </summary>
        /// <returns><see cref="Model.Fluent"/> model represented by given view model.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        Model.Fluent IViewModelFor<Model.Fluent>.ToModel()
        {
            if (Fluent == null)
                throw new MemberNotDefinedException("LiteralFluentError");

            return Fluent;
        }
    }
}