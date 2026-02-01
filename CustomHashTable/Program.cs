namespace CustomHashTable
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var table = new HashTable<string, int>();
            table.Add("hello", 1);
            Console.WriteLine(table.TryGetValue("privet", out int _));

            table["privet"] = 2;

            Console.WriteLine(table.TryGetValue("privet", out int _)); 
        }
    }
}
