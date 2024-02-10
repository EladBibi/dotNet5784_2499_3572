

using System.Xml.Linq;

namespace DalApi;

public interface IDal
{
    IEngineer Engineer { get; }
    IDependency Dependency { get; }
    ITask Task { get; }
    DateTime? GetDates(string date);
    public void SetDates(DateTime d, string date);



}
