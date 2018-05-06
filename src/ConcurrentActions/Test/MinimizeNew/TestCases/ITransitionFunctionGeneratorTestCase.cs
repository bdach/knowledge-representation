using System.Collections.Generic;
using Model;

namespace Test.MinimizeNew.TestCases
{
    public interface ITransitionFunctionGeneratorTestCase
    {
        Dictionary<(CompoundAction, State), HashSet<State>> ResZero { get; }
        Dictionary<(CompoundAction, State, State), HashSet<Fluent>> NewSets { get; }
        TransitionFunction TransitionFunction { get; }
    }
}
