


using BlApi;
using BO;

using DO;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private readonly IBl _bl;
    internal TaskImplementation(IBl bl) => _bl = bl;


    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int DepentedTask { get; private set; }

    public bool Schedule_date()
    {
        return (_dal.Task.ReadAll(k => k.scheduledDate == null).Any());

    }

 

   public bool finish_project()
    {
        return _bl.GetDate("StartDate") != DateTime.MinValue &&
            _dal.Task.ReadAll(p => getstatus(p) != Status.Done).Any() is false;
            
    }

    public int Create(BO.Task item)
    {
        if (_bl.GetDate("FinishDate") != DateTime.MinValue)
            throw new BlLogicalErrorException("The project is finished!");




        if (_bl.GetDate("StartDate") != DateTime.MinValue)
            throw new BlLogicalErrorException("It is not possible to create new tasks after the start of the project");



        if (item.Alias is null || item.Alias == "")
            throw new BlInvalidInputException("The data you entered is incorrect for the task");
        int EngineerId = 0;
        if (item.Engineer is not null)
        {
            if (check_id_engineer(item.Engineer.Id) is true && item.Engineer.Id != 0)
                throw new BlInvalidInputException("The data you entered is incorrect for the task's engineer");

            EngineerId = item.Engineer.Id;
        }


        if (item!.Dependencies is null)
            item.Dependencies = new List<BO.TaskInList>();
        if (item.Complexity is null)
            item.Complexity = BO.EngineerExperience.Beginner;

       
        DO.Task doTask = new DO.Task(item.Id, EngineerId, item.Alias, item.Description, item.Deliverables,
            item.Remarks, _bl.Clock, item.ScheduledDate, item.StartDate, item.CompleteDate,
             (DO.EngineerExperience?)item.Complexity, item.RequiredEffortTime);

        int idTask;
        try
        {
            idTask = _dal.Task.Create(doTask);
        }

        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={item.Id} already exists", ex);
        }

        if (item!.Dependencies is null)
            item.Dependencies = new List<BO.TaskInList>();

        return idTask;
    }


    public bool check_level_engineer(int id, BO.EngineerExperience? level)
    {
        if (id == 0)
            return false;
        
    
        if (level is not null)
        {
            return (int)(BO.EngineerExperience)_dal.Engineer.Read(id)!.level! < (int)level;
        }
        return true;
    }

    public void reset_schdule_date()
    {
        foreach (var temp in ReadAll(p => p.scheduledDate != null))
            UpdateDate(null, temp.Id, "schedule");
    }

    public bool list_task_for_engineer(DO.Task do_task, int id)
    {
        DO.Engineer do_eng = _dal.Engineer.Read(id)!;
        BO.Task bo_task = Read(do_task.Id)!;
        bool flag = false;
        if (bo_task.Dependencies is null)
        {

            var tasks = (from dep in bo_task.Dependencies
                         where dep.Status != BO.Status.Done
                         select dep).ToList();

            flag = tasks.Any();

        }



        return (do_task.EngineerId == 0 && !flag && (int)do_eng.level! >= (int)bo_task.Complexity! );

    }



    

    public bool check_id_engineer(int id)
    {
        return !(_dal.Engineer.ReadAll(k => k.Id == id).Any());
    }


   

    public void Delete(int id)
    {
        if (_bl.GetDate("StartDate") != DateTime.MinValue)
            throw new BlLogicalErrorException("It is not possible to delete tasks after the start of the project");
               



        if (_dal.Dependency.ReadAll(K => K.DependsOnTask == id).Any())
            throw new BlNullPropertyException("It is not possible to delete a task that depends on other tasks");
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exists", ex);
        }
    }

    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask is null)
            return null;

        IEnumerable<Dependency> depList = _dal.Dependency.ReadAll(t => t.DependentTask == id);
        DO.Engineer eng = _dal.Engineer.Read(doTask.EngineerId) ?? new();

        if (depList is null)
            depList = new List<Dependency>();
        
      


        return new BO.Task()
        {
            Id = id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            CreatedAtDate = doTask.CreatedAtDate,
            ScheduledDate = doTask.scheduledDate,
            StartDate = doTask.StartDate,
            CompleteDate = doTask.CompleteDate,
            ForeCastDate = forcaste(doTask),
            Complexity = (BO.EngineerExperience?)doTask.Complexity,
            RequiredEffortTime = doTask.RequiredEffortTime,
            Status = getstatus(doTask),
            Dependencies = (from item in depList
                            select new TaskInList()
                            {
                                Id = item.DependsOnTask,
                                Description = _dal.Task.Read(item.DependsOnTask)!.Description,
                                Alias = _dal.Task.Read(item.DependsOnTask)!.Alias,
                                Status = 0
                            }).ToList(),
            Engineer = new EngineerInTask() { Id = doTask.EngineerId, Name = eng.name }
        };
    }


    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null)
    {



        return (filter is not null) ?
            (from DO.Task doTask in _dal.Task.ReadAll(p => filter(p))
               
             select new BO.TaskInList
             {
                 Id = doTask.Id,
                 Description = doTask.Description,
                 Alias = doTask.Alias,
                 Status = getstatus(doTask)
             }) :
                 (from DO.Task doTask in _dal.Task.ReadAll()

                  select new BO.TaskInList
                  {
                      Id = doTask.Id,
                      Description = doTask.Description,
                      Alias = doTask.Alias,
                      Status = getstatus(doTask)
                  });

    }



    public void update_engineer_id(int eng, int task)
    {
        if (_dal.Task.ReadAll(k => k.EngineerId == eng && getstatus(k) == Status.OnTrack).Any() is true)
            throw new BlLogicalErrorException("The engineer is already assigned a task");
        
        
        
        var Tasks = from DO.Dependency doDependency in _dal.Dependency.ReadAll(p => p.DependentTask == task)
                    let date = _dal.Task.Read(doDependency.DependsOnTask)!.scheduledDate
                    where date is not null
                    select doDependency;

        if (Tasks.Any(p => getstatus(_dal.Task.Read(p.DependsOnTask)!) != Status.Done) is true)
            throw new BlLogicalErrorException("Not all pending tasks have been completed!");
                

        DO.Task update_do_task = _dal.Task.Read(task)! with { EngineerId = eng };
        _dal.Task.Update(update_do_task);

    }



    public void remove_engineer_from_task(int task_id)
    {
        DO.Task do_task = _dal.Task.Read(task_id)!;
        if (getstatus(do_task) == BO.Status.OnTrack)
            throw new BlLogicalErrorException
                ("You cannot remove an engineer from a task that has already started working");
        DO.Task task = do_task with { EngineerId = 0 };
        _dal.Task.Update(do_task);
    }





    public void Update(BO.Task item)
    {
        if (CheckData(item))
            throw new BlInvalidInputException("The data you entered is incorrect for the update task");

        DO.Task OldTask = _dal.Task.Read(item.Id) ?? throw
        new BO.BlAlreadyExistsException($"Task with ID={item.Id} does not exists");

        if (OldTask.CompleteDate != item.CompleteDate)
        {
            if (OldTask.StartDate is null || item.StartDate is null)
                throw new BlLogicalErrorException("You cannot enter an end date before the task has started");
            if (item.CompleteDate < item.StartDate)
                throw new BlLogicalErrorException("The task cannot be finished before it has started");
        }


        //int eng_id = 0;

        if (_bl.GetDate("StartDate") != DateTime.MinValue)
        {
            if (item.Engineer is null)
                item.Engineer = new EngineerInTask { Id = 0 };
            if (check_level_engineer(item.Engineer.Id, item.Complexity) is true)
                throw new BlInvalidInputException("The engineer you entered does not match the mission level");
        }




        

        if (item.StartDate is not null)
            if (item.StartDate < _bl.GetDate("StartDate"))
                throw new BlInvalidInputException("The start date can't be earlier the project's date start");

        DO.Task NewdoTask = new DO.Task(item.Id, item.Engineer!.Id, item.Alias, item.Description, item.Deliverables,
           item.Remarks, item.CreatedAtDate, item.ScheduledDate, item.StartDate, item.CompleteDate,
            (DO.EngineerExperience?)item.Complexity, item.RequiredEffortTime);

        _dal.Task.Update(NewdoTask);



    }

    public IEnumerable<BO.TaskInGantt> GanttList(DateTime date)
    {
        
        var x = from task in _dal.Task.ReadAll()
                select new BO.TaskInGantt()
                {
                    
                    
                    Id = task.Id,
                    Name = task.Alias!,
                    StartOffset =(task.StartDate is null)? (int)(task.scheduledDate-date )!.Value.TotalHours:
                    (int)(task.StartDate - date)!.Value.TotalHours,
                    TaskLenght =(task.CompleteDate is null)? (int)task.RequiredEffortTime!.Value.TotalHours:
                        (int)(task.CompleteDate - task.StartDate)!.Value.TotalMinutes+15,
                    Status = getstatus(task),
                    CompliteValue = CalcValue(task),
                    Dependencies_id = (from dep in _dal.Dependency.ReadAll(p=>p.DependentTask == task.Id)
                                       select dep.DependsOnTask).ToList()
               };
        return x;
    }
   

    bool CheckData(BO.Task item)
    {
        return (item.Id <= 0 || item.Alias is null || item.Alias == "");
    }

   
    
    
    
       
    public void UpdateDate(DateTime? d, int id, string s)
    {
        DO.Task? dotask = _dal.Task.Read(id);
        if (dotask is null)
            throw new BlNullPropertyException($"Task with ID={id} does Not exist");



        if (d is not null)
        {
            if (((DateTime)d).Date < _bl.Clock.Date)
                throw new BlLogicalErrorException("The date you entered has already passed");

            if (_bl.GetDate("StartDate") == DateTime.MinValue)
                throw new BlLogicalErrorException("Cant not update dates before you enter a start date for the project");

            if (((DateTime)_bl.GetDate("StartDate")!).Date > ((DateTime)d).Date)
                throw new BlLogicalErrorException("The start date cannot be earlier than the project start date");




            var Tasks = from DO.Dependency doDependency in _dal.Dependency.ReadAll(p => p.DependentTask == id)
                        select doDependency;
            
            if(Tasks.Any(p=>_dal.Task.Read(p.DependsOnTask)!.scheduledDate is null) is true)
            throw new BlLogicalErrorException("The dates of the tasks on which the task depends are not yet updated");
            if (s == "schedule" && Tasks.FirstOrDefault(p => forcaste(_dal.Task.Read(p.DependsOnTask)!) > d) is not null)
                throw new BlLogicalErrorException("It is not possible to update a date earlier than finish of pending task");
                    
        }
        DO.Task? UpdateTask;
        if ( s== "schedule")
            UpdateTask = dotask with { scheduledDate = d };
        else
             UpdateTask = dotask with { StartDate = d };





        _dal.Task.Update(UpdateTask);
        
    }  

    public void AddDependency(int IdDepented, int IdDepentedOn)


    {

        if (IdDepented == IdDepentedOn)
            throw new BlLogicalErrorException("A task cannot depend on itself");

        if (_bl.GetDate("StartDate") != DateTime.MinValue)
            throw new BlLogicalErrorException("Dependencies cannot be added after the execution phase has started");



        BO.Task? Task_Depented = Read(IdDepented);
        BO.Task? botask = Read(IdDepentedOn);
        if (botask is null || Task_Depented is null)
            throw new BlNullPropertyException($"The task does not exist");
        if (Task_Depented.Dependencies is not null)
        {
            if (Task_Depented.Dependencies.Any(k => k.Id == IdDepentedOn) is true)
                throw new BlLogicalErrorException("The task already depends on this task");

        }
        if (Circularity_test(IdDepented, IdDepentedOn) is true)
            throw new BlNullPropertyException("There is a circular dependency in the tasks!");

        BO.TaskInList newTask = new BO.TaskInList()
        {
            Id = IdDepentedOn,
            Description = botask.Description,
            Alias = botask.Alias,
            Status = botask.Status
        };
        BO.Task? temp = Read(IdDepented);



        DO.Dependency doDependency = new DO.Dependency(0, IdDepented, IdDepentedOn);
        _dal.Dependency.Create(doDependency);
        temp!.Dependencies!.Add(newTask);
    }


    public bool Circularity_test(int task_id, int dep_id)
    {
        if (task_id == dep_id)
            return true;

        BO.Task? dep_task = Read(dep_id);

        if (dep_task is not null)
        {
            if (dep_task.Dependencies is null)
                return false;
        }
        else
            throw new BlNullPropertyException("The task does not exist");

        foreach (var task in dep_task.Dependencies)
        {
            if (Circularity_test(task_id, task.Id) is true)
                return true;

        }




        return false;

    }


    public void DeleteDependency(int IdDepented, int IdDepentedOn)
    {
        if (_bl.GetDate("StartDate") != DateTime.MinValue)
            throw new BlLogicalErrorException("Dependencies cannot be deleted after the execution phase has started");

        BO.Task? Task_Depented = Read(IdDepented);
        BO.Task? botask = Read(IdDepentedOn);
        if (botask is null || Task_Depented is null)
            throw new BlNullPropertyException($"The task does not exist");
        if (Task_Depented.Dependencies is not null)
            if (Task_Depented.Dependencies.RemoveAll(x => x?.Id == IdDepentedOn) == 0)
                throw new BlNullPropertyException($"The task does not depend on the task you entered");





        DO.Dependency? dep_for_delete = _dal.Dependency.Read(x => x.DependentTask == IdDepented
        && x.DependsOnTask == IdDepentedOn);
        if (dep_for_delete is not null)
            _dal.Dependency.Delete(dep_for_delete.Id);

    }

    void UpdateEngineer(int id, EngineerInTask engineer)
    {
        if (_bl.GetDate("StartDate") == DateTime.MinValue)
            throw new BlLogicalErrorException("It is not possible to assign an engineer before the start of the execution phase");




        DO.Task? dotask = _dal.Task.Read(id);
        DO.Engineer? doengineer = _dal.Engineer.Read(engineer.Id);
        if (engineer.Name is null || engineer.Name == "" || engineer.Id <= 0 || id <= 0 || dotask is null || doengineer is null)
            throw new BlInvalidInputException("The data entered does not match");


        if ((int?)dotask.Complexity > (int?)doengineer.level)
            throw new BlLogicalErrorException("The task level is not suitable for an engineer");
        //מאחר והמנהדס עובר למשימה חדשה צמריך לבדוק אם הייתה לו משימה קודמת ולעדכן בהתאם לכך שזמן הסיום יהיה כעת ואז הסטטוס יהיה done 

        var item = _dal.Task.ReadAll(p => p.EngineerId == engineer.Id);
        if (item.Any())
            foreach (var item2 in item)
                if (item2.CompleteDate > _bl.Clock.Date)
                {
                    DO.Task NewTask = item2 with { CompleteDate = _bl.Clock.Date };
                    _dal.Task.Update(NewTask);
                }







        DO.Task UpdateTask = dotask with { EngineerId = engineer.Id };
        _dal.Task.Update(UpdateTask);

    }

    DateTime? forcaste(DO.Task T)
    {

        DateTime? max = T.StartDate >= T.scheduledDate ? T.StartDate : T.scheduledDate;

        max = max + T.RequiredEffortTime;

        return max;
   }


    // פונקציית עזר להחזרת כל המשימות שבה המשימה המתקבלת תלויה, בשביל
    public List <DO.Task> Dependecies_Tasks(DO.Task T)
    {
        return (from dep in _dal.Dependency.ReadAll(p => p.DepentedTask == T.Id)
               select _dal.Task.Read(dep.DependsOnTask)).ToList();
    }



    

    public Status getstatus(DO.Task T)
    {
        
       
        int i = 0;
        if (T.scheduledDate is not null)
        {
            if (Dependecies_Tasks(T).Any(t => getstatus(t) == Status.Delayed) is true)
                i = 4;
            else
                i = 1;

        }
           
        else
            return (Status)0;

        if (T.StartDate is not null)
        {

            if (forcaste(T) < _bl.Clock.Date)
                i = 4;
            else
                i = 2;

        }
        if (T.CompleteDate is not null)
           
                i = 3;

        switch (i)
        {
            case 0:
                return (Status)0;

            case 1:
                return (Status)1;

            case 2:
                return (Status)2;

            case 3:
                return (Status)3;
            case 4:
                return (Status)4;   


            default:
                return 0;

        }
    }


    public void finish_task(int id )
    {

        DO.Task task = _dal.Task.Read(id)! with { CompleteDate = DateTime.Today };
        //if(forcaste(task) > _bl.Clock.Date)
            //throw new BlLogicalErrorException("You didn't work long enough on the task");


        _dal.Task.Update(task);
        
        
    }

    public bool time_required()
    {
        return (_dal.Task.ReadAll(k => k.RequiredEffortTime == null).Any());

    }


    //מתודת עזר בשביל חישוב האם כל המשימות התלוות במשימה זו הסתיימו
    bool EndOfTasks(DO.Task T)
    {
        if (_dal.Dependency.ReadAll(p => p.DependentTask == T.Id)
            .FirstOrDefault(k => getstatus(_dal.Task.Read(k.DependsOnTask)!) != BO.Status.Done) is not null)
            return false;
        return true;
    }

    private int CalcValue(DO.Task task)
    {
        if (task.StartDate is null)
            return 0;

        DateTime clock = Factory.Get().Clock;
        if (clock > task.StartDate && task.CompleteDate is null)
            return (int)((double)(clock - task.StartDate).Value.TotalDays /
                (double)task.RequiredEffortTime!.Value.TotalDays) * 100;

        return 0;
    }

    public void ScheduleTasks(DateTime startDate)
    {
        Dictionary<int, DO.Task> tasks = _dal.Task.ReadAll().ToList().ToDictionary(task => task.Id);
        List<Dependency> dependencies = _dal.Dependency.ReadAll().ToList();


        // Initialize the schedule with tasks that have no dependencies
        Dictionary<int, DO.Task> schedule = tasks.Where(task => !dependencies.Any(dep => dep.DependentTask == task.Key)).
            Select(task => task.Value).ToList().ToDictionary(task => task.Id);

        foreach (int key in schedule.Keys)
        {
            DO.Task old = schedule[key];
            TimeSpan? lenghTask = old.RequiredEffortTime;
            old = old with { scheduledDate = startDate, DeadLine = startDate + lenghTask };
            schedule[key] = old;
        }

        foreach (int task in tasks.Keys)
        {
            if (schedule.ContainsKey(task))
                tasks.Remove(task);
        }


        while (tasks.Count > 0)
        {
            foreach (int newTask in tasks.Keys)
            {
                bool canSchedule = true;

                foreach (Dependency dep in dependencies.Where(dep => dep.DependentTask == newTask))
                {
                    if (!schedule.ContainsKey(dep.DependsOnTask))
                    {
                        canSchedule = false;
                        break;
                    }
                }

                if (canSchedule)
                {
                    DateTime? earlyStart = DateTime.MinValue;
                    DateTime? lastDepDate = DateTime.MinValue;

                    foreach (Dependency dep in dependencies.Where(dep => dep.DependentTask == newTask))
                    {
                        lastDepDate = schedule[dep.DependsOnTask].DeadLine;
                        if (lastDepDate > earlyStart)
                            earlyStart = lastDepDate;
                    }
                    tasks[newTask] = tasks[newTask] with { scheduledDate = earlyStart, DeadLine = earlyStart + tasks[newTask].RequiredEffortTime };

                    schedule.Add(newTask, tasks[newTask]);
                    tasks.Remove(newTask);
                }
            }
        }

        schedule.Values.ToList().ForEach(task => { _dal.Task.Update(task); });
    }


}






