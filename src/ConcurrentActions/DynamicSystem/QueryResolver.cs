using System.Collections.Generic;
using System.Linq;
using DynamicSystem.MinimizeNew;
using DynamicSystem.NewGeneration;
using DynamicSystem.QueriesEvaluation;
using DynamicSystem.ResZero;
using DynamicSystem.SetGeneration;
using DynamicSystem.Verification;
using Model;
using Model.ActionLanguage;
using Model.QueryLanguage;

namespace DynamicSystem
{
    /// <summary>
    /// Main class of the dynamic system.
    /// </summary>
    public static class QueryResolver
    {
        /// <summary>
        /// Resolves the queries supplied, using the language signature and action domain.
        /// </summary>
        /// <param name="signature">Instance of <see cref="Signature"/>, containing the fluents and atomic actions in the scenario.</param>
        /// <param name="actionDomain">Instance of <see cref="ActionDomain"/>, containing the action statements in the scenario.</param>
        /// <param name="querySet">Instance of <see cref="QuerySet"/>, containing the queries concerning the current scenario.</param>
        /// <returns>An instance of <see cref="QueryResolution"/>, containing the results of supplied queries.</returns>
        public static QueryResolution ResolveQueries(Signature signature, ActionDomain actionDomain, QuerySet querySet)
        {
            var admissibleStates = new HashSet<State>(SetGenerator.GetAdmissibleStates(signature.Fluents, actionDomain));
            var initialStates = new HashSet<State>(SetGenerator.GetInitialStates(signature.Fluents, actionDomain));
            var compoundActions = new HashSet<CompoundAction>(SetGenerator.GetCompoundActions(signature.Actions));

            var resZero = ResZeroGenerator.GenerateResZero(actionDomain, compoundActions, admissibleStates);
            var newSets = NewSetGenerator.GetNewSets(actionDomain, signature, resZero);
            var res = TransitionFunctionGenerator.GenerateTransitionFunction(resZero, newSets);
            var structures = initialStates.Select(state => new Structure(admissibleStates, state, res)).ToList();

            var models = structures.Where(structure => StructureVerification.CheckStatements(actionDomain, structure)).ToList();
            var resolution = QueriesEvaluator.EvaluateQueries(models, querySet);
            return resolution;
        }
    }
}