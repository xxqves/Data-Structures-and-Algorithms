namespace BFS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();
            graph["Me"] = new List<string> { "Alice", "Bob", "Clar" };
            graph["Bob"] = new List<string> { "Anuj", "Peggi" };
            graph["Alice"] = new List<string> { "Peggi" };
            graph["Clar"] = new List<string> { "Tom", "Jhonny" };
            graph["Anuj"] = new List<string>();
            graph["Peggi"] = new List<string>();
            graph["Tom"] = new List<string>();
            graph["Jhonny"] = new List<string>();

            foreach (var node in graph)
            {
                Console.WriteLine($"{node.Key}: {string.Join(", ", node.Value)}");
            }

            Queue<string> searchQueue = new Queue<string>(graph["Me"]);

            List<string> searched = new List<string>();

            while (searchQueue.Count > 0)
            {
                string person = searchQueue.Dequeue();

                if (!searched.Contains(person))
                {
                    Console.WriteLine($"Проверяем {person}...");

                    if (PersonIsSeller(person))
                    {
                        Console.WriteLine($"Найден продавец манго: {person}");
                        return;
                    }
                    else
                    {
                        foreach (var friend in graph[person])
                        {
                            searchQueue.Enqueue(friend);
                        }

                        searched.Add(person);
                    }
                }
            }

            Console.WriteLine("Продавец манго не найден!");
        }

        static bool PersonIsSeller(string name)
        {
            return name.EndsWith("m", StringComparison.OrdinalIgnoreCase);
        }
    }
}
