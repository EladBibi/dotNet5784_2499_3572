

namespace BlApi;

public interface ITask
{
    public BO.Task? Read(int id);
    public  void ScheduleTasks(DateTime startDate);
    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null);
    public IEnumerable<BO.TaskInGantt> GanttList(DateTime date);
    public int  Create(BO.Task item);
    public void Update(BO.Task item);
    public void Delete(int id);
    public void AddDependency(int IdDepented, int IdDepentedOn);
    public void DeleteDependency(int IdDepented, int IdDepentedOn);
    public void UpdateDate(DateTime? d, int id,string s);
  
    public BO.Status getstatus(DO.Task T);
    public bool Schedule_date();
    public void reset_schdule_date();
    public bool check_id_engineer(int id);
    public bool list_task_for_engineer(DO.Task do_task, int id);
    public void update_engineer_id(int eng, int task);
    public void finish_task(int id);
    public bool time_required();
    public void remove_engineer_from_task(int task_id);
    public bool finish_project();
}
