﻿
namespace Dal;
using DalApi;
using DO;




public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        Task temp = new Task(DataSource.Config.NextTask,
        DataSource.Tasks.Add(item);


    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public List<Task> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Task item)
    {
        throw new NotImplementedException();
    }
}
