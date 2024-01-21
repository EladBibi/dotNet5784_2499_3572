
namespace Dal;
using DalApi;
using DO;





internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int NewId = DataSource.Config.NextTask;
        Task NewTask = item with { Id = NewId };
        DataSource.Tasks.Add(NewTask);
        return NewId;

    }




    public void Delete(int id)
    {
        if (DataSource.Tasks.RemoveAll(x => x?.Id == id) == 0)
            throw new DalDoesNotExistException(@"Object of type ""Task"" with such ID does not exist");

    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(s => s.Id == id);
       
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {

        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);
    }
   public Task? Read(Func<Task, bool> filter)
    {

        return DataSource.Tasks.FirstOrDefault(s =>filter(s));
    }
    public void Update(Task item)
    {
        for (int i = 0; i < DataSource.Tasks.Count; ++i)

            if (DataSource.Tasks[i].Id == item.Id)
            {
                Delete(item.Id);
                DataSource.Tasks.Insert(i, item);
                return;

            }
        throw new DalDoesNotExistException(@"Object of type ""Task"" with such ID does not exist");
    }

    public void DeleteAll()
    {
        DataSource.Tasks.Clear();

    }
}
    
