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
    public static class QueryResolver
    {
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