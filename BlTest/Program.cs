using System.Xml.Serialization;
using BlApi;
using BO;
//using DO;
using Task = BO.Task;

namespace BlTest;


internal class Program
{
    static readonly IBl bl = BlApi.Factory.Get();
    static readonly string TaskPath = $@"..\\xml\\TaskTest.xml";
    static readonly string EngPath = $@"..\\xml\\EngineerTest.xml";


    static void Main()
    {

        while (true)
        {
            Console.WriteLine(
                "to engineer action press 1\n" +
                "to tak action press 2\n" +
                "to intialize the data press 3 \n" +
                "to add start project value press 4\n" +
                "to add end project value press 5\n" +
                "EXIT press 6\n");

            int action = int.Parse(Console.ReadLine()!);

            switch (action)
            {
                case 1:
                    EngineerMain();
                    break;

                case 2:
                    TaskMain();
                    break;

                case 3:
                    Console.Write("Would you like to create Initial data? (Y/N)\n");
                    string ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                    if (ans == "Y") 
                        DalTest.Initialization.Do();
                    break;
                case 6:
                    return;
                     
                default:
                    break;
            }
        }
    }

    static void EngineerMain()
    {

        int Id;
        while (true)
        {
            Console.WriteLine(
                "Add engineer press 1\n" +
                "Read engineer press 2\n" +
                "Update engineer press 3\n" +
                "Read all engineers press 4\n" +
                "Delete engineer press 5\n" +
                "Back to main press 6\n");

            int action = int.Parse(Console.ReadLine()!);
            try
            {
                switch (action)
                {
                    case 1:
                        Console.WriteLine("Enter the data to test xml and press Y");
                        string accept = Console.ReadLine()!;
                        if (accept == "Y")
                            try
                            {
                                bl.Engineer.Create(LoadTest<Engineer>(EngPath));
                            }
                            catch(DalXMLFileLoadCreateException ex)
                            {
                                Console.WriteLine( ex.Message);
                            }

                        break;

                    case 2:
                        Console.Write("Enter Engineer Id: ");
                        Id = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(bl.Engineer!.Read(Id));
                        break;
                       
                    case 3:
                    
                        Console.WriteLine("Enter the data to test xml and press Y");
                        accept = Console.ReadLine()!;
                        if (accept == "Y")
                            try
                            {
                                bl.Engineer.Update(LoadTest<Engineer>(EngPath));
                            }
                            catch (DalXMLFileLoadCreateException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        break;

                    case 4:
                        foreach (var eng in bl.Engineer.ReadAll())
                            Console.WriteLine(eng);
                        break;
                    case 5:
                        Console.WriteLine("enter id\n");
                        Id = int.Parse(Console.ReadLine()!);
                        bl.Engineer.Delete(Id);

                        break;
                    case 6:
                        return;
                    default:
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

    }

    static void TaskMain()
    {
        int id;
        while (true)
        {
            Console.WriteLine(
                "Add task press 1\n" +
                "Read task press 2\n" +
                "Read all tasks press 3\n" +
                "Update task press 4\n" +
                "Delete task press 5\n" +
                "Update date press 6" +
                "Back to main press 7\n");

            int action = int.Parse(Console.ReadLine()!);
            try
            {
                switch (action)
                {
                    case 1:
                        Console.WriteLine("Enter the data to test xml and press Y");
                        string accept = Console.ReadLine()!;
                        if (accept == "Y")
                            try
                            {
                                bl.Task.Create(LoadTest<Task>(TaskPath));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                                break;


                                

                    case 2:
                   
                        Console.Write("Enter Task Id: ");
                        id = int.Parse(Console.ReadLine()!);
                        try
                        {
                            Console.WriteLine(bl.Task!.Read(id));
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine( e.Message);
                        }

                            
                        break;

                        
                    case 3:
                        Console.WriteLine("Enter the data to test xml and press Y");
                        accept = Console.ReadLine()!;
                        if (accept == "Y")
                            try
                            {
                                bl.Task.Update(LoadTest<Task>(EngPath));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }


                        break;

                    case 4:
                       
                            foreach (var eng in bl.Task.ReadAll())
                                Console.WriteLine(eng);
                        

                        break;

                    case 5:
                        Console.WriteLine("enter the id of the task");
                        id = int.Parse(Console.ReadLine()!);
                        try
                        {
                            bl.Task.Delete(id);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;

                    case 6:
                        DateTime d;
                        Console.WriteLine("enter id of the task");
                        id = int.Parse(Console.ReadLine()!);
                        Console.WriteLine( "Enter the date you want to update");
                        if (!DateTime.TryParse(Console.ReadLine()!, out d))
                            throw new FormatException("Wrong input");
                        try
                        {
                            bl.Task.UpdateDate(d, id);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine( e.Message);
                        }




                        break;

                    case 7:
                        return;

                    default:
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

    }

    private static Item LoadTest<Item>(string filePath) where Item : class
    {
        using FileStream file = new(filePath, FileMode.Open);
        XmlSerializer x = new(typeof(Item));
        return x.Deserialize(file) as Item ?? throw new DalXMLFileLoadCreateException("Can't load the field");
    }

}