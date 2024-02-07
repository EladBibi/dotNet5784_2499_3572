

namespace BlImplementation;
using BlApi;

internal class Bl : IBl
{
    
    public ITask Task => new TaskImplementation();

    public IEnginner Enginner =>  new EngineerImplementation();
}
