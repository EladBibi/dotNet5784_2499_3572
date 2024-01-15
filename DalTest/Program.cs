namespace DalTest;
using Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class Program
{
   



    static readonly IDal s_dal = new DalList();

static void Main(string[] args)
     {

      


        string temp;


                 try
                 {
                     int choise, num;

            Initialization.Do(s_dal);



            do
                     {
                         Console.WriteLine("Enter a number to choose one of the following:\n" +
                         "0:Exit\n" +
                         "1:Engineer\n" +
                         "2:Task\n" +
                         "3:Dependency\n");
                         temp = Console.ReadLine()!;
                         choise = int.Parse(temp);
                         switch (choise)
                         {


                             case 1:

                                 do
                                 {
                                     Program.print();
                                     temp = Console.ReadLine()!;
                                     num = int.Parse(temp);
                                     switch (num)
                                     {

                                         case 2:

                                             Program.AddEngineer();
                                             break;
                                         case 3:
                                             int id;
                                             Console.WriteLine("Enter the ID of the engineer:");
                                             temp = Console.ReadLine()!;
                                             id = int.Parse(temp);
                                             try
                                             {
                                                 if (s_dal.Engineer!.Read(id) is null)



                                                     throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");

                                             }

                                             catch ( DalDoesNotExistException e)
                                             {
                                                 Console.WriteLine(e.Message);
                                             }

                                             Console.WriteLine(s_dal.Engineer!.Read(id));


                                             break;








                                         case 4:
                                             foreach (var Eng in s_dal.Engineer!.ReadAll())
                                                 Console.WriteLine(Eng);
                                             break;
                                         case 5:
                                             int ID;
                                             Console.WriteLine("Enter the number of the engineer you want to update");
                                             temp = Console.ReadLine()!;
                                             ID = int.Parse(temp);
                                             try
                                             {
                                                 if (s_dal.Engineer!.Read(ID) is null)
                                                     throw new DalDoesNotExistException($"Engineer with ID={ID} does Not exist");
                                                 else
                                                     UpdateEngineer(s_dal.Engineer!.Read(ID)!);
                                             }
                                             catch(DalDoesNotExistException e)
                                             {
                                                 Console.WriteLine(e.Message);
                                             }

                                             break;
                                         case 6:
                                             Console.WriteLine("Enter the number of the engineer you want to delete");
                                             int ide;
                                             temp = Console.ReadLine()!;
                                             ide = int.Parse(temp);
                                             try
                                             {
                                                 s_dal.Engineer!.Delete(ide);
                                             }
                                             catch (DalDoesNotExistException e)
                                             {
                                                 Console.WriteLine(e.Message);
                                             }
                                             break;
                                         case 1:
                                             break;
                                     }
                                     Console.WriteLine();
                                 } while (num != 1);
                                 break;



                             case 2:
                                 do
                                 {
                                     Program.print();
                                     temp = Console.ReadLine()!;
                                     num = int.Parse(temp);
                                     switch (num)
                                     {
                                         case 2:
                                             Program.AddTask();
                                             break;
                                         case 3:
                                             int id;
                                             Console.WriteLine("Enter the ID of the task:");
                                             temp = Console.ReadLine()!;
                                             id = int.Parse(temp);

                                             try
                                             {
                                                 if (s_dal.Task!.Read(id) is null)
                                                     throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
                                             }
                                             catch (DalDoesNotExistException e)
                                             {
                                                 Console.WriteLine(e.Message);
                                             }
                                             break;
                                         case 4:
                                             foreach (var TasK in s_dal.Task!.ReadAll())
                                                 Console.WriteLine(TasK);
                                             break;

                                         case 5:
                                             int ID;
                                             Console.WriteLine("Enter the number of the Task you want to update");
                                             temp = Console.ReadLine()!;
                                             ID = int.Parse(temp);
                                             try
                                             {
                                                 if (s_dal.Task!.Read(ID) is null)
                                                     throw new DalDoesNotExistException($"Task with ID={ID} does Not exist");
                                                 else
                                                     UpdateTask(s_dal.Task!.Read(ID)!);
                                             }
                                             catch (DalDoesNotExistException e)
                                             {
                                                 Console.WriteLine(e.Message);
                                             }

                                             break;
                                         case 6:
                                             Console.WriteLine("Enter the number of the taks you want to delete");
                                             int ide;
                                             temp = Console.ReadLine()!;
                                             ide = int.Parse(temp);
                                             try
                                             {
                                                 s_dal.Task!.Delete(ide);
                                             }
                                             catch (Exception e)
                                             {
                                                 Console.WriteLine(e.Message);
                                             }
                                             break;
                                         case 1:
                                             break;
                                     }
                                     Console.WriteLine();
                                 } while (num != 1);
                                 break;
                             case 3:
                                 do
                                 {
                                     Program.print();
                                     temp = Console.ReadLine()!;
                                     num = int.Parse(temp);
                                     switch (num)
                                     {
                                         case 2:
                                             Program.AddDependency();
                                             break;
                                         case 3:
                                             int id;
                                             Console.WriteLine("Enter the ID of the dependency:");
                                             temp = Console.ReadLine()!;
                                             id = int.Parse(temp);

                                             try
                                             {
                                                 if (s_dal.Dependency!.Read(id) is null)
                                                     throw new  DalDoesNotExistException($"Dependency with ID={id} does Not exist");
                                             }
                                             catch (DalDoesNotExistException e)
                                             {
                                                 Console.WriteLine(e.Message);
                                             }
                                             Console.WriteLine(s_dal.Dependency!.Read(id));
                                             break;
                                         case 4:
                                             foreach (var Dep in s_dal.Dependency!.ReadAll())
                                                 Console.WriteLine(Dep);
                                             break;
                                         case 5:
                                             int ID;
                                             Console.WriteLine("Enter the number of the Dependency you want to update");
                                             temp = Console.ReadLine()!;
                                             ID = int.Parse(temp);
                                             try
                                             {
                                                 if (s_dal.Dependency!.Read(ID) is null)
                                                     throw new  DalDoesNotExistException($"Dependency with ID={ID} does Not exist");
                                                 else
                                                     UpdateDependency(s_dal.Dependency!.Read(ID)!);
                                             }
                                             catch (DalDoesNotExistException e)
                                             {
                                                 Console.WriteLine(e.Message);
                                             }

                                             break;
                                         case 6:
                                             Console.WriteLine("Enter the number of the dependency you want to delete");
                                             int ide;
                                             temp = Console.ReadLine()!;
                                             ide = int.Parse(temp);
                                             try
                                             {
                                                 s_dal.Dependency!.Delete(ide);
                                             }
                                             catch (Exception e)
                                             {
                                                 Console.WriteLine(e.Message);
                                             }
                                             break;
                                         case 1:
                                             break;
                                     }
                                   Console.WriteLine();
                                 } while (num != 1);
                                 break;
                             case 0:
                                 break;



                         }

                     } while (choise != 0);

                 }
                 catch (Exception e)
                 {
                     Console.WriteLine(e.Message);

        }
        

    }
















    public static void print() { 
    
        Console.WriteLine("Enter the appropriate number to activate the desired method:\n" +
                         "1:Exit\n" +
                         "2:Adding a new object to the list\n" +
                         "3:Printing the object data\n" +
                         "4:Print all object data in the list\n" +
                         "5:Updating the object data\n" +
                         "6:Deleting an existing object from the list\n");
    }

    public static void AddDependency()
    {
        string temp;
        int DependentTask, DependsOnTask;

        Console.WriteLine("Enter the id of the dependent task:");
        temp = Console.ReadLine()!;
        DependentTask = int.Parse(temp);
        Console.WriteLine("Enter the ID of the task on which the task depends");
        temp = Console.ReadLine()!;
        DependsOnTask = int.Parse(temp);
        Dependency New = new Dependency(0, DependentTask, DependsOnTask);
        s_dal.Dependency!.Create(New);

    }


    public static void UpdateDependency(Dependency dep)
    {
        string temp;
        int DependentTask, DependsOnTask;
        DependentTask = dep.DependentTask;
        DependsOnTask = dep.DependsOnTask;
        Console.WriteLine("Enter the new data:");
        Console.WriteLine("Enter the new id of the dependent task:");
        temp = Console.ReadLine()!;
        if (temp != "")
            DependentTask = int.Parse(temp);
        Console.WriteLine("Enter the new id of the task on which the task depends");
        temp = Console.ReadLine()!;
        if (temp != "")
            DependsOnTask = int.Parse(temp);
        Dependency New = new Dependency(dep.Id, DependentTask, DependsOnTask);
        s_dal.Dependency!.Update(New);

    }
    












    public static void AddEngineer()

    {
        int id;
        double cost;
        string? name, email;
        string temp;
        DO.EngineerExperience level;
        Console.WriteLine("Enter the engineer's id:");
        temp = Console.ReadLine()!;
        id = int.Parse(temp);
        Console.WriteLine("Enter the salary:");
        temp = Console.ReadLine()!;
        cost = double.Parse(temp);
        Console.WriteLine("Enter the engineer's name:");
        name = Console.ReadLine();
        Console.WriteLine("Enter the engineer's email:");
        email =Console.ReadLine();
        Console.WriteLine("Enter the engineer's level:");
        temp = Console.ReadLine()!;
         level = (DO.EngineerExperience)Enum.Parse(typeof(EngineerExperience), temp); 
       


        Engineer New = new Engineer(id, cost, name, email, level);
        try
        {
            s_dal.Engineer!.Create(New);
        }
        catch(DalAlreadyExistsException e ) 
        {
            Console.WriteLine(e.Message);
        }
    }
    public static void UpdateEngineer(Engineer e){

        double cost = e.Cost;
        string? name = e.name;
        string? email = e.Email;
        string temp;
        DO.EngineerExperience? level = e.level;
        Console.WriteLine("Enter the new  data:");
        Console.WriteLine("Enter the new salary:");
        temp = Console.ReadLine()!;
        
        if (temp != "")
            cost = double.Parse(temp);
        Console.WriteLine("Enter the engineer's new name:");
        temp = Console.ReadLine()!;
        if (temp != "")
            name = temp;
        Console.WriteLine("Enter the engineer's new email:");
        temp = Console.ReadLine()!;
        if (temp != "")
            email = temp;
        Console.WriteLine("Enter the engineer's level:");
        temp = Console.ReadLine()!;
        if (temp != "")
            level = (DO.EngineerExperience)Enum.Parse(typeof(EngineerExperience), temp);
        Engineer New = new Engineer(e.Id, cost, name, email, level);
        s_dal.Engineer!.Update(New);
  
    }

    public static void AddTask()
    {
        
        int EngineerId;
        string? Alias;
        string? Description;
        string? Deliverables;
        string? Remarks;
        string temp;
        bool IsMilestone;
        DateTime? CreatedAtDate;
        DateTime? scheduledDate;
        DateTime? StartDate;
        DateTime? CompleteDate;
        DateTime? DeadLineDate;
        DO.EngineerExperience? level;
        TimeSpan? RequiredEffortTime;
       
        Console.WriteLine("Enter the engineer's id:");
        temp = Console.ReadLine()!;
        EngineerId = int.Parse(temp);
        Console.WriteLine("Enter the task's alias:");
        Alias = Console.ReadLine()!;
        Console.WriteLine("Enter the task's description:");
        Description = Console.ReadLine()!;
        Console.WriteLine("Enter the task's deliverables:");
        Deliverables = Console.ReadLine()!;
        Console.WriteLine("Enter the task's remarks:");
        Remarks = Console.ReadLine()!;
        Console.WriteLine("Is the task a milestone? Enter true or false:");
        temp = Console.ReadLine()!;
        IsMilestone = bool.Parse(temp);
        Console.WriteLine("Enter the task creation date");
        temp = Console.ReadLine()!;
        CreatedAtDate = DateTime.Parse(temp);
        Console.WriteLine("Enter the scheduled date of the task");
        temp = Console.ReadLine()!;
        scheduledDate = DateTime.Parse(temp);
        Console.WriteLine("Enter the start date of the task");
        temp = Console.ReadLine()!;
        StartDate = DateTime.Parse(temp);
        Console.WriteLine("Enter the complete date of the task");
        temp = Console.ReadLine()!;
        CompleteDate = DateTime.Parse(temp);
        Console.WriteLine("Enter the dead line Date of the task");
        temp = Console.ReadLine()!;
        DeadLineDate = DateTime.Parse(temp);
        Console.WriteLine("Enter the engineer level required for the task:");
        temp = Console.ReadLine()!;
        level = (DO.EngineerExperience)Enum.Parse(typeof(EngineerExperience), temp);
        Console.WriteLine("Enter the amount of time needed for the task:");
        temp = Console.ReadLine()!;
        RequiredEffortTime = TimeSpan.Parse(temp);
        Task NewTask = new Task(0, EngineerId, Alias, Description, Deliverables,
            Remarks, IsMilestone, CreatedAtDate, scheduledDate, StartDate,
            CompleteDate, DeadLineDate, level, RequiredEffortTime);
        s_dal.Task!.Create(NewTask);
    }
    public static void UpdateTask(Task tas)
    {
        int EngineerId = tas.Id;
        string? Alias = tas.Alias;
        string? Description = tas.Description;
        string? Deliverables = tas.Deliverables;
        string? Remarks = tas.Remarks;
        string temp;
        bool IsMilestone = tas.IsMilestone;
        DateTime? CreatedAtDate = tas.CreatedAtDate;
        DateTime? scheduledDate = tas.scheduledDate;
        DateTime? StartDate = tas.StartDate;
        DateTime? CompleteDate = tas.CompleteDate;
        DateTime? DeadLineDate = tas.DeadLineDate;
        DO.EngineerExperience? level = tas.Complexity;
        TimeSpan? RequiredEffortTime = tas.RequiredEffortTime;

        Console.WriteLine("Enter the new  data:");
        Console.WriteLine("Enter the new engineer's id:");
        temp = Console.ReadLine()!;

        if (temp != "")
            EngineerId = int.Parse(temp);
        Console.WriteLine("Enter the new task's alias:");
        temp = Console.ReadLine()!;
        if (temp != "")
            Alias = temp;
        Console.WriteLine("Enter the new task's Description");
        temp = Console.ReadLine()!;
        if (temp != "")
            Description = temp;
        Console.WriteLine("Enter the new task's Deliverables");
        temp = Console.ReadLine()!;
        if (temp != "")
            Deliverables = temp;
        Console.WriteLine("Enter the new task's remarks");
        temp = Console.ReadLine()!;
        if (temp != "")
            Remarks = temp;
        Console.WriteLine("Enter the new value in the task milestone:");
        temp = Console.ReadLine()!;
        if (temp != "")
            IsMilestone = bool.Parse(temp);
        Console.WriteLine("Enter the new value in the new create date of the task:");
        temp = Console.ReadLine()!;
        if (temp != "")
            CreatedAtDate = DateTime.Parse(temp);
        Console.WriteLine("Enter the new value in the new scheduled date date of the task:");
        temp = Console.ReadLine()!;
        if (temp != "")
            scheduledDate = DateTime.Parse(temp);
        Console.WriteLine("Enter the new value in the new start date of the task:");
        temp = Console.ReadLine()!;
        if (temp != "")
            StartDate = DateTime.Parse(temp);

        Console.WriteLine("Enter the new value in the new  Complete date date of the task:");
        temp = Console.ReadLine()!;
        if (temp != "")
            CompleteDate = DateTime.Parse(temp);
        Console.WriteLine("Enter the new value in the new  dead line date date of the task:");
        temp = Console.ReadLine()!;
        if (temp != "")
            DeadLineDate = DateTime.Parse(temp);
        Console.WriteLine("Enter the new visible engineer level");
        temp = Console.ReadLine()!;
        if (temp != "")
            level = (DO.EngineerExperience)Enum.Parse(typeof(EngineerExperience), temp);
        Console.WriteLine("Enter the amount of time needed for the task:");
        temp = Console.ReadLine()!;
        if (temp != "")
            RequiredEffortTime = TimeSpan.Parse(temp);
        Task New = new Task(tas.Id, EngineerId, Alias, Description, Deliverables,
            Remarks, IsMilestone, CreatedAtDate, scheduledDate, StartDate,
            CompleteDate, DeadLineDate, level, RequiredEffortTime);
        s_dal.Task!.Update(New);
    }




    















}






        





    