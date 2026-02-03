namespace Dijkstra_s_Algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Dictionary<string, float>> graph = new Dictionary<string, Dictionary<string, float>>
            {
                ["Start"] = new Dictionary<string, float>
                {
                    ["A"] = 6,
                    ["B"] = 2
                },
                ["A"] = new Dictionary<string, float>
                {
                    ["FIN"] = 1
                },
                ["B"] = new Dictionary<string, float>
                {
                    ["A"] = 3,
                    ["FIN"] = 5
                },
                ["FIN"] = new Dictionary<string, float>()
            };

            string startNode = "Start";
            string targetNode = "FIN";

            var (costs, parents) = Dijkstra(graph, startNode);

            Console.WriteLine("Кратчайшие расстояния от начальной вершины:");
            Console.WriteLine("┌─────────────────┬──────────────┐");
            Console.WriteLine("│     Вершина     │  Расстояние  │");
            Console.WriteLine("├─────────────────┼──────────────┤");

            foreach (var node in costs)
            {
                string distanceStr = node.Value == float.PositiveInfinity
                    ? "∞"
                    : node.Value.ToString("F1");

                Console.WriteLine($"│ {node.Key,-15} │ {distanceStr,-12} │");
            }
            Console.WriteLine("└─────────────────┴──────────────┘\n");

            if (costs[targetNode] == float.PositiveInfinity)
            {
                Console.WriteLine($"Путь от '{startNode}' до '{targetNode}' не найден!");
                return;
            }

            var path = ReconstructPath(parents, startNode, targetNode);

            if (path.Count == 0)
            {
                Console.WriteLine($"Не удалось восстановить путь до '{targetNode}'");
                return;
            }

            Console.WriteLine("Найден кратчайший путь:");
            Console.WriteLine($"Из: {startNode}");
            Console.WriteLine($"В:  {targetNode}");
            Console.WriteLine($"Общий вес: {costs[targetNode]:F1}\n");

            Console.WriteLine("Детали пути:");
            Console.WriteLine("┌─────────────────────────────────────────┬────────────┐");
            Console.WriteLine("│                Переход                  │    Вес     │");
            Console.WriteLine("├─────────────────────────────────────────┼────────────┤");

            float totalWeight = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                string from = path[i];
                string to = path[i + 1];

                float edgeWeight = graph[from][to];
                totalWeight += edgeWeight;

                Console.WriteLine($"│ {from,-10} -> {to,-10}           │    {edgeWeight,-5:F1}   │");
            }

            Console.WriteLine("├─────────────────────────────────────────┼────────────┤");
            Console.WriteLine($"│ {"ИТОГО",-33} │    {totalWeight,-5:F1}   │");
            Console.WriteLine("└─────────────────────────────────────────┴────────────┘\n");

            Console.WriteLine("Графическое представление:");
            string pathStr = string.Join(" -> ", path);
            Console.WriteLine($"   {pathStr}");
            Console.Write("   ");
            for (int i = 0; i < pathStr.Length; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("->\n");

            Console.WriteLine("Сравнение с прямыми путями:");

            if (graph["Start"].ContainsKey("A") && graph["A"].ContainsKey("FIN"))
            {
                float directPathWeight = graph["Start"]["A"] + graph["A"]["FIN"];
                Console.WriteLine($"   Start -> A -> FIN:    {directPathWeight:F1} (не оптимально)");
            }

            Console.WriteLine($"   {string.Join(" -> ", path)}: {totalWeight:F1} YES ОПТИМАЛЬНЫЙ");

            if (graph["Start"].ContainsKey("B") && graph["B"].ContainsKey("FIN"))
            {
                float bPathWeight = graph["Start"]["B"] + graph["B"]["FIN"];
                Console.WriteLine($"   Start -> B -> FIN:    {bPathWeight:F1}");
            }
        }

        static (Dictionary<string, float> costs, Dictionary<string, string> parents)
            Dijkstra(Dictionary<string, Dictionary<string, float>> graph, string start)
        {
            Dictionary<string, float> costs = new Dictionary<string, float>();
            Dictionary<string, string> parents = new Dictionary<string, string>();
            HashSet<string> processed = new HashSet<string>();

            foreach (var node in graph.Keys)
            {
                if (node == start)
                {
                    costs[node] = 0;
                    parents[node] = null;
                }
                else
                {
                    costs[node] = float.PositiveInfinity;
                    parents[node] = null;
                }
            }

            string currentNode = FindLowestCostNode(costs, processed);

            while (currentNode != null)
            {
                float currentCost = costs[currentNode];

                if (graph.ContainsKey(currentNode))
                {
                    foreach (var neighbor in graph[currentNode])
                    {
                        string neighborName = neighbor.Key;
                        float edgeWeight = neighbor.Value;
                        float newCost = currentCost + edgeWeight;

                        if (newCost < costs[neighborName])
                        {
                            costs[neighborName] = newCost;
                            parents[neighborName] = currentNode;
                        }
                    }
                }

                processed.Add(currentNode);
                currentNode = FindLowestCostNode(costs, processed);
            }

            return (costs, parents);
        }

        static string FindLowestCostNode(Dictionary<string, float> costs, HashSet<string> processed)
        {
            float lowestCost = float.PositiveInfinity;
            string lowestCostNode = null;

            foreach (var node in costs)
            {
                if (node.Value < lowestCost && !processed.Contains(node.Key))
                {
                    lowestCost = node.Value;
                    lowestCostNode = node.Key;
                }
            }

            return lowestCostNode;
        }

        static List<string> ReconstructPath(Dictionary<string, string> parents,
                                           string start, string target)
        {
            List<string> path = new List<string>();
            string current = target;

            while (current != null)
            {
                path.Add(current);
                current = parents[current];
            }

            path.Reverse();

            if (path.Count > 0 && path[0] == start)
            {
                return path;
            }

            return new List<string>();
        }
    }
}
