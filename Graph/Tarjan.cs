using System;
using System.Collections.Generic;

namespace Graph
{
    public static class Tarjan
    {
        /// <see cref="SCC{TSource}(IEnumerable{TSource}, Func{TSource, IEnumerable{TSource}}, IEqualityComparer{TSource}?)"/>
        public static IEnumerable<IEnumerable<TSource>> SCC<TSource>(this IDictionary<TSource, IEnumerable<TSource>> G, IEqualityComparer<TSource>? comparer = null) where TSource : notnull
        {
            return SCC(G.Keys, key => new HashSet<TSource>(G[key]), comparer);
        }

        /// <summary>
        /// Runs Tarjan's strongly connected components algorithm on the given graph.
        /// See https://en.wikipedia.org/wiki/Tarjan%27s_strongly_connected_components_algorithm for details
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="V">List of nodes</param>
        /// <param name="getDependencies">Function returning dependencies of a given node</param>
        /// <param name="comparer">Comparer to test if nodes are equal</param>
        /// <returns>A list of strongly connected components</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IEnumerable<IEnumerable<TSource>> SCC<TSource>(this IEnumerable<TSource> V, Func<TSource, IEnumerable<TSource>> getDependencies, IEqualityComparer<TSource>? comparer = null) where TSource : notnull
        {
            var result = new List<IEnumerable<TSource>>();

            var index = 0;
            var S = new Stack<TSource>();

            var indexes = new Dictionary<TSource, int>();
            var lowlinks = new Dictionary<TSource, int>();
            var onStack = new Dictionary<TSource, bool>();

            foreach (var v in V)
            {
                if (!indexes.ContainsKey(v))
                {
                    StrongConnect(v);
                }
            }

            void StrongConnect(TSource v)
            {
                // Set the depth index for v to the smallest unused index
                indexes[v] = index;
                lowlinks[v] = index;
                index++;
                S.Push(v);
                onStack[v] = true;

                // Consider successors of v
                foreach (var w in getDependencies(v))
                {
                    if (!indexes.ContainsKey(w))
                    {
                        // Successor w has not yet been visited; recurse on it
                        StrongConnect(w);
                        lowlinks[v] = Math.Min(lowlinks[v], lowlinks[w]);
                    }
                    else if (onStack[w])
                    {
                        // Successor w is in stack S and hence in the current SCC
                        // If w is not on stack, then (v, w) is an edge pointing to an SCC already found and must be ignored
                        // Note: The next line may look odd - but is correct.
                        // It says w.index not w.lowlink; that is deliberate and from the original paper
                        lowlinks[v] = Math.Min(lowlinks[v], indexes[w]);
                    }
                }

                TSource u;
                // If v is a root node, pop the stack and generate a SCC
                if (lowlinks[v] == indexes[v])
                {
                    // start a new strongly connected component
                    var stronglyConnectedComponent = new List<TSource>();
                    do
                    {
                        u = S.Pop();
                        onStack[u] = false;
                        stronglyConnectedComponent.Add(u);
                    }
                    while (comparer != null ? !comparer.Equals(u, v) : !Equals(u, v));
                    result.Add(stronglyConnectedComponent);
                }
            }

            return result;
        }
    }
}
