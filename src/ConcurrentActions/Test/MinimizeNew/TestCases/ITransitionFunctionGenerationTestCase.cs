using DynamicSystem.NewGeneration;
using Model;

namespace Test.MinimizeNew.TestCases
{
    public interface ITransitionFunctionGenerationTestCase
    {
        TransitionFunction ResZero { get; }
        NewSetMapping NewSets { get; }
        TransitionFunction TransitionFunction { get; }
    }
}
