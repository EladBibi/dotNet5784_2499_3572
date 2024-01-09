
namespace DalTest;

using Dal;
using DalApi;
using DO;


public static class Initialization
{

    private static IEngineer? s_dalEngineer;
    private static ITask? s_dalTask;
    private static IDependency? s_dalDependency;
    private static readonly Random s_rand = new Random();

    private static void createTasks()
    {
        for (int i = 0; i < 25; ++i)
        {
            
            DateTime DateTimeCreate = DateTime.Now;
            
            Task NewTask = new Task(0, 0, "", "", "", "", false,
                DateTimeCreate, null, null, null,
                null, (DO.EngineerExperience)(i % 5),null);
            s_dalTask!.Create(NewTask);
        }
    }
    private static void createEngineers()
    {
        string[] Names = {

        "Avi","Levi", "Eli","Amar", "Yair","Cohen",
        "Moshe","Levin", "Daniel", "Klein" ,"Ori", "Israelof"};
        int id;
        for (int i = 0; i <11; i+=2)
        {
            string name = Names[i] + ' ' + Names[i + 1];
            double cost = s_rand.Next(1500, 3001);
            string email = Names[i] + Names[i + 1] + (i + 100) + "@gmail.com";
            do
                id = s_rand.Next(100000000, 1000000000);
            while (s_dalEngineer!.Read(id) is not null);
            Engineer NewEngineer = new Engineer(id, cost, name, email, (DO.EngineerExperience)(i % 5));
            s_dalEngineer.Create(NewEngineer);
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
                    s_dalDependency!.Create(NewD);

                }
                j = i + 2;
                ++i;
            }
        }

    }



    public static void Do(ITask? dalTask, IEngineer? dalEngineer, IDependency? dalDependency)
    {

        s_dalTask = dalTask ?? throw new NullReferenceException("Task can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("Engineer can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("Dependency can not be null!");
        createEngineers();
        createTasks();
        createDependencies();
    }



}





        


    

















    

