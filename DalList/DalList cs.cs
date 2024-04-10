
namespace Dal;
using DalApi;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

//sealed - מחלקה שלא ניתן לרשת ממנה
sealed internal class DalList : IDal
{
    

    //מה שמתבצע פה זה אתחול עיקבי שמוודא יעילות בכך שהאתחול מתבצע רק
    //כאשר באמת צריכים אותו ולא באופן אוטומטי עד שהוא באמת נדרש. אתחול בטוח
    //אתחול בטוח מהווה הגנה על האתחול במקרה שישנם תהליכים מרובים בו זמנית אז בכך מוודאים שלא
    //יוכלו לבקש את התוכן בו זמנית דבר שעלול לגרום לבעיות
    private static Lazy<IDal> instance=new Lazy<IDal>(()=>new DalList(),LazyThreadSafetyMode.ExecutionAndPublication);
    public static IDal Instance { get; } = instance.Value;

    // public static IDal Instance { get; } = new DalList();
    private DalList() { }
    // IDal עבור כל אחד מתתי הממשקים נממש את התכונה המתאימה שהוגדרה עבורו בממשק
    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public ITask Task => new TaskImplementation();    
}
