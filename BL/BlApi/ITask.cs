﻿

namespace BlApi;

public interface ITask
{
    public BO.Task? Read(int id);

    public IEnumerable<BO.TaskInList> ReadAll(Func<Task, bool>? filter = null);
    public int  Create(BO.Task item);
    public void Update(BO.Task item);
    public void Delete(int id);
    public void UpdateDate(DateTime d, int id);
    public void CreateSchedule();
    void AddDependency(int IdDepented, int IdDepentedOn);
}
