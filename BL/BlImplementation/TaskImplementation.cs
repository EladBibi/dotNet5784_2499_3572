

namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Task item)
    {
        if (item.Id <= 0 || item.Alias == "" || item.Alias is null)
            throw new Exception("ERROR");
        int EngineerId = 0;
        if (item.Engineer is not null)
            EngineerId = item.Engineer.Id;



        DO.Task doTask = new DO.Task(item.Id, EngineerId, item.Alias, item.Deliverables, item.Description,
            item.Remarks, item.CreatedAtDate, item.ScheduledDate, item.StartDate, item.CompleteDate,
             (DO.EngineerExperience?)item.Complexity, item.RequiredEffortTime);
        int idTask;
        try
        {
            idTask = _dal.Task.Create(doTask);
        }

        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={item.Id} already exists", ex);
        }


        var result = from Object in item.Dependencies
                     select new DO.Dependency(0, item.Id, Object.Id);
        foreach (var temp in result)
            _dal.Dependency.Create(temp);
        return idTask;
    }



    public void CreateSchedule()
    {
        
    }

    public void Delete(int id)
    {
        DateTime? temp = _dal.GetDates("StartDate");
        if (temp is not null)
            if (temp >= DateTime.Now)
                throw new Exception("error");

        DO.Task? DeleteTask = _dal.Task.Read(s => s.Id == id);
        if ( DeleteTask is null || _dal.Dependency.ReadAll(K => K.DependsOnTask == id) is not null)
            throw new Exception("error");
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={id} does not exists", ex);
        }
    }





    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Student with ID={id} does Not exist");
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
            Dependencies = (from item in _dal.Dependency.ReadAll(t => t.DependentTask == id)
                            select new TaskInList()
                            {
                                Id = item.DependsOnTask,
                                Description = _dal.Task.Read(item.DependsOnTask)!.Description,
                                Alias = _dal.Task.Read(item.DependsOnTask)!.Alias,
                                Status = 0
                            }).ToList(),
            Engineer = new EngineerInTask() { Id = doTask.EngineerId, Name = _dal.Engineer.Read(doTask.EngineerId)!.name }
        };
    }













      
    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null)
    {
    


        return (filter is not null) ?
            (from DO.Task doTask in _dal.Task.ReadAll(p =>filter(p)) 
            //(int?)p.Complexity <= (int?)(DO.EngineerExperience)level && p.EngineerId == 0
                //&& EndOfTasks(p) || (true ||filter(p)))
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

    



    public void Update(BO.Task item)
    {
        if (item.Id <= 0 || item.Alias is null || item.Alias == "" || item.Engineer!.Id <= 0)
            throw new Exception("error");

        DO.Task OldTask = _dal.Task.Read(item.Id) ?? throw
        new BO.BlAlreadyExistsException($"Student with ID={item.Id} does not exists");




        DO.Task NewdoTask = new DO.Task(item.Id, item.Engineer!.Id, item.Alias, item.Deliverables, item.Description,
           item.Remarks, item.CreatedAtDate, item.ScheduledDate, item.StartDate, item.CompleteDate,
            (DO.EngineerExperience?)item.Complexity, item.RequiredEffortTime);
        if (_dal.GetDates("StartDate") is not null)
            if (OldTask.EngineerId != NewdoTask.EngineerId || OldTask.CreatedAtDate != NewdoTask.CreatedAtDate
                || OldTask.scheduledDate != NewdoTask.scheduledDate || OldTask.StartDate != NewdoTask.StartDate
                || OldTask.CompleteDate != NewdoTask.CompleteDate || OldTask.Complexity != NewdoTask.Complexity 
                || OldTask.RequiredEffortTime != NewdoTask.RequiredEffortTime)
                throw new Exception("Cant not update thats fields after");
        _dal.Task.Update(NewdoTask);
    }











    public void UpdateDate(DateTime d, int id)
    {
        if (_dal.GetDates("StartDate") is not null)
            throw new Exception("error");
        DO.Task? dotask = _dal.Task.Read(id);
        if (dotask is null)
            throw new Exception("error");
        var Tasks = from DO.Dependency doDependency in _dal.Dependency.ReadAll(p => p.DependentTask == id)
                    let date = _dal.Task.Read(doDependency.DependsOnTask)!.scheduledDate
                    where date is not null
                    select doDependency;
        if (Tasks.Count() == 0)
            throw new Exception("error");
        if (Tasks.FirstOrDefault(p => forcaste(_dal.Task.Read(p.DependsOnTask)!) < d) is not null)
            throw new Exception("error");
        DO.Task? UpdateTask = dotask with { scheduledDate = d };

        try
        {
            _dal.Task.Update(UpdateTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={id} does not exists", ex);
        }

    }



    
    
    void AddDependency(int IdDepented, int IdDepentedOn)
    {
        if (_dal.GetDates("StartDate") is not null)
            throw new Exception("error");
        DO.Dependency doDependency = new DO.Dependency(0, IdDepented, IdDepentedOn);
        _dal.Dependency.Create(doDependency);
        BO.Task? botask = Read(IdDepentedOn);
        if(botask is null || Read(IdDepented) is null)
            throw new Exception("error");

        BO.TaskInList newTask = new BO.TaskInList() { Id = IdDepentedOn, Description = botask.Description,
            Alias = botask.Alias, Status = botask.Status };


        Read(IdDepented)!.Dependencies!.Add(newTask);

    }
    DateTime? forcaste(DO.Task T)
    {
        DateTime? max = T.StartDate >= T.scheduledDate ? T.StartDate : T.scheduledDate;

        max = max + T.RequiredEffortTime;
        return max;
    }
    BO.Status getstatus(DO.Task T)
    {
       
        int i = 0;
        if (T.StartDate is not null)
            i = 1;
        if (T.CompleteDate is not null)
            if (T.CompleteDate <= DateTime.Now)
                i = 3;
            else
                i = 2;

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
                
            default:
                return 0;
 
        }
    }
    //מתודת עזר בשביל חישוב האם כל המשימות התלוות במשימה זו הסתיימו
    bool EndOfTasks(DO.Task T)
    {
        if (_dal.Dependency.ReadAll(p => p.DependentTask == T.Id)
            .FirstOrDefault(k => getstatus(_dal.Task.Read(k.DependsOnTask)!) != BO.Status.Done) is not null)
            return false;
        return true;
    }
  






}