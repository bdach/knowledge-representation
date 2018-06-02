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
    public class NoninertialTest
    {
        private Signature signature;
        private ActionDomain actionDomain;
        private QuerySet querySet;

        [SetUp]
        public void SetUp()
        {
            var aF = new Fluent("a");
            var bF = new Fluent("b");
            var cF = new Fluent("c");

            var doA = new Action("doA");
            var doB = new Action("doB");

            signature = new Signature(
                new[] {aF, bF, cF},
                new[] {doA, doB}
            );

            var a = new Literal(aF);
            var notA = new Literal(aF, true);
            var b = new Literal(bF);

            actionDomain = new ActionDomain();

            // doA causes a if b
            actionDomain.EffectStatements.Add(
                new EffectStatement(doA, b, a)
            );

            // initially ~a
            actionDomain.InitialValueStatements.Add(
                new InitialValueStatement(notA)
            );

            // doB releases b
            actionDomain.FluentReleaseStatements.Add(
                new FluentReleaseStatement(doB, bF)
            );

            // noninertial c
            actionDomain.FluentSpecificationStatements.Add(
                new FluentSpecificationStatement(cF)
            );

            querySet = new QuerySet();

            // accessible a
            querySet.AccessibilityQueries.Add(
                new AccessibilityQuery(a)
            );
        }

        [Test]
        public void RunSystem()
        {
            // given all of the above
            // when
            var queryResolution = DynamicSystem.QueryResolver.ResolveQueries(signature, actionDomain, querySet);
            // then
            queryResolution.AccessibilityQueryResults.First().Item2.Should().BeTrue();
        }
    }
}