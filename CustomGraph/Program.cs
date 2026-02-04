namespace CustomGraph
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WeightedGraph<string> graph = new WeightedGraph<string>();
            graph.AddVertex("S");
            graph.AddVertex("A");
            graph.AddVertex("B");
            graph.AddVertex("E");

            graph.AddEdge("S", "A", 6, directed: true);
            graph.AddEdge("S", "B", 2, directed: true);
            graph.AddEdge("B", "A", 3, directed: true);
            graph.AddEdge("A", "E", 1, directed: true);
            graph.AddEdge("B", "E", 5, directed: true);

            Console.WriteLine(graph); // graph.ToString()
            Console.WriteLine(new string('-', 50));

            Console.WriteLine(graph.BFS("S", "E")); // bfs algorithm
            Console.WriteLine(new string('-', 50));

            var (cost, path) = graph.Dijkstra("S", "E"); // Dijkstra algorithm
            Console.WriteLine(cost);
            var s = string.Join("->", path);
            Console.WriteLine(s);
        }
    }
}
