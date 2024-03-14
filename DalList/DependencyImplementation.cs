

namespace Dal;
using DalApi;
using DO;
using static DataSource;


internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = Config.NextDependency;
        item = item with { Id = id };  //כיוון שהישות היא מסוג רשומה , איך לנו את היכולת לשנותא תה שדה
        Dependencies.Add(item); 
        return id;  
    }

    public void Delete(int id)
    {
        
        //למצוא בתוך המערך את האובייקט על פי המזהה שנתנו לנו ולמחוק אותו
        if (Dependencies.RemoveAll(x => x.DependentTask == id) == 0)
            throw new DalDoesNotExistException(@"Object of type ""Dependency"" with such ID does not exist");
    }

    public Dependency? Read(int id)
    {
        Read(x => x.Id == id);
        return Dependencies.FirstOrDefault(s => s.Id == id);
        
    }
   public Dependency? Read(Func<Dependency, bool> filter)
    {
        return Dependencies.FirstOrDefault(s =>filter(s));
    }

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {

        if (filter == null)
            return Dependencies.Select(item => item);
        else
            return Dependencies.Where(filter);
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

