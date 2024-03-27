

using System.Xml.Linq;

namespace DalApi;

public interface IDal
{
    IEngineer Engineer { get; }
    IDependency Dependency { get; }
    ITask Task { get; }
   
    


}
