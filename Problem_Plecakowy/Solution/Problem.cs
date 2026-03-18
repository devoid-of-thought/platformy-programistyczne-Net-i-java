public class Problem
{
    public int count;
    public int seed;
    public List<Item> ItemList = new List<Item> { };
    public Problem(int count, int seed)
    {
        this.count = count;
        this.seed = seed;
        Random rand = new Random(seed);


        if (count < 0)
        {
            throw new ArgumentException("Item count can't be negative!");
        }
        for (int i = 1; i <= count; i++)
        {
            Item item = new Item(i, rand.Next(1,10), rand.Next(1,10));
            ItemList.Add(item);
        }

        
    }
    public override string ToString()
    {
        return $"Problem: Rozmiar={count}, Ziarno={seed}, Lista przedmiotów={string.Join(", ", ItemList)}";
    }
    public Result Solve(int capacity)
    {
        List<Item> Sorted = ItemList.OrderBy(i => i.value / i.weight).Reverse().ToList();
        int ItemsInBack = 0;
        int inBack = 0;
        Result result = new Result();

        foreach (Item I in Sorted)
        {
            if (inBack+ I.weight >= capacity)
            {
                break;
            }
            result.Ids.Add(I.id);
            result.totalValue+=I.value;
            result.totalWeight += I.weight;
            inBack += I.weight;
            if ( Sorted.Count == ItemsInBack)
            {
                break;
            }
           
        }
         
         Console.WriteLine(result.ToString());
         return result;
    }
}