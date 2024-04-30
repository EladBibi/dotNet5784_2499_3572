

namespace BlImplementation;
using BlApi;
using Dal;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Bl : IBl
{

    public ITask Task => new TaskImplementation(this);

    public IEnginner Engineer => new EngineerImplementation(this);

    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => DalTest.Initialization.Reset();
    

    private static DateTime s_Clock = DalTest.Initialization.Get_Sistem_Date();
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

    

    public void AddYear() => Clock = Clock.AddYears(1);

    public void AddMonth() => Clock = Clock.AddMonths(1);

    public void AddDay() => Clock = Clock.AddDays(1);

    public void AddHour() => Clock = Clock.AddHours(1);
   
    public void AddSecond() => Clock = Clock.AddSeconds(1);

    public void InitializeTime() => Clock = DateTime.Now.Date;
    public int get_task_id()
    {
        return DalTest.Initialization.Get_Task_Id();
    }
    public void Set_Sistem_Date() => DalTest.Initialization.Set_Sistem_Date();
    public DateTime Get_Sistem_Date() => DalTest.Initialization.Get_Sistem_Date();

    public DateTime? GetDate(string d) => DalTest.Initialization.GetDates(d);
    public void SetDate(DateTime d, string date) => DalTest.Initialization.SetDates(d,date);

    public string? getDataBase() => DalTest.Initialization.GetDataBase();




}


