using DO;
using System.Net.Http.Headers;
using System.Reflection;

namespace DalTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World");

            MyStaticClass.myStaticVariable = 78;





        }
    }
}

public record StudentA
{
    public  required int Id { get; init; } = 7;
    public required string? Name { get; init; } =null;
    public string? Alias { get; init; } =null;
    public bool IsActive { get; init; } = false;
    public DateTime RegistrationDate => DateTime.Now;
    //public StudentA(): this() { }
}


//stA1.Id = 11; //compilation error. Id prop is immutable
public static class MyStaticClass

{

    public static int myStaticVariable = 0;

    public static void MyStaticMethod()

    {

        Console.WriteLine("This is a static method.");

    }

    public static int MyStaticProperty { get; set; }

}