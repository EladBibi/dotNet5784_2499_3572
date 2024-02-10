

namespace Dal;
using DalApi;
using System.Diagnostics;
using System.Xml.Linq;

sealed public class DalXml : IDal
{
   
    //מה שמתבצע פה זה אתחול עיקבי שמוודא יעילות בכך שהאתחול מתבצע רק כאשר באמת צריכים אותו ולא באופן אוטומטי עד שהוא באמת נדרש. אתחול בטוח
    //אתחול בטוח מהווה הגנה על האתחול במקרה שישנם תהליכים מרובים בו זמנית אז בכך מוודאים שלא יוכלו לבקש את התוכן בו זמנית דבר שעלול לגרום לבעיות
    private static Lazy<IDal> instance = new Lazy<IDal>(() => new DalXml(), LazyThreadSafetyMode.ExecutionAndPublication);
    public static IDal Instance { get; } = instance.Value;
    private DalXml() { }
    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public ITask Task => new TaskImplementation();
    public DateTime? GetDates(string date)
    {
        XElement root = XElement.Load(@"..\xml\data-config.xml");
        return root.ToDateTimeNullable(date);

    }
    public void SetDates(DateTime d, string date)
    {
        d.AddMonths(2);
        XElement root = XElement.Load(@"..\xml\data-config.xml");
        root.Element(date)?.SetValue((d).ToString());
        root.Save(@"..\xml\data-config.xml");
    }
}
