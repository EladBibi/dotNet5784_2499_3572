

namespace BlApi;

public interface ITask
{
    public BO.Task? Read(int id);

    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null);
    public int  Create(BO.Task item);
    public void Update(BO.Task item);
    public void Delete(int id);
    public void AddDependency(int IdDepented, int IdDepentedOn);
    public void DeleteDependency(int IdDepented, int IdDepentedOn);
    public void UpdateDate(DateTime d, int id);
    public void CreateSchedule(DateTime date);
    public BO.Status getstatus(DO.Task T);
    public bool Schedule_date();
  
}
