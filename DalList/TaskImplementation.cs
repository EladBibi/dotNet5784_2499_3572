
namespace Dal;
using DalApi;
using DO;





public class TaskImplementation : ITask
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
        if (DataSource.Tasks.RemoveAll(x=>x?.Id==id)==0)
            throw new Exception(@"Object of type ""Task"" with such ID does not exist");

    }

    public Task? Read(int id)
    {
        Task? temp = DataSource.Tasks.Find(x => x?.Id == id);
        return temp;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
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
        throw new Exception(@"Object of type ""Task"" with such ID does not exist");
    }


}
    
