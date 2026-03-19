using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Problem_Plecakowy;

namespace TestProject;

[TestClass]
public class UnitTest
{
    [TestMethod]
    public void TestMethodCountElements()
    {
        List<int> sizes = new List<int> { 10, 20, 30, 40, 50 };

        foreach (int n in sizes)
        {
            Problem problem = new Problem(n, 1);

            Assert.AreEqual(n, problem.count, $"Failed for size {n}");
        }
    }
    [TestMethod]
    public void TestMethodAtLeastOne()
    {
        Problem problem = new Problem(3, 1);
        int capacity = 10;
        bool doesOneFits = false;
        Result result = problem.Solve(capacity);

        for (int i = 0; i <problem.count; i++)
        {
            if (problem.ItemList[i].weight <= capacity)
            {
                doesOneFits = true;
            }
        }
        if (doesOneFits)
        {
            Assert.IsTrue(result.Ids.Count >=1);
        }

    }
    [TestMethod]
    public void TestMethodNone()
    {
        Problem problem = new Problem(0, 1);
        int capacity = 10;
        Result result = problem.Solve(capacity);

        Assert.IsTrue(result.Ids.Count == 0);
        

    }
    [TestMethod]
    public void TestMethodIsResultCorrect()
    {
        Problem problem = new Problem(5, 1);
        int capacity = 5;
        Result result = problem.Solve(capacity);

        List<int> correctIds = [1,3];
        Console.WriteLine($"Lista przedmiotów={string.Join(", ", result.Ids)} , Lista Przedmiotów Poprawna = 1 , 3 , 5");
        Assert.IsTrue(result.Ids.SequenceEqual(correctIds));
    }

    [TestMethod]
    public void TestMethodTooBig()
    {
        Problem problem = new Problem(1, 2);
        int capacity = 1;
        Result result = problem.Solve(capacity);

        Assert.IsTrue(result.Ids.Count == 0);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException),
    "Item count can't be negative!")]
    public void TestMethodNegativeCapacity()
    {
        Problem problem = new Problem(-5, 1);
        int capacity = 1;
        Result result = problem.Solve(capacity);
    }


}