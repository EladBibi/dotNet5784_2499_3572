

namespace BlApi;

public  interface IBl
{
    public ITask Task { get; }
    public IEnginner Engineer { get; }
    public void InitializeDB();

     public void ResetDB();
    public DateTime Clock { get; }
    public void AddYear();
    public void AddMonth();
    public void AddDay();
    public void AddHour();
    public void InitializeTime();
    public int get_task_id();
    public DateTime? GetDate(string date);
    public void SetDate(DateTime d,string date);



}





