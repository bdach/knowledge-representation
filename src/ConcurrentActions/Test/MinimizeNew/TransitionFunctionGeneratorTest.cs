using DynamicSystem.MinimizeNew;
using NUnit.Framework;
using Test.MinimizeNew.TestCases;
using static NUnit.Framework.CollectionAssert;

namespace Test.MinimizeNew
{
    [TestFixture]
    public class TransitionFunctionGeneratorTest
    {
        private static void Test(ITransitionFunctionGenerationTestCase testCase)
        {
            var expected = testCase.TransitionFunction;
            var actual = TransitionFunctionGenerator.GenerateTransitionFunction(testCase.ResZero, testCase.NewSets);

            AreEquivalent(expected.CompoundActions, actual.CompoundActions);
            AreEquivalent(expected.States, actual.States);

            foreach (var compoundAction in expected.CompoundActions)
                foreach (var state in expected.States)
                    AreEquivalent(expected[compoundAction, state], actual[compoundAction, state]);
        }

        [Test]
        public void TestTransitionFunctionGeneration_ProducerAndConsumer()
        {
            Test(new ProducerAndConsumerTransitionFunctionGenerationTestCase());
        }

        [Test]
        public void TestTransitionFunctionGeneration_SchroedingerCat()
        {
            Test(new SchoedingerCatTransitionFunctionGenerationTestCase());
        }

        [Test]
        public void TestTransitionFunctionGeneration_TwoPainters()
        {
            Test(new TwoPaintersTransitionFunctionGenerationTestCase());
        }
    }
}
