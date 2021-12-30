using Graph;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GraphTests
{
    [TestFixture]
    public class JohnsonTests
    {
        static readonly object[] Cases =
        {
            new object[] { Graphs.graph1, new[] { new[] { "0", "2", "1" } } },
            new object[] { Graphs.graph2, Enumerable.Empty<IEnumerable<string>>() },
            new object[] { Graphs.graph3, new[] { new[] { "0", "1", "2" } } },
            new object[] { Graphs.graph4, new[] { new[] { "0", "3", "2" }, new[] { "0", "1", "2" }, new[] { "6", "4" }, new[] { "6", "4", "5" }, new[] { "9", "8" } } },
            new object[] { Graphs.graph5, new[] { new[] { "0", "1", "2", "3" }, new[] { "2", "4" } } },
            new object[] { Graphs.graph6, new[] { new[] { "3" }, new[] { "0", "2" }, new[] { "0", "1", "2" } } },
        };

        [TestCaseSource(nameof(Cases))]
        public void Cycles(IDictionary<string, IEnumerable<string>> graph, IEnumerable<IEnumerable<string>> expected)
        {
            var result = graph.Cycles().ToList();

            Assert.That(result, Is.EquivalentTo(expected));
        }
    }
}