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
        }

        /// <summary>
        /// Gets the underlying literal model out of the view model.
        /// </summary>
        /// <returns><see cref="Literal"/> model represented by given view model as <see cref="IFormula"/>.</returns>
        public IFormula ToModel()
        {
            if (Fluent == null)
                throw new MemberNotDefinedException("Literal does not have any fluent assigned");

            return new Literal(Fluent, false);
        }
        
        /// <inheritdoc />
        public IFormulaViewModel Accept(IFormulaViewModel existingFormula)
        {
            return new LiteralViewModel(Fluent);
        }

        /// <summary>
        /// Gets the underlying fluent model out of the view model.
        /// </summary>
        /// <returns><see cref="Model.Fluent"/> model represented by given view model.</returns>
        Model.Fluent IViewModelFor<Model.Fluent>.ToModel()
        {
            if (Fluent == null)
                throw new MemberNotDefinedException("Literal does not have any fluent assigned");

            return Fluent;
        }
    }
}