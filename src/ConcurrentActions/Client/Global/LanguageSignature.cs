using Client.Abstract;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model;
using Model.Forms;
using ReactiveUI;

namespace Client.Global
{
    /// <summary>
    /// Singleton class used to store all the information about currently defined language.
    /// </summary>
    public class LanguageSignature : FodyReactiveObject
    {
        /// <summary>
        /// Collection of all <see cref="ActionViewModel"/>s for <see cref="Action"/>s currently defined in the language.
        /// </summary>
        public ReactiveList<ActionViewModel> ActionViewModels { get; protected set; }

        /// <summary>
        /// Collection of all <see cref="LiteralViewModel"/>s for <see cref="Literal"/>s currently defined in the language.
        /// </summary>
        public ReactiveList<LiteralViewModel> LiteralViewModels { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="LanguageSignature"/> instance.
        /// </summary>
        public LanguageSignature()
        {
            ActionViewModels = new ReactiveList<ActionViewModel>();
            LiteralViewModels = new ReactiveList<LiteralViewModel>();
        }

        /// <summary>
        /// Clears all collections that are a part of the language signature.
        /// </summary>
        public void Clear()
        {
            ActionViewModels.Clear();
            LiteralViewModels.Clear();
        }
    }
}