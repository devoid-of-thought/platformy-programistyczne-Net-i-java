import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

public class ProblemTest {

    @Test
    public void IfEligibleReturnsTest(){
        Problem prl = new Problem(1,1,10,1,10);
        assertFalse(prl.solve().Ids.isEmpty());
    }
    @Test
    public void IfNotEligibleReturnsTest(){
        Problem prl = new Problem(1,1,8,10,20);
        assertTrue(prl.solve().Ids.isEmpty());
    }
    @Test
    public void CheckAllItemsFitInBoundary(){
        Problem prl = new Problem(5,1,10,1,10);
        boolean res = true;
        for (Item I : prl.ItemList){
            if (!(I.weight >=1 && I.weight <= 10 && I.value >= 1 && I.value <= 10)){
                res = false;
            }
        }
        assertTrue(res);
    }
    @Test
    public void CheckIfResultIsCorrect(){
        Problem prl = new Problem(10,1,11,1,10);
        Result res = prl.solve();
        int[] correct;
        correct = new int[]{1, 1, 1, 1, 1, 7};
        boolean result = true;
        for (int i = 0; i < 6; i++){
            if (res.Ids.get(i) != correct[i]){
                result = false;
            }
        }
        assertTrue(result);

    }
}
