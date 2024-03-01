
using BlApi;
using BO;
using DalApi;

namespace BlImplementation;

internal class EngineerImplementation : BlApi.IEnginner
{
    private readonly IDal dal = DalApi.Factory.Get;
    public int Create(BO.Engineer engineer)
    {
        //TODO update Exception
        if (!IsValid(engineer))
            throw new InvalidDataException("the details of engineer is not valid");
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

            return dal.Engineer.Create(eng);
        }
        //TODO update Exception
        catch (Exception ex) { throw new BlAlreadyExistsException(ex.Message); }
    }

    public void Delete(int id)
    {
        try
        {
            bool isWorkOnTask = dal.Task.ReadAll(x => x.EngineerId == id && x.StartDate != null).Any();

            if (isWorkOnTask)
                throw new InvalidOperationException("the engineer aorked on task");

            dal.Engineer.Delete(id);
        }
        catch (Exception ex) { throw new BlDoesNotExistException(ex.Message); }
    }

    public BO.Engineer Read(int id)
    {
        DO.Engineer engineerd = dal.Engineer.Read(id) ?? throw new BlDoesNotExistException("the engineer id dost exist");

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
                    throw new InvalidDataException("the worker task on other engineer");//TODO
            }

            DO.Engineer eng = dal.Engineer.Read(engineer.Id) ??
                throw new BO.BlDoesNotExistException($"Engineer with ID={engineer.Id} does not exists");

            DO.EngineerExperience? newLevel = (DO.EngineerExperience)engineer.Level! > eng.level ?
                (DO.EngineerExperience)engineer.Level : eng.level;

            eng = eng with
            {
                Cost = engineer.Cost,
                Email = engineer.Email,
                level = newLevel,//ערך התקין לוגית אותו וידאנו לעיל
                name = engineer.name
            };

            dal.Engineer.Update(eng);
        }
        catch (Exception ex) { throw new Exception(ex.Message); }
    }

    private bool IsValid(BO.Engineer engineer)
    {
        return string.IsNullOrEmpty(engineer.Email) ? false :
            engineer.Cost <= 0.0 ? false :
            engineer.Id < 1 ? false :
            string.IsNullOrEmpty(engineer.name) ? false :
            engineer.Level is null ? false : true;
    }
}
