﻿using System;
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
    /// View model for <see cref="GeneralValueQueryView"/> which represents a general value query in the scenario.
    /// </summary>
    public class GeneralValueQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<GeneralValueQuery>, IGalleryItem
    {
        /// <summary>
        /// First keyword describing the query.
        /// </summary>
        public string LabelLeft => "necessary";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "ValueQuery";

        /// <summary>
        /// Second keyword describing the query.
        /// </summary>
        public string LabelRight => "after";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"[ ] {LabelLeft} [ ] {LabelRight}";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a target formula.
        /// </summary>
        public IViewModelFor<IFormula> Target { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The <see cref="ProgramViewModel"/> instance.
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
        /// Initializes a new <see cref="GeneralValueQueryViewModel"/> instance.
        /// </summary>
        public GeneralValueQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IViewModelFor<IFormula>>(formulaViewModel =>
                    throw new NotImplementedException());

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new NotImplementedException());
        }

        /// <summary>
        /// Gets the underlying query model out of the view model.
        /// </summary>
        /// <returns><see cref="GeneralValueQuery"/> model represented by given view model.</returns>
        public GeneralValueQuery ToModel()
        {
            var target = Target?.ToModel();
            var program = Program?.ToModel();

            if (target == null)
                throw new MemberNotDefinedException("Target in a general value query is not defined");
            if (program == null)
                throw new MemberNotDefinedException("Program in a general value query is not defined");

            return new GeneralValueQuery(target, program);
        }
    }
}