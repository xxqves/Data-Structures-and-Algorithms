namespace CustomGraph
{
    internal readonly struct Edge<T> : IEquatable<Edge<T>>
    {
        public T From { get; }

        public T To { get; }

        public double Weight { get; }

        public Edge(T from, T to, double weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        public bool Equals(Edge<T> other)
        {
            return EqualityComparer<T>.Default.Equals(From, other.From) &&
               EqualityComparer<T>.Default.Equals(To, other.To) &&
               Weight.Equals(other.Weight);
        }
    }
}
