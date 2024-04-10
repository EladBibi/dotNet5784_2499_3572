

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
    public void UpdateDate(DateTime? d, int id,string s);
    public void CreateSchedule(DateTime date);
    public BO.Status getstatus(DO.Task T);
    public bool Schedule_date();
    public void reset_schdule_date();
    public bool check_id_engineer(int id);
    public bool list_task_for_engineer(DO.Task do_task, int id);
    public void update_engineer_id(int eng, int task);
    public void finish_task(int id);
    public bool time_required();
}
