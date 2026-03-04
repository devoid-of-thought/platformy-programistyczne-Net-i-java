namespace solution
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter Number:");

            int number = Convert.ToInt32(Console.ReadLine());
            FizzBuzz fz = new FizzBuzz(number);

            fz.Run();
        }


    }
}




