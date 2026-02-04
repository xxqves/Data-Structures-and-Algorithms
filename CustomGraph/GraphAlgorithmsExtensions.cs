namespace CustomGraph
{
    internal static class GraphAlgorithmsExtensions
    {
        public static int BFS<T>(this WeightedGraph<T> graph, T start, T target) where T: IEquatable<T>
        {
            if (!graph.HasVertex(start) || !graph.HasVertex(target))
            {
                throw new ArgumentException($"Start or target vertex not found in graph");
            }

            HashSet<T> visited = new HashSet<T>();
            Queue<T> queue = new Queue<T>();

            queue.Enqueue(start);
            visited.Add(start);

            int distance = 0;

            while (queue.Count > 0)
            {
                int level = queue.Count;

                for (int i = 0; i < level; i++)
                {
                    T current = queue.Dequeue();

                    Console.WriteLine($"Processing {current}, distance: {distance}...");
                    if (EqualityComparer<T>.Default.Equals(current, target))
                    {
                        return distance;
                    }

                    foreach (var edge in graph.GetNeighbors(current))
                    {
                        if (!visited.Contains(edge.To))
                        {
                            queue.Enqueue(edge.To);
                            visited.Add(edge.To);
                        }
                    }
                }

                distance++;
            }

            return -1;
        }

        public static (double cost, List<T> path) Dijkstra<T>(this WeightedGraph<T> graph, T start, T target) where T : IEquatable<T>
        {
            if (!graph.HasVertex(start) || !graph.HasVertex(target))
            {
                throw new ArgumentException($"Start or target vertex not found in graph");
            }

            var costs = new Dictionary<T, double>();
            var parents = new Dictionary<T, T>();
            var processed = new HashSet<T>();

            foreach (var vertex in graph.Vertices)
            {
                costs[vertex] = double.PositiveInfinity;
            }

            costs[start] = 0;

            var priorityQueue = new PriorityQueue<T, double>();
            priorityQueue.Enqueue(start, 0);

            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Dequeue();

                if (processed.Contains(current))
                {
                    continue;
                }

                processed.Add(current);

                if (EqualityComparer<T>.Default.Equals(current, target))
                {
                    return (costs[current], ReconstructPath(parents, start, target));
                }

                foreach (var edge in graph.GetNeighbors(current))
                {
                    if (processed.Contains(edge.To))
                    {
                        continue;
                    }

                    double newCost = costs[current] + edge.Weight;

                    if (newCost < costs[edge.To])
                    {
                        costs[edge.To] = newCost;
                        parents[edge.To] = current;
                        priorityQueue.Enqueue(edge.To, newCost);
                    }
                }
            }

            throw new InvalidOperationException($"No path from {start} to {target}");
        }

        private static List<T> ReconstructPath<T>(Dictionary<T, T> parents, T start, T target) where T : IEquatable<T>
        {
            var path = new List<T>();
            var current = target;

            while (!EqualityComparer<T>.Default.Equals(current, default) &&
                   !EqualityComparer<T>.Default.Equals(current, start))
            {
                path.Add(current!);
                parents.TryGetValue(current!, out current);
            }

            if (EqualityComparer<T>.Default.Equals(current, start))
            {
                path.Add(start);
                path.Reverse();
                return path;
            }

            return new List<T>();
        }
    }
}
