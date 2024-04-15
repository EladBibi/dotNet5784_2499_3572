using BO;

namespace BlApi;

public  interface IEnginner
{
    int Create(Engineer engineer);
    Engineer Read(int id);
    void Update(Engineer engineer);
    void Delete(int id);
    IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null);
    public IEnumerable<BO.EngineerInTask?> Read_Engineer_In_Task();
}
