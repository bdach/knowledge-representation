using System.Collections.Generic;
using NUnit.Framework;
using Model;
using System.Linq;
using DynamicSystem.Decomposition;
using FluentAssertions;

namespace Test.Decomposition
{
    [TestFixture]
    public class LinqExtensionsTests
    {
        [Test]
        public void CartesianProductTest()
        {
            // given
            var listA = new List<Action> {new Action("a"), new Action("b"), new Action("c")};
            var listB = new List<Action> {new Action("1"), new Action("2")};
            //when
            var cartesianProduct = listA.CartesianProduct(listB).ToList();
            //then
            cartesianProduct.Count.Should().Be(6);
            cartesianProduct.Should().Contain((new Action("a"), new Action("1")));
            cartesianProduct.Should().Contain((new Action("a"), new Action("2")));
            cartesianProduct.Should().Contain((new Action("b"), new Action("1")));
            cartesianProduct.Should().Contain((new Action("b"), new Action("2")));
            cartesianProduct.Should().Contain((new Action("c"), new Action("1")));
            cartesianProduct.Should().Contain((new Action("c"), new Action("2")));
        }

        [Test]
        public void AllPairsTest()
        {
            // given
            var listA = new List<Action> { new Action("a"), new Action("b"), new Action("c") };
            //when
            var allPairs = listA.AllPairs().ToList();
            //then
            allPairs.Count.Should().Be(3);
            allPairs.Should().Contain((new Action("a"), new Action("b")));
            allPairs.Should().Contain((new Action("b"), new Action("c")));
            allPairs.Should().Contain((new Action("a"), new Action("c")));
        }
    }
}
