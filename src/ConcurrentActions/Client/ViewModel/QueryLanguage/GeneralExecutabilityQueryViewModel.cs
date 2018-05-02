using System;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.QueryLanguage;
using Client.ViewModel.Terminal;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    /// <summary>
    /// View model for <see cref="GeneralExecutabilityQueryView"/> which represents a general exectubaility query in the scenario.
    /// </summary>
    public class GeneralExecutabilityQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<GeneralExecutabilityQuery>
    {
        /// <summary>
        /// Keyword describing the query.
        /// </summary>
        public string Label => "executable always";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "ExecutabilityQuery";

        /// <summary>
        /// The <see cref="Model.Program"/> instance.
        /// </summary>
        public ProgramViewModel Program { get; set; } = new ProgramViewModel();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Command adding a new program.
        /// </summary>
        public ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="GeneralExecutabilityQueryViewModel"/> instance.
        /// </summary>
        public GeneralExecutabilityQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotApplicableException("General executability query does not support adding formulae"));

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new NotImplementedException());

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
        }

        /// <inheritdoc />
        public IQueryClauseViewModel NewInstance()
        {
            return new GeneralExecutabilityQueryViewModel();
        }

        /// <summary>
        /// Gets the underlying query model out of the view model.
        /// </summary>
        /// <returns><see cref="GeneralExecutabilityQuery"/> model represented by given view model.</returns>
        public GeneralExecutabilityQuery ToModel()
        {
            var program = Program?.ToModel();

            if (program == null)
                throw new MemberNotDefinedException("Program in a general executability query is not defined");

            return new GeneralExecutabilityQuery(program);
        }
    }
}