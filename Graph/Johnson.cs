using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public static class Johnson
    {
        /// <see cref="Cycles{TSource}(IEnumerable{TSource}, Func{TSource, IEnumerable{TSource}}, IEqualityComparer{TSource}?)"/>
        public static IEnumerable<IEnumerable<TSource>> Cycles<TSource>(this IDictionary<TSource, IEnumerable<TSource>> G, IEqualityComparer<TSource>? comparer = null) where TSource : notnull
        {
            return Cycles(G.Keys, key => G[key], comparer);
        }

        // Port of python networkx simple_cycles implementation

        /// <summary>
        /// Finds all cycles in a given graph using Johnson's algorithm
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="G">List of nodes</param>
        /// <param name="getDependencies">Function returning dependencies of a given node</param>
        /// <param name="comparer">Comparer to test if nodes are equal</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<TSource>> Cycles<TSource>(this IEnumerable<TSource> G, Func<TSource, IEnumerable<TSource>> getDependencies, IEqualityComparer<TSource>? comparer = null) where TSource : notnull
        {
            static void _unblock(TSource thisnode, HashSet<TSource> blocked, Dictionary<TSource, HashSet<TSource>> B)
            {
                var stack = new Stack<TSource>();
                stack.Push(thisnode);
                while (stack.Count > 0)
                {
                    var node = stack.Pop();
                    if (blocked.Contains(node))
                    {
                        blocked.Remove(node);

                        // init if B[nbr] does not exist
                        if (!B.ContainsKey(node))
                        {
                            B[node] = new HashSet<TSource>();
                        }

                        foreach (var item in B[node])
                        {
                            stack.Push(item);
                        }
                        B[node].Clear();
                    }
                }
            }

            // Johnson's algorithm requires some ordering of the nodes.
            // We assign the arbitrary ordering given by the strongly connected comps
            // There is no need to track the ordering as each node removed as processed.
            // Also we save the actual graph so we can mutate it. We only take the
            // edges because we do not want to copy edge and node attributes here.
            var subG = G.ToDictionary(x => x, x => new HashSet<TSource>(getDependencies(x)));
            var sccs = new Stack<Stack<TSource>>(subG.Keys.SCC(x => subG[x]).Where(x => x.Count() > 1).Select(x => new Stack<TSource>(x)));

            // Johnson's algorithm exclude self cycle edges like (v, v)
            // We record those cycles in advance and then remove from subG
            foreach (var c in subG)
            {
                if (c.Value.Contains(c.Key))
                {
                    yield return new[] { c.Key };
                    c.Value.Remove(c.Key);
                }
            }

            while (sccs.Count > 0)
            {
                var scc = sccs.Pop();
                var sccG = subG.Where(x => scc.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value.Intersect(scc).ToList());
                // order of scc determines ordering of nodes
                var startnode = scc.Pop();
                // Processing node runs "circuit" routine from recursive version
                var path = new Stack<TSource>();
                path.Push(startnode);
                var blocked = new HashSet<TSource> { startnode }; // vertex: blocked from search?
                var closed = new HashSet<TSource>(); // nodes involved in a cycle
                var B = new Dictionary<TSource, HashSet<TSource>>(); //  graph portions that yield no elementary circuit
                var stack = new Stack<(TSource, Stack<TSource>)>(); //  sccG gives comp nbrs
                stack.Push((startnode, new Stack<TSource>(sccG[startnode])));

                while (stack.Count > 0)
                {
                    var (thisnode, nbrs) = stack.First();
                    if (nbrs.Count > 0)
                    {
                        var nextnode = nbrs.Pop();
                        if (comparer != null ? comparer.Equals(nextnode, startnode) : Equals(nextnode, startnode))
                        {
                            yield return new List<TSource>(path.Reverse());

                            foreach (var item in path)
                            {
                                closed.Add(item);
                            }
                        }
                        else if (!blocked.Contains(nextnode))
                        {
                            path.Push(nextnode);
                            stack.Push((nextnode, new Stack<TSource>(sccG[nextnode])));
                            closed.Remove(nextnode);
                            blocked.Add(nextnode);
                            continue;
                        }
                    }

                    //  done with nextnode... look for more neighbors
                    if (nbrs.Count == 0) // no more nbrs
                    {
                        if (closed.Contains(thisnode))
                        {
                            _unblock(thisnode, blocked, B);
                        }
                        else
                        {
                            foreach (var nbr in sccG[thisnode])
                            {
                                // init if B[nbr] does not exist
                                if (!B.ContainsKey(nbr))
                                {
                                    B[nbr] = new HashSet<TSource>();
                                }

                                if (!B[nbr].Contains(thisnode))
                                {
                                    B[nbr].Add(thisnode);
                                }
                            }
                        }

                        stack.Pop();
                        path.Pop();

                        var H = subG.Where(x => scc.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value.Intersect(scc).ToList());
                        foreach (var x in H.Keys.SCC(x => H[x]).Where(x => x.Count() > 1))
                        {
                            if (!sccs.Any(y => x.Except(y).Count() == 0))
                            {
                                sccs.Push(new Stack<TSource>(x));
                            }
                        }
                    }
                }
            }
        }
    }
}
