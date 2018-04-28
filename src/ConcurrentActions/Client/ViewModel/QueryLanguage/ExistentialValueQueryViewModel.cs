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
    /// View model for <see cref="ExistentialValueQueryView"/> which represents an existential value query in the scenario.
    /// </summary>
    public class ExistentialValueQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<ExistentialValueQuery>, IGalleryItem
    {
        /// <summary>
        /// First keyword describing the query.
        /// </summary>
        public static string LabelLeft => "possibly";

        /// <summary>
        /// Second keyword describing the query.
        /// </summary>
        public static string LabelRight => "after";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"[ ] {LabelLeft} [ ] {LabelRight} [ ]";

        /// <summary>
        /// The target <see cref="IFormulaViewModel"/> instance.
        /// </summary>
        public IFormulaViewModel Target { get; set; }

        /// <summary>
        /// The <see cref="ProgramViewModel"/> instance.
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
        /// Initializes a new <see cref="ExistentialValueQueryViewModel"/> instance.
        /// </summary>
        public ExistentialValueQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new NotImplementedException());
        }

        /// <summary>
        /// Gets the underlying query model out of the view model.
        /// </summary>
        /// <returns><see cref="ExistentialValueQuery"/> model represented by given view model.</returns>
        public ExistentialValueQuery ToModel()
        {
            if (Target == null)
                throw new MemberNotDefinedException("Target in an existential value query is not defined");
            if (Program == null)
                throw new MemberNotDefinedException("Program in an existential value query is not defined");

            return new ExistentialValueQuery(Target.ToModel(), Program.ToModel());
        }
    }
}