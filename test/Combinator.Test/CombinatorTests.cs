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
            public TestNode(int value, int cost)
            {
                Value = value;
                Cost = cost;
            }

            public int Value { get; }
            public int Cost { get; }
        }

        [Test]
        public void Test_Combine1()
        {
            // Arrange
            var nodes = new List<TestNode>()
            {
                new TestNode(5, 1),
                new TestNode(7, 2),
                new TestNode(10, 3)
            };

            const int minValue = 100;
            const int maxValue = 105;
            const int maxCost = 200;

            static int GetValue(TestNode node) => node.Value;
            static int GetCosts(TestNode node) => node.Cost;

            // Act
            var results = Combinator
                .Combine(
                    nodes: nodes,
                    valueSelector: GetValue,
                    minValue: minValue,
                    maxValue: maxValue,
                    costSelector: GetCosts,
                    maxCost: maxCost
                )
                .ToList();

            // Assert
            Assert.That(results.Count > 0);

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
                new TestNode(7, 2),
                new TestNode(10, 3)
            };

            const int minValue = 100;
            const int maxValue = 105;

            static int GetValue(TestNode node) => node.Value;
            static int GetCosts(TestNode node) => node.Cost;

            // Act
            var results = Combinator
                .Combine(
                    nodes: nodes,
                    valueSelector: GetValue,
                    minValue: minValue,
                    maxValue: maxValue,
                    costSelector: GetCosts
                )
                .ToList();

            // Assert
            Assert.That(results.Count > 0);

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
                new TestNode(7, 2),
                new TestNode(10, 3)
            };

            const int maxCost = 200;

            static int GetValue(TestNode node) => node.Value;
            static int GetCosts(TestNode node) => node.Cost;

            // Act
            var results = Combinator
                .Combine(
                    nodes: nodes,
                    valueSelector: GetValue,
                    costSelector: GetCosts,
                    maxCost: maxCost
                )
                .ToList();

            // Assert
            Assert.That(results.Count > 0);

            Assert.That(
                results.All(s => s.Cost <= maxCost)
            );
        }
    }
}