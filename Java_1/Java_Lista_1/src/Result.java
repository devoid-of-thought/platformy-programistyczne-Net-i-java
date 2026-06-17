import java.util.ArrayList;
import java.util.List;

public class Result {
    public List<Integer> Ids = new ArrayList<>();
    public int totalWeight = 0;
    public int totalValue = 0;

    @Override
    public String toString() {
        return "Result: Ids=" + Ids + ", Total Weight=" + totalWeight + ", Total Value=" + totalValue;
    }

}
