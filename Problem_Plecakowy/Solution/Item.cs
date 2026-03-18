public class Item
{
    public int id;
    public int value{get; set;}
    public int weight{get; set;}
    public Item(int id, int value, int weight)
    {
        this.id = id;
        this.value = value;
        this.weight = weight;

        Console.WriteLine($"No: {this.id}, Val: { this.value}, Weight: { this.weight}");
    }
}