

namespace BlImplementation;
using BlApi;
using BO;

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
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        DO.Task? DeleteTask = _dal.Task.Read(s => s.Id == id);
        if (DeleteTask is null || _dal.Task.ReadAll(K => K.Id != id).FirstOrDefault(s => s.StartDate > DeleteTask.StartDate) is not null)
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



                          










    public IEnumerable<BO.TaskInList> ReadAll(Func<System.Threading.Tasks.Task, bool>? filter = null)
    {
        

    }

    public void Update(BO.Task item)
    {
        throw new NotImplementedException();
    }

    public void UpdateDate(DateTime d, int id)
    {
        throw new NotImplementedException();
    }
    void AddDependency(int IdDepented, int IdDepentedOn) {



    }
    DateTime? forcaste(DO.Task T)
    {
        DateTime? max = T.StartDate >= T.scheduledDate ? T.StartDate : T.scheduledDate;

        max = max + T.RequiredEffortTime;
        return max;
    }
    BO.Status getstatus(DO.Task T)
    {
        return 0;
    }

}