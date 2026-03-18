namespace solution
{
    class FizzBuzz
    {
        int number = 0;
        public FizzBuzz(int number)
        {
            this.number = number;
        }
        public void Run()
        {
            for (int i = 0; i <= number; i++)
                if (i % 5 == 0 && i % 3 == 0)
                {
                    Console.WriteLine("FizzBuzz");
                }
                else
                    if (i % 5 == 0)
                    {
                        Console.WriteLine("Buzz");

                    }
                    else
                        if (i % 3 == 0)
                        {
                            Console.WriteLine("Fizz");

                        }
                        else
                        {
                            Console.WriteLine(i);
                        }
    
        }

    }
}


