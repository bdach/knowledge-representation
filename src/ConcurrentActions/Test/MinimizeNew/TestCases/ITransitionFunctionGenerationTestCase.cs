using System.Collections.Generic;
using Model;
using Model.Forms;

namespace Test.MinimizeNew.TestCases
{
    public interface ITransitionFunctionGenerationTestCase
    {
        TransitionFunction ResZero { get; }
        Dictionary<(CompoundAction, State, State), HashSet<Literal>> NewSets { get; }
        TransitionFunction TransitionFunction { get; }
    }
}
