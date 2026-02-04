using System.Text;

namespace CustomGraph
{
    internal class WeightedGraph<T> where T: IEquatable<T>
    {
        private Dictionary<T, List<Edge<T>>> _adjacencyList;

        public int VertexCount { get => _adjacencyList.Count; }

        public int EdgeCount { get; private set; }

        public IEnumerable<T> Vertices { get => _adjacencyList.Keys; }

        public IEnumerable<Edge<T>> Edges
        {
            get
            {
                foreach (var vertex in _adjacencyList.Values)
                {
                    foreach (var edge in vertex)
                    {
                        yield return edge;
                    }
                }
            }
        }

        public WeightedGraph()
        {
            _adjacencyList = new Dictionary<T, List<Edge<T>>>();
        }

        public void AddVertex(T vertex)
        {
            if (vertex is null)
            {
                throw new ArgumentNullException(nameof(vertex));
            }

            if (_adjacencyList.ContainsKey(vertex))
            {
                throw new InvalidOperationException("Such a vertex has already been added");
            }

            _adjacencyList.Add(vertex, new List<Edge<T>>());
        }

        public bool RemoveVertex(T vertex)
        {
            if (!HasVertex(vertex))
            {
                return false;
            }

            int edgesRemoved = 0;
            foreach (var v in _adjacencyList.Keys.ToList())
            {
                if (v.Equals(vertex)) continue;

                int removed = _adjacencyList[v].RemoveAll(edge => edge.To.Equals(vertex));
                edgesRemoved += removed;
                EdgeCount -= removed;
            }

            edgesRemoved += _adjacencyList[vertex].Count;
            EdgeCount -= _adjacencyList[vertex].Count;

            return _adjacencyList.Remove(vertex);
        }

        public void AddEdge(T from, T to, double weight, bool directed = false)
        {
            if (from is null) throw new ArgumentNullException(nameof(from));
            if (to is null) throw new ArgumentNullException(nameof(to));
            if (weight < 0) throw new ArgumentException("Weight cannot be negative", nameof(weight));

            if (!HasVertex(from)) AddVertex(from);
            if (!HasVertex(to)) AddVertex(to);

            if (HasEdge(from, to))
            {
                throw new InvalidOperationException($"Edge from {from} to {to} already exists");
            }

            _adjacencyList[from].Add(new Edge<T>(from, to, weight));
            EdgeCount++;

            if (!directed)
            {
                _adjacencyList[to].Add(new Edge<T>(to, from, weight));
                EdgeCount++;
            }
        }

        public bool RemoveEdge(T from, T to, bool directed = false)
        {
            if (!HasEdge(from, to))
            {
                return false;
            }

            int removed = _adjacencyList[from].RemoveAll(edge => edge.To.Equals(to));

            if (removed > 0)
            {
                EdgeCount -= removed;

                if (!directed && HasEdge(from, to))
                {
                    int reverseRemoved = _adjacencyList[to].RemoveAll(edge => edge.To.Equals(from));
                }
                return true;
            }

            return false;
        }

        public void Clear()
        {
            _adjacencyList.Clear();
            EdgeCount = 0;
        }

        public bool HasVertex(T vertex)
        {
            return _adjacencyList.ContainsKey(vertex);
        }

        public bool HasEdge(T from, T to)
        {
            if (!HasVertex(from) || !HasVertex(to))
            {
                return false;
            }

            return _adjacencyList[from].Any(edge => edge.To.Equals(to));
        }

        public IReadOnlyList<Edge<T>> GetNeighbors(T vertex)
        {
            if (!HasVertex(vertex))
            {
                throw new KeyNotFoundException($"Vertex {vertex} not found");
            }

            return _adjacencyList[vertex].AsReadOnly();
        }

        public double? GetEdgeWeight(T from, T to)
        {
            if (!HasVertex(from) || !HasVertex(to))
            {
                return null;
            }

            var edge = _adjacencyList[from].FirstOrDefault(edge => edge.To.Equals(to));
            return edge.Weight;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Graph with {VertexCount} vertices and {EdgeCount} edges:");

            foreach (var vertex in _adjacencyList)
            {
                sb.Append($"{vertex.Key}: ");

                if (vertex.Value.Count == 0)
                {
                    sb.AppendLine("no neighbors");
                }
                else
                {
                    var neighbors = vertex.Value.Select(e => $"{e.To}({e.Weight})");
                    sb.AppendLine(string.Join(", ", neighbors));
                }
            }

            return sb.ToString();
        }
    }
}