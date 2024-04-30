
namespace DalTest;

using Dal;
using DalApi;
using DO;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Initialization
{
    private static readonly Random s_rand = new Random();
    private static readonly int MIN_ID = 100000000;
    private static readonly int MAX_ID = 1000000000;
    private static IDal? s_dal = null;

    private static void createTasks()
    {
        for (int i = 0; i < 25; ++i)
        {
            // TimeSpan? span =  TimeSpan.FromDays(s_rand.Next(7, 22));
            TimeSpan? span = new TimeSpan(1, s_rand.Next(1, 10), 0);
            DateTime DateTimeCreate = DateTime.Now;

            Task NewTask = new Task(0, 1 + i, "", "", "", "", DateTimeCreate, null, null, null, 
                (DO.EngineerExperience)(i % 5),RequiredEffortTime:  span);
            s_dal!.Task.Create(NewTask);
        }
    }
    private static void createEngineers()
    {
        string[] Names = {

        "Avi","Levi", "Eli","Amar", "Yair","Cohen",
        "Moshe","Levin", "Daniel", "Klein" ,"Ori", "Israelof"};
        int id;
        for (int i = 0; i < 11; i += 2)
        {
            string name = Names[i] + ' ' + Names[i + 1];
            double cost = s_rand.Next(1500, 3001);
            string email = Names[i] + Names[i + 1] + (i + 100) + "@gmail.com";
            do
                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dal!.Engineer.Read(id) is not null);
            Engineer NewEngineer = new Engineer(id, cost, name, email, (DO.EngineerExperience)(i % 5));
            s_dal!.Engineer.Create(NewEngineer);
        }
    }

    //המתודה מציבה ערכים בשיטיות כך שכל שני משימות סמוכות יהיו תלויות בשלושת המשימות הבאות וסה"כ יהיו 42 תלויות
    private static void createDependencies()
    {
        for (int i = 1; i <= 13;)
        {

            for (int k = 0, j = i + 2; k < 2; ++k)
            {

                for (int l = 0; l < 3; ++l, ++j)
                {
                    Dependency NewD = new Dependency(0, i, j);
                    s_dal!.Dependency!.Create(NewD);

                }
                j = i + 2;
                ++i;
            }
        }

    }
    //זוהי מתודה ציבורית דו, שתזמן את המתודות הפרטיות שהכנו ותחולל את האתחולים של הרשימות 
    public static void Do()
    {
        if (s_dal is null)
            s_dal = DalApi.Factory.Get;

          Reset();
        SetDates(DateTime.MinValue,"StartDate");
        SetDates(DateTime.MinValue,"FinishDate");
        createEngineers();
        createTasks();
        createDependencies();
    }

    public static void Reset()
    {
        if (s_dal is null)
            s_dal = DalApi.Factory.Get;
            s_dal.Engineer.DeleteAll();
            s_dal.Task.DeleteAll();
            s_dal.Dependency.DeleteAll();
        SetDates(DateTime.MinValue, "StartDate");
        SetDates(DateTime.MinValue, "FinishDate");


    }


    public static int Get_Task_Id()
    {
       
        if (s_dal is null)
            s_dal = DalApi.Factory.Get;
        Task? task = s_dal.Task.ReadAll().LastOrDefault();
        int id = task is  not null ? task.Id : 0;
        return id;

    }

    public static DateTime? GetDates(string date)
    {
        XElement root = XElement.Load(@"..\xml\data-config.xml");
        return DateTime.TryParse((string?)root.Element(date), out var result) ? (DateTime?)result : null;

    }

    public static DateTime Get_Sistem_Date()
    {
        XElement root = XElement.Load(@"..\xml\data-config.xml");
        return (DateTime.TryParse((string?)root.Element("SistemDate"), out var result) ? (DateTime)result : DateTime.Now).
            AddSeconds(1);
           

    }


    public static void Set_Sistem_Date()
    {
       
        XElement root = XElement.Load(@"..\xml\data-config.xml");
        root.Element("SistemDate")?.SetValue((DateTime.Now).ToString());
        root.Save(@"..\xml\data-config.xml");
    }


    public static void SetDates(DateTime d, string date)
    {
        
        XElement root = XElement.Load(@"..\xml\data-config.xml");
        root.Element(date)?.SetValue((d).ToString());
        root.Save(@"..\xml\data-config.xml");
    }

    public static string? GetDataBase()
    {
        XElement root = XElement.Load(@"..\xml\dal-config.xml");
        return (string?)root.Element("dal");
    }
            

    













}
































