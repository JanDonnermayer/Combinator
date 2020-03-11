using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Combinator.Tests
{
    [TestFixture]
    public class CombinatorTests
    {
        private struct TestNode
        {
            public TestNode(double value, double cost)
            {
                Value = value;
                Cost = cost;
            }

            public double Value { get; }
            public double Cost { get; }
        }

        [Test]
        public void Test_Combine1()
        {
            // Arrange
            var nodes = new List<TestNode>()
            {
                new TestNode(5, 1),
                new TestNode(10, 2),
                new TestNode(10, 3)
            };

            const double minValue = 20;
            const double maxValue = 25;
            const double maxCost = 10;

            static double GetValue(TestNode node) => node.Value;
            static double GetCosts(TestNode node) => node.Cost;

            // Act
            var results = CombinationProvider
                .Combine(
                    nodes: nodes,
                    valueSelector: GetValue,
                    minValue: minValue,
                    maxValue: maxValue,
                    costSelector: GetCosts,
                    maxCost: maxCost
                )
                .Take(10)
                .ToList();

            foreach (var res in results)
                TestContext.Progress.WriteLine(res);

            // Assert
            Assert.That(results.Count > 0);

            Assert.AreEqual(results.Distinct(), results);

            Assert.That(
                results.All(s =>
                    s.Value <= maxValue
                    && s.Value >= minValue
                    && s.Cost <= maxCost
                )
            );
        }

        [Test]
        public void Test_Combine2()
        {
            // Arrange
            var nodes = new List<TestNode>()
            {
                new TestNode(5, 1),
                new TestNode(10, 2),
                new TestNode(10, 3)
            };

            const double minValue = 100;
            const double maxValue = 105;

            static double GetValue(TestNode node) => node.Value;
            static double GetCosts(TestNode node) => node.Cost;

            // Act
            var results = CombinationProvider
                .Combine(
                    nodes: nodes,
                    valueSelector: GetValue,
                    minValue: minValue,
                    maxValue: maxValue,
                    costSelector: GetCosts
                )
                .Take(10)
                .ToList();

            // Assert
            Assert.That(results.Count > 0);

            Assert.AreEqual(results.Distinct(), results);

            Assert.That(
                results.All(s =>
                    s.Value <= maxValue
                    && s.Value >= minValue
                )
            );
        }

        [Test]
        public void Test_Combine3()
        {
            // Arrange
            var nodes = new List<TestNode>()
            {
                new TestNode(5, 1),
                new TestNode(10, 2),
                new TestNode(10, 3)
            };

            const double minCost = 100;
            const double maxCost = 105;

            static double GetValue(TestNode node) => node.Value;
            static double GetCosts(TestNode node) => node.Cost;

            // Act
            var results = CombinationProvider
                .Combine(
                    nodes: nodes,
                    valueSelector: GetValue,
                    costSelector: GetCosts,
                    minCost: minCost,
                    maxCost: maxCost
                )
                .Take(10)
                .ToList();

            // Assert
            Assert.That(results.Count > 0);

            Assert.AreEqual(results.Distinct(), results);

            Assert.That(
                results.All(s => s.Cost <= maxCost)
            );
        }
    }
}