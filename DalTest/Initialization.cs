
namespace DalTest;

using Dal;
using DO;

internal class Intialize
{

    private static readonly Random s_rand = new Random();
    private const int MIN_ID = 100000000;
    private const int MAX_ID = 1000000000;

    private static void createTasks()
    {
        for (int i = 0; i < 25; ++i)
        {

            DateTime DateTimeCreate = DateTime.Now;

            Task NewTask = new Task(0, 100000000 + i, "", "", "", "", false,
                DateTimeCreate, null, null, null,
                null, (DO.EngineerExperience)(i % 5), null);
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



    public static void Do()
    {

        s_dal = DalApi.Factory.Get;
        createEngineers();
        createTasks();
        createDependencies();
    }
}
































