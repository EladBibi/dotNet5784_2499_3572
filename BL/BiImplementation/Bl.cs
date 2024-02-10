

namespace BlImplementation;
using BlApi;

internal class Bl : IBl
{
    
    public ITask Task => new TaskImplementation();

    public IEnginner Engineer =>  new EngineerImplementation();
}
