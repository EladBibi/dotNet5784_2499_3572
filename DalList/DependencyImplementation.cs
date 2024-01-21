

namespace Dal;
using DalApi;
using DO;


internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int NewId = DataSource.Config.NextDependency;
        Dependency NewDependency = item with { Id = NewId };
        DataSource.Dependencies.Add(NewDependency);
        return NewId;
    }

    public void Delete(int id)
    {
        if (DataSource.Dependencies.RemoveAll(x => x?.Id == id) == 0)
            throw new DalDoesNotExistException(@"Object of type ""Dependency"" with such ID does not exist");
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(s => s.Id == id);
        
    }
   public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(s =>filter(s));
    }
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {

        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);
    }

    public void Update(Dependency item)
    {
        for (int i = 0; i < DataSource.Tasks.Count; ++i)

            if (DataSource.Dependencies[i].Id == item.Id)
            {
                Delete(item.Id);
                DataSource.Dependencies.Insert(i, item);
                return;
                

            }
        throw new DalDoesNotExistException(@"Object of type ""Dependency"" with such ID does not exist");
    }
   public void DeleteAll()
    {
        DataSource.Dependencies.Clear();

    }
}

