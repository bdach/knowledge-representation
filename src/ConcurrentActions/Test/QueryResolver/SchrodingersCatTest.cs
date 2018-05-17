using System.Linq;
using FluentAssertions;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using Model.QueryLanguage;
using NUnit.Framework;

namespace Test.QueryResolver
{
    [TestFixture]
    public class SchrodingersCatTest
    {
        private Signature signature;
        private ActionDomain actionDomain;
        private QuerySet querySet;

        [SetUp]
        public void SetUp()
        {
            var aliveF = new Fluent("alive");
            var purringF = new Fluent("purring");

            var pet = new Action("Pet");
            var peek = new Action("Peek");

            signature = new Signature(
                new[] {aliveF, purringF},
                new[] {pet, peek}
            );

            var alive = new Literal(aliveF);
            var purring = new Literal(purringF);
            var notAlive = new Literal(aliveF, true);
            var notPurring = new Literal(purringF, true);

            actionDomain = new ActionDomain();

            // initially alive
            actionDomain.InitialValueStatements.Add(
                new InitialValueStatement(alive)
            );

            // Peek releases alive if alive
            actionDomain.FluentReleaseStatements.Add(
                new FluentReleaseStatement(peek, aliveF, alive)
            );

            // Peek releases purring if alive
            actionDomain.FluentReleaseStatements.Add(
                new FluentReleaseStatement(peek, purringF, alive)
            );

            // Pet causes purring if alive
            actionDomain.EffectStatements.Add(
                new EffectStatement(pet, alive, purring)
            );

            // always ~alive -> ~purring
            actionDomain.ConstraintStatements.Add(
                new ConstraintStatement(new Implication(notAlive, notPurring))
            );

            var firstProgram = new Program(new[]
            {
                new CompoundAction(pet), new CompoundAction(peek), 
            });
            var secondProgram = new Program(new[]
            {
                new CompoundAction(new[] {pet, peek}), 
            });

            querySet = new QuerySet();

            // possibly ~alive after ({Pet}, {Peek})
            querySet.ExistentialValueQueries.Add(
                new ExistentialValueQuery(notAlive, firstProgram)
            );

            // necessary purring after ({Peek, Pet})
            querySet.GeneralValueQueries.Add(
                new GeneralValueQuery(purring, secondProgram)
            );

            // executable always ({Peek, Pet})
            querySet.GeneralExecutabilityQueries.Add(
                new GeneralExecutabilityQuery(secondProgram)
            );

            // accessible purring
            querySet.AccessibilityQueries.Add(
                new AccessibilityQuery(purring)
            );
        }

        [Test]
        public void RunSystem()
        {
            // given all of the above
            // when
            var queryResolution = DynamicSystem.QueryResolver.ResolveQueries(signature, actionDomain, querySet);
            // then
            queryResolution.ExistentialValueQueryResults.First().Item2.Should().BeTrue();
            queryResolution.GeneralValueQueryResults.First().Item2.Should().BeFalse();
            queryResolution.GeneralExecutabilityQueryResults.First().Item2.Should().BeTrue();
            queryResolution.AccessibilityQueryResults.First().Item2.Should().BeTrue();
        }
    }
}