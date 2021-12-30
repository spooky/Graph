using Graph;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GraphTests
{
    [TestFixture]
    public class TarjanTests
    {
        static readonly object[] Cases =
        {
            new object[] { Graphs.graph1, new[] { new[] { "4" }, new[] { "3" }, new[] { "1", "2", "0" } } },
            new object[] { Graphs.graph2, new[] { new[] { "3" }, new[] { "2" }, new[] { "1" }, new[] { "0" } } },
            new object[] { Graphs.graph3, new[] { new[] { "5" }, new[] { "3" }, new[] { "4" }, new[] { "6" }, new[] { "2", "1", "0" } } },
            new object[] { Graphs.graph4, new[] { new[] { "8", "9" }, new[] { "7" }, new[] { "5", "4", "6" }, new[] { "3", "2", "1", "0" }, new[] { "10" } } },
            new object[] { Graphs.graph5, new[] { new[] { "4", "3", "2", "1", "0" } } },
            new object[] { Graphs.graph6, new[] { new[] { "2", "1", "0" }, new[] { "3" } } },
        };

        [TestCaseSource(nameof(Cases))]
        public void Scc(IDictionary<string, IEnumerable<string>> graph, IEnumerable<IEnumerable<string>> expected)
        {
            var result = graph.SCC().ToList();

            Assert.That(result, Is.EquivalentTo(expected));
        }
    }
}