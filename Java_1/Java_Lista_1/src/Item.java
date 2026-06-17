public class Item {
public int id;
public int value;
public int weight;
public Item(int id, int value, int weight) {
    this.id = id;
    this.value = value;
    this.weight = weight;
    System.out.println("No:"+ this.id + ", Val: " + this.value + ", Weight:" + this.weight);
}

}

