import java.util.Random;
import java.util.Scanner;

public class Main {

    public static void main(String[] args) {
        Scanner sc = new Scanner(System.in);
        System.out.println(" Enter the Number of items :");
        int num = sc.nextInt();

        System.out.println(" Enter the seed :");
        int seed = sc.nextInt();
        Random random = new Random(seed);
        System.out.println(random.nextInt(9)+1);
        System.out.println(" Enter the Capacity :");
        int capacity = sc.nextInt();
        Problem p = new Problem(num, seed,capacity,1,10);

        p.solve();
        sc.close();

    }

}
