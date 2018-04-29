﻿using System.Reactive;
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
        /// Underlying constant instance.
        /// </summary>
        public Constant Constant { get; set; }

        /// <summary>
        /// Symbol used to represent the constant.
        /// </summary>
        public string Symbol => Constant.ToString();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

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
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotApplicableException("Constant does not support adding formulae"));
        }

        /// <summary>
        /// Gets the underlying constant model out of the view model.
        /// </summary>
        /// <returns><see cref="Model.Forms.Constant"/> model represented by given view model.</returns>
        public IFormula ToModel()
        {
            if (Constant != null)
                throw new MemberNotDefinedException("Constant does not have a value");

            return Constant;
        }
    }
}