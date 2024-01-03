namespace targil0
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            //Console.WriteLine("Hello123456789, World11!");
            int numVal = Int32.Parse("-105");
            Console.WriteLine(numVal)

            Welcome3572();
            Welcome2499();
            Console.ReadKey();
        }
        static partial void Welcome2499();

        private static void Welcome3572()
        {
            Console.Write("Enter your name: ");
            string userName = Console.ReadLine()!;
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }
    }
}

