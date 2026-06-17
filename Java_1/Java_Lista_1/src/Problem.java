import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class Problem {
    public int count;
    public int seed;
    public int lowerBound;
    public int upperBound;
    public int capacity;
    public List<Item> ItemList = new ArrayList<>();

    public Problem(int count, int seed,int capacity, int lowerBound, int upperBound){
        this.count = count;
        this.seed = seed;
        this.lowerBound = lowerBound;
        this.capacity = capacity;
        this.upperBound = upperBound;
        Random rand = new Random(seed);

        if (count < 0){
            throw new IllegalArgumentException("Item count can't be negative");

        }
        for (int i = 1; i <= count; i++){
            Item item = new Item(i,rand.nextInt(this.lowerBound,this.upperBound),rand.nextInt(this.lowerBound,this.upperBound));
            ItemList.add(item);
        }
    }
    @Override
    public String toString(){
        return "Problem: Rozmiar=" + count + ", Ziarno=" + seed + ", Lista przedmiotów=" + ItemList;
    }
    public Result solve(){

        List<Item> sorted = new ArrayList<>(ItemList);
        sorted.sort((a, b) -> Double.compare((double) b.value / b.weight, (double) a.value / a.weight));        int ItemsInBack = 0;
        Result result = new Result();
        for (Item I : sorted){
            if (result.totalWeight+ I.weight > capacity){
                continue;
            }
            while(result.totalWeight + I.weight <= capacity ) {
                result.Ids.add(I.id);
                result.totalValue += I.value;
                result.totalWeight += I.weight;
            }
        }
        System.out.println(result.toString());
        return result;
    }
}