namespace Problem_Plecakowy;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(" Enter the Number of items :");
        int num = int.Parse(Console.ReadLine());
        Console.WriteLine(" Enter the seed :");
        int seed = int.Parse(Console.ReadLine());
        Random random = new Random(seed);
        Console.WriteLine(random.Next(1,10));

        Problem p = new Problem(num, seed);
        Console.WriteLine(" Enter the Capacity :");
        int capacity = int.Parse(Console.ReadLine());
        p.Solve(capacity);
    }
}
