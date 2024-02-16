

namespace BlApi;

public  interface IBl
{
    public ITask Task { get; }
    public IEnginner Engineer { get; }
    public void InitializeDB();
    
    public void ResetDB();

}





