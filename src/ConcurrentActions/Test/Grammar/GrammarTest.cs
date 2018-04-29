using DynamicSystem.Grammar;
using NUnit.Framework;

namespace Test.Grammar
{
    [TestFixture()]
    public class GrammarTest
    {
        [Test]
        public void TestGrammar()
        {
            // T - truth
            // F - falsity
            // | - alternatywa
            // & - koniunkcja
            // -> - implikacja
            // <-> - rownowaznosc
            // ~ - negacja
            var actionDomain = DynamicSystemParserUtils.ParseActionDomain(@"
                initially ~brushA
                initially ~brushB
                TakeA causes brushA & ~brushB
                TakeB causes brushB & ~brushA
                PaintA causes ~brushA if brushA
                PaintB causes ~brushB if brushB
                always ~brushA | ~brushB
            ");

            actionDomain.ToString();
        }

//
        
    }
}