using System.Collections.Generic;

namespace GraphTests
{
    public static class Graphs
    {
        public static IDictionary<string, IEnumerable<string>> graph1 = new Dictionary<string, IEnumerable<string>>()
        {
            { "0", new HashSet<string> { "2", "3" } },
            { "1", new HashSet<string> { "0" } },
            { "2", new HashSet<string> { "1" } },
            { "3", new HashSet<string> { "4" } },
            { "4", new HashSet<string> {  } },
        };

        public static Dictionary<string, IEnumerable<string>> graph2 = new Dictionary<string, IEnumerable<string>>()
        {
            { "0", new HashSet<string> { "1" } },
            { "1", new HashSet<string> { "2" } },
            { "2", new HashSet<string> { "3" } },
            { "3", new HashSet<string> {  } },
        };

        public static Dictionary<string, IEnumerable<string>> graph3 = new Dictionary<string, IEnumerable<string>>()
        {
            { "0", new HashSet<string> { "1" } },
            { "1", new HashSet<string> { "2", "3", "4", "6" } },
            { "2", new HashSet<string> { "0" } },
            { "3", new HashSet<string> { "5" } },
            { "4", new HashSet<string> { "5" } },
            { "5", new HashSet<string> {  } },
            { "6", new HashSet<string> {  } },
        };

        public static Dictionary<string, IEnumerable<string>> graph4 = new Dictionary<string, IEnumerable<string>>()
        {
            { "0", new HashSet<string> { "1", "3" } },
            { "1", new HashSet<string> { "2", "4" } },
            { "2", new HashSet<string> { "0", "6" } },
            { "3", new HashSet<string> { "2" } },
            { "4", new HashSet<string> { "5", "6" } },
            { "5", new HashSet<string> { "6", "7", "8", "9" } },
            { "6", new HashSet<string> { "4" } },
            { "7", new HashSet<string> { "9" } },
            { "8", new HashSet<string> { "9" } },
            { "9", new HashSet<string> { "8" } },
            { "10", new HashSet<string> {  } },
        };

        public static Dictionary<string, IEnumerable<string>> graph5 = new Dictionary<string, IEnumerable<string>>()
        {
            { "0", new HashSet<string> { "1" } },
            { "1", new HashSet<string> { "2" } },
            { "2", new HashSet<string> { "3", "4" } },
            { "3", new HashSet<string> { "0" } },
            { "4", new HashSet<string> { "2" } },
        };

        public static Dictionary<string, IEnumerable<string>> graph6 = new Dictionary<string, IEnumerable<string>>()
        {
            { "0", new HashSet<string> { "1", "2" } },
            { "1", new HashSet<string> { "2" } },
            { "2", new HashSet<string> { "0", "3" } },
            { "3", new HashSet<string> { "3" } },
        };
    }
}