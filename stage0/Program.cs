namespace targil0
{


    enum Days { Sat, Sun, Mon, Tue, Wed, Thu, Fri };
    partial class Program
    {


        //Console.WriteLine("Hello123456789, World11!");




        private static void Main(string[] args)
        {
            //Console.WriteLine("Hello123456789, World11!");
           
             int f = int.Parse("123");
            Console.WriteLine("{0} {1}" ,f,25);

            //Welcome3572();
            Welcome2499();
            //Console.ReadKey();
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







