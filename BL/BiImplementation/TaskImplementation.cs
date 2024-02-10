

//namespace BlImplementation;
//using BlApi;
//using BO;
//using DO;
//using System.Diagnostics.CodeAnalysis;
//using System.Runtime.InteropServices;
//using System.Security.Cryptography;
//using System.Xml.Linq;

//internal class TaskImplementation : ITask
//{

//    private DalApi.IDal _dal = DalApi.Factory.Get;

//    public int Create(BO.Task item)
//    {
//        DateTime? temp = _dal.GetDates("StartDate");
//        if (temp is not null)
//            throw new BlLogicalErrorException("It is not possible to create new tasks after entering a start date for the project");

//        if (CheckData(item))
//            throw new BlInvalidInputException("The data you entered is incorrect for the task");
//        int EngineerId = 0;
//        if (item.Engineer is not null)
//            EngineerId = item.Engineer.Id;



//        DO.Task doTask = new DO.Task(item.Id, EngineerId, item.Alias, item.Deliverables, item.Description,
//            item.Remarks, item.CreatedAtDate, item.ScheduledDate, item.StartDate, item.CompleteDate,
//             (DO.EngineerExperience?)item.Complexity, item.RequiredEffortTime);
//        int idTask;
//        try
//        {
//            idTask = _dal.Task.Create(doTask);
//        }

//        catch (DO.DalAlreadyExistsException ex)
//        {
//            throw new BO.BlAlreadyExistsException($"Student with ID={item.Id} already exists", ex);
//        }


//        var result = from Object in item.Dependencies
//                     select new DO.Dependency(0, item.Id, Object.Id);
//        foreach (var Temp in result)
//            _dal.Dependency.Create(Temp);
//        return idTask;
//    }



//    public void CreateSchedule()
//    {
//        DateTime date;


//        _dal.SetDates(DateTime.Now, "StartDate");
//        foreach (var temp in _dal.Task.ReadAll())
//        {

//            string s = Console.ReadLine()!;
//            if (!DateTime.TryParse(s, out date))
//                throw new BlInvalidInputException("The data you entered is incorrect for a date");
//            if (!_dal.Dependency.ReadAll(p => p.DependentTask == temp.Id).Any())
//            {
//                if (_dal.GetDates("StartDate") > date)
//                    throw new BlLogicalErrorException("The start date cannot be later than the project start date");
//                DO.Task dotask = temp with { scheduledDate = date };
//                _dal.Task.Update(dotask);
//            }
//            else
//            {
//                try
//                {
//                    UpdateDate(date, temp.Id);
//                }
//                catch (BlLogicalErrorException ex)
//                {
//                    throw;
//                }

//            }
//        }



//    }

//    public void Delete(int id)
//    {
//        DateTime? temp = _dal.GetDates("StartDate");
//        if (temp is not null)
//            throw new BlLogicalErrorException("It is not possible to delete  tasks after entering a start date for the project");



//        if (_dal.Dependency.ReadAll(K => K.DependsOnTask == id).Any())
//            throw new BlNullPropertyException("It is not possible  to delete a task that depends on other tasks");
//        try
//        {
//            _dal.Task.Delete(id);
//        }
//        catch (DO.DalDoesNotExistException ex)
//        {
//            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exists", ex);
//        }
//    }





//    public BO.Task? Read(int id)
//    {
//        DO.Task? doTask = _dal.Task.Read(id);
//        if (doTask == null)
//            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
//        return new BO.Task()
//        {
//            Id = id,
//            Alias = doTask.Alias,
//            Description = doTask.Description,
//            Deliverables = doTask.Deliverables,
//            Remarks = doTask.Remarks,
//            CreatedAtDate = doTask.CreatedAtDate,
//            ScheduledDate = doTask.scheduledDate,
//            StartDate = doTask.StartDate,
//            CompleteDate = doTask.CompleteDate,
//            ForeCastDate = forcaste(doTask),
//            Complexity = (BO.EngineerExperience?)doTask.Complexity,
//            RequiredEffortTime = doTask.RequiredEffortTime,
//            Status = getstatus(doTask),
//            Dependencies = (from item in _dal.Dependency.ReadAll(t => t.DependentTask == id)
//                            select new TaskInList()
//                            {
//                                Id = item.DependsOnTask,
//                                Description = _dal.Task.Read(item.DependsOnTask)!.Description,
//                                Alias = _dal.Task.Read(item.DependsOnTask)!.Alias,
//                                Status = 0
//                            }).ToList(),
//            Engineer = new EngineerInTask() { Id = doTask.EngineerId, Name = _dal.Engineer.Read(doTask.EngineerId)!.name }
//        };
//    }














//    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null)
//    {



//        return (filter is not null) ?
//            (from DO.Task doTask in _dal.Task.ReadAll(p => filter(p))
//                 //(int?)p.Complexity <= (int?)(DO.EngineerExperience)level && p.EngineerId == 0
//                 //&& EndOfTasks(p) ||)
//             select new BO.TaskInList
//             {
//                 Id = doTask.Id,
//                 Description = doTask.Description,
//                 Alias = doTask.Alias,
//                 Status = getstatus(doTask)
//             }) :
//                 (from DO.Task doTask in _dal.Task.ReadAll()

//                  select new BO.TaskInList
//                  {
//                      Id = doTask.Id,
//                      Description = doTask.Description,
//                      Alias = doTask.Alias,
//                      Status = getstatus(doTask)
//                  });

//    }





//    public void Update(BO.Task item)
//    {
//        if (CheckData(item))
//            throw new BlInvalidInputException("The data you entered is incorrect for the update task");

//        DO.Task OldTask = _dal.Task.Read(item.Id) ?? throw
//        new BO.BlAlreadyExistsException($"Task with ID={item.Id} does not exists");




//        DO.Task NewdoTask = new DO.Task(item.Id, item.Engineer!.Id, item.Alias, item.Deliverables, item.Description,
//           item.Remarks, item.CreatedAtDate, item.ScheduledDate, item.StartDate, item.CompleteDate,
//            (DO.EngineerExperience?)item.Complexity, item.RequiredEffortTime);
//        if (_dal.GetDates("StartDate") is not null)
//            if (OldTask.EngineerId != NewdoTask.EngineerId || OldTask.CreatedAtDate != NewdoTask.CreatedAtDate
//                || OldTask.scheduledDate != NewdoTask.scheduledDate || OldTask.StartDate != NewdoTask.StartDate
//                || OldTask.CompleteDate != NewdoTask.CompleteDate || OldTask.Complexity != NewdoTask.Complexity
//                || OldTask.RequiredEffortTime != NewdoTask.RequiredEffortTime)
//                throw new BlLogicalErrorException("Cant not update thats fields after you enter a start date for the project");
//        _dal.Task.Update(NewdoTask);
//    }
//    bool CheckData(BO.Task item)
//    {
//        return (item.Id <= 0 || item.Alias is null || item.Alias == "" || item.Engineer!.Id <= 0 || item.Engineer.Name == "" ||
//            item.Engineer.Name is null || item.ScheduledDate < DateTime.Now || item.StartDate < DateTime.Now ||
//            item.CompleteDate < DateTime.Now);
//    }










//    public void UpdateDate(DateTime d, int id)
//    {
//        if (_dal.GetDates("StartDate") is null)
//            throw new BlLogicalErrorException("Cant not update dates before  you enter a start date for the project");
//        DO.Task? dotask = _dal.Task.Read(id);
//        if (dotask is null)
//            throw new BlNullPropertyException($"Task with ID={id} does Not exist");
//        var Tasks = from DO.Dependency doDependency in _dal.Dependency.ReadAll(p => p.DependentTask == id)
//                    let date = _dal.Task.Read(doDependency.DependsOnTask)!.scheduledDate
//                    where date is not null
//                    select doDependency;
//        if (Tasks.Count() == 0)
//            throw new BlLogicalErrorException("The dates of the tasks on which the task depends are not yet updated");
//        if (Tasks.FirstOrDefault(p => forcaste(_dal.Task.Read(p.DependsOnTask)!) < d) is not null)
//            throw new BlLogicalErrorException("It is not possible to update a date for" +
//                "a task that is earlier than the date of a task on which it depends");
//        DO.Task? UpdateTask = dotask with { scheduledDate = d };



//        _dal.Task.Update(UpdateTask);

//    }




//    public void AddDependency(int IdDepented, int IdDepentedOn)
//    {
//        DateTime? temp = _dal.GetDates("StartDate");
//        if (temp is not null)
//            throw new BlLogicalErrorException("Dependencies cannot be added after the execution phase has started");
//        DO.Dependency doDependency = new DO.Dependency(0, IdDepented, IdDepentedOn);
//        _dal.Dependency.Create(doDependency);
//        BO.Task? botask = Read(IdDepentedOn);
//        if (botask is null || Read(IdDepented) is null)
//            throw new BlNullPropertyException($"Task with ID={IdDepented} does Not exist");

//        BO.TaskInList newTask = new BO.TaskInList()
//        {
//            Id = IdDepentedOn,
//            Description = botask.Description,
//            Alias = botask.Alias,
//            Status = botask.Status
//        };


//        Read(IdDepented)!.Dependencies!.Add(newTask);
//    }



    void UpdateEngineer(int id, EngineerInTask engineer)
    {
        DateTime? temp = _dal.GetDates("StartDate");
        if (temp is null)
            throw new BlLogicalErrorException("It is not possible to assign an engineer before the start of the execution phase");


        DO.Task? dotask = _dal.Task.Read(id);
        DO.Engineer? doengineer = _dal.Engineer.Read(engineer.Id);
        if (engineer.Name is null || engineer.Name == "" || engineer.Id <= 0 || id <= 0 || dotask is null || doengineer is null)
            throw new BlInvalidInputException("The data entered does not match");


        if ((int?)dotask.Complexity > (int?)doengineer.level)
            throw new BlLogicalErrorException("The task level is not suitable for an engineer");
        //מאחר והמנהדס עובר למשימה חדשה צמריך לבדוק אם הייתה לו משימה קודמת ולעדכן בהתאם לכך שזמן הסיום יהיה כעת ואז הסטטוס יהיה done 
        
        var item = _dal.Task.ReadAll(p=>p.EngineerId == engineer.Id);
        if (item.Any())
            foreach (var item2 in item)
                if (item2.CompleteDate > DateTime.Now)
                {
                    DO.Task NewTask = item2 with { CompleteDate = DateTime.Now };
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





    
