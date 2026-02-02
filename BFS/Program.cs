namespace BFS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>
            {
                ["You"] = new List<string> { "Alice", "Bob", "Claire" },
                ["Bob"] = new List<string> { "Anuj", "Peggy" },
                ["Alice"] = new List<string> { "Peggy" },
                ["Claire"] = new List<string> { "Tom", "Jhonny" },
                ["Anuj"] = new List<string>(),
                ["Peggy"] = new List<string>(),
                ["Tom"] = new List<string>(),
                ["Jhonny"] = new List<string>()
            };

            Console.WriteLine(HasConnection(graph, "You", "Peggy"));
            Console.WriteLine(FindConnectionLength(graph, "You", "Peggy"));
        }

        static bool HasConnection(Dictionary<string, List<string>> graph, string startPerson, string targetPerson)
        {
            HashSet<string> visited = new();

            Queue<string> queue = new Queue<string>(graph[startPerson]);

            while (queue.Count > 0)
            {
                string person = queue.Dequeue();
                if (!visited.Contains(person))
                {
                    Console.WriteLine($"Проверяем {person}");
                    if (person == targetPerson)
                    {
                        return true;
                    }
                    else
                    {
                        foreach (var friend in graph[person])
                        {
                            queue.Enqueue(friend);
                        }

                        visited.Add(person);
                    }
                }
            }

            return false;
        }

        static int FindConnectionLength(Dictionary<string, List<string>> graph, string startPerson, string endPerson)
        {
            Queue<(string person, int distance)> queue = new Queue<(string, int)>();
            HashSet<string> visited = new HashSet<string>();

            queue.Enqueue((startPerson, 0));
            visited.Add(startPerson);
            
            while (queue.Count> 0)
            {
                var (currentPerson, currentDistance) = queue.Dequeue();

                Console.WriteLine($"Проверяем {currentPerson} расстояние: {currentDistance}...");
                if (currentPerson == endPerson)
                {
                    return currentDistance;
                }
                else
                {
                    foreach (var friend in graph[currentPerson])
                    {
                        if (!visited.Contains(friend))
                        {
                            queue.Enqueue((friend, currentDistance + 1));
                            visited.Add(friend);
                        }                        
                    }                    
                }
            }

            return -1;
        }
    }
}