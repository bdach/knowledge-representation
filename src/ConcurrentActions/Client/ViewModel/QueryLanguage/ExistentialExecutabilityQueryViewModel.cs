using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.QueryLanguage;
using Client.ViewModel.Terminal;
using Model;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    /// <summary>
    /// View model for <see cref="ExistentialExecutabilityQueryView"/> which represents an existential exectubaility query in the scenario.
    /// </summary>
    public class ExistentialExecutabilityQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<ExistentialExecutabilityQuery>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the query.
        /// </summary>
        public static string Label => "executable sometimes";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{Label} [ ]";

        /// <summary>
        /// The <see cref="Model.Program"/> instance.
        /// </summary>
        public ProgramViewModel Program { get; set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Command adding a new program.
        /// </summary>
        public ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ExistentialExecutabilityQueryViewModel"/> instance.
        /// </summary>
        public ExistentialExecutabilityQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotApplicableException("Existential executability query does not support adding formulae"));

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new System.NotImplementedException());
        }

        /// <summary>
        /// Gets the underlying query model out of the view model.
        /// </summary>
        /// <returns><see cref="ExistentialExecutabilityQuery"/> model represented by given view model.</returns>
        public ExistentialExecutabilityQuery ToModel()
        {
            if (Program == null)
                throw new MemberNotDefinedException("Program in an existential executability query is not defined");

            return new ExistentialExecutabilityQuery(Program.ToModel());
        }
    }
}