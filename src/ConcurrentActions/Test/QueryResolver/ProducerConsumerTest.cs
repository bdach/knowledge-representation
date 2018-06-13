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
    public class ProducerConsumerTest
    {
        private Signature signature;
        private ActionDomain actionDomain;
        private QuerySet querySet;

        [SetUp]
        public void SetUp()
        {
            var bufferEmptyF = new Fluent("bufferEmpty");
            var hasItemF = new Fluent("hasItem");

            var put = new Action("Put");
            var get = new Action("Get");
            var consume = new Action("Consume");

            signature = new Signature(
                new[] {bufferEmptyF, hasItemF},
                new[] {put, get, consume}
            );

            var bufferEmpty = new Literal(bufferEmptyF);
            var hasItem = new Literal(hasItemF);
            var notBufferEmpty = new Literal(bufferEmptyF, true);
            var notHasItem = new Negation(hasItem);

            actionDomain = new ActionDomain();

            // Put causes ~bufferEmpty if bufferEmpty
            actionDomain.EffectStatements.Add(
                new EffectStatement(put, bufferEmpty, notBufferEmpty)
            );
            // Get causes bufferEmpty & hasItem if ~bufferEmpty & ~hasItem
            actionDomain.EffectStatements.Add(
                new EffectStatement(get, new Conjunction(notBufferEmpty, notHasItem), new Conjunction(bufferEmpty, hasItem))
            );
            // Consume causes hasItem
            actionDomain.EffectStatements.Add(
                new EffectStatement(consume, notHasItem)
            );
            // impossible Consume if ~hasItem
            actionDomain.EffectStatements.Add(
                EffectStatement.Impossible(consume, notHasItem)
            );
            // initially bufferEmpty
            actionDomain.InitialValueStatements.Add(
                new InitialValueStatement(bufferEmpty)
            );

            var program = new Program(new[]
            {
                new CompoundAction(put),
                new CompoundAction(new[] {get, consume}), 
            });

            querySet = new QuerySet();

            // possibly ~bufferEmpty & ~hasItem after {Put}, {Get, Consume}
            querySet.ExistentialValueQueries.Add(
                new ExistentialValueQuery(new Conjunction(notBufferEmpty, notHasItem), program)
            );

            // necessary ~bufferEmpty & ~hasItem after {Put}, {Get, Consume}
            querySet.GeneralValueQueries.Add(
                new GeneralValueQuery(new Conjunction(notBufferEmpty, notHasItem), program)
            );

            // executable sometimes {Put}, {Get, Consume}
            querySet.ExistentialExecutabilityQueries.Add(
                new ExistentialExecutabilityQuery(program)
            );

            // executable always {Put}, {Get, Consume}
            querySet.GeneralExecutabilityQueries.Add(
                new GeneralExecutabilityQuery(program)
            );

            // accessible bufferEmpty | ~hasItem
            querySet.AccessibilityQueries.Add(
                new AccessibilityQuery(new Alternative(bufferEmpty, notHasItem))
            );
        }

        [Test]
        public void RunSystem()
        {
            // given all of the above
            // when
            var queryResolution = DynamicSystem.QueryResolver.ResolveQueries(signature, actionDomain, querySet);
            // then
            queryResolution.ExistentialValueQueryResults.First().Item2.Should().BeFalse();
            queryResolution.GeneralValueQueryResults.First().Item2.Should().BeFalse();
            queryResolution.ExistentialExecutabilityQueryResults.First().Item2.Should().BeFalse();
            queryResolution.GeneralExecutabilityQueryResults.First().Item2.Should().BeFalse();
            queryResolution.AccessibilityQueryResults.First().Item2.Should().BeTrue();
        }
    }
}