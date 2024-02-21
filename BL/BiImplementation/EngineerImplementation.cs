﻿
using BlApi;
using DalApi;

namespace BlImplementation;

internal class EngineerImplementation : BlApi.IEnginner
{
    private readonly IDal dal = DalApi.Factory.Get;
    public int Create(BO.Engineer engineer)
    {
        //TODO update Exception
        if (!IsValid(engineer))
            throw new Exception("the details of engineer is not valid");
        try
        {
            DO.Engineer eng = new DO.Engineer()
            {
                Id = engineer.Id,
                name = engineer.name,
                Email = engineer.Email,
                Cost = engineer.Cost,
                level = (DO.EngineerExperience)engineer.Level!,
            };

            int id = dal.Engineer.Create(eng);
            return id;
        }
        //TODO update Exception
        catch (Exception ex) { throw new Exception(ex.Message); }
    }

    public void Delete(int id)
    {
        bool isWorkOnTask = dal.Task.ReadAll(x => x.EngineerId == id && x.StartDate != null).Any();

        if (isWorkOnTask)
            throw new Exception("the engineer aorked on task");

        dal.Engineer.Delete(id);
    }

    public BO.Engineer Read(int id)
    {
        //TODO update Exception
        DO.Engineer engineerd = dal.Engineer.Read(id) ?? throw new Exception("the engineer id dost exist");
        
        DO.Task? task = dal.Task.Read(x => x.EngineerId == id &&
            x.StartDate != null && x.CompleteDate == null);

        return new BO.Engineer()
        {
            Id = engineerd.Id,
            name = engineerd.name,
            Cost = engineerd.Cost,
            Email = engineerd.Email,
            Level = (BO.EngineerExperience)engineerd.level!,
            Task = task is not null ? new BO.TaskInEngineer()
            {
                Alias = task.Alias,
                Id = task.Id,
            } : null
        };
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        return (from eng in dal.Engineer.ReadAll()
                let task = dal.Task.Read(x => x.EngineerId == eng.Id)
                select new BO.Engineer()
                {
                    name = eng.name,
                    Cost = eng.Cost,
                    Email = eng.Email,
                    Level = (BO.EngineerExperience)eng.level!,
                    Id = eng.Id,
                    Task = task is not null ? new BO.TaskInEngineer()
                    {
                        Alias = task.Alias,
                        Id = task.Id,
                    } : null
                }).Where(eng => filter is null ? true : filter(eng));
    }

    public void Update(BO.Engineer engineer)
    {
        if (!IsValid(engineer))
            throw new Exception("the details of engineer faild");
       try
        {
            //TODO
            if (engineer.Task != null)
            {
                DO.Task task = dal.Task.Read(engineer.Task!.Id) ?? new();
                if (task.EngineerId != engineer.Id && task.EngineerId != 0)
                    throw new Exception();//TODO
            }
               
    
        
        DO.Engineer eng = dal.Engineer.Read(engineer.Id) ??
            throw new BO.BlDoesNotExistException($"Engineer with ID={engineer.Id} does not exists"); 

                DO.EngineerExperience? newLevel = (DO.EngineerExperience)engineer.Level! > eng.level ?
                    (DO.EngineerExperience)engineer.Level : eng.level;
                eng = eng with
                {
                    Cost = engineer.Cost,
                    Email = engineer.Email,
                    level = newLevel,
                    name = engineer.name
                };

                dal.Engineer.Update(eng);

            
        }
        //TODO
        catch (Exception ex) { throw new Exception(ex.Message); }
    }

    private bool IsValid(BO.Engineer engineer)
    {
        return engineer.Email is null ? false :
            engineer.Cost <= 0.0 ? false :
            engineer.Id < 1 ? false :
            engineer.name is null ? false :
            engineer.Level is null ? false : true;
    }
}
