using System;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.QueryLanguage;
using Client.ViewModel.Terminal;
using Model.Forms;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    /// <summary>
    /// View model for <see cref="GeneralExecutabilityQueryView"/> which represents a general exectubaility query in the scenario.
    /// </summary>
    public class GeneralExecutabilityQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<GeneralExecutabilityQuery>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the query.
        /// </summary>
        public string Label => "executable always";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "ExecutabilityQuery";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{Label} [ ]";

        /// <summary>
        /// The <see cref="Model.Program"/> instance.
        /// </summary>
        public ProgramViewModel Program { get; set; } = new ProgramViewModel();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IViewModelFor<IFormula>, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Command adding a new program.
        /// </summary>
        public ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="GeneralExecutabilityQueryViewModel"/> instance.
        /// </summary>
        public GeneralExecutabilityQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IViewModelFor<IFormula>>(formulaViewModel =>
                    throw new NotApplicableException("General executability query does not support adding formulae"));

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new NotImplementedException());
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