public class Result
{
    public List<int> Ids = new List<int>{};
    public int totalWeight= 0;
    public int totalValue= 0 ;
    public override string ToString()
    {
        
        return $"Lista przedmiotów={string.Join(", ", Ids)} Total value: {this.totalValue}, Total Weight={this.totalWeight}";
    }
}