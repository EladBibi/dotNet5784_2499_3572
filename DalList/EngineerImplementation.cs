
namespace Dal;
using DalApi;
using DO;

internal class EngineerImplementation : IEngineer
{
     

    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Exists(x=> x.Id==item.Id))
            if (DataSource.Engineers.FirstOrDefault(s => s.Id == item.Id) is not null)
                throw new DalAlreadyExistsException("An object of type Engineer with such an ID already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        if (DataSource.Engineers.RemoveAll(x => x?.Id == id) == 0)
            throw new DalDoesNotExistException(@"Object of type ""Engineer"" with such ID does not exist");
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(s => s.Id == id);
       
    }
     public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(s =>filter(s));

    }
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {

        if (filter == null)
            return DataSource.Engineers.Select(item => item);
        else
            return DataSource.Engineers.Where(filter);
    }

    public void Update(Engineer item)
    {
        for (int i = 0; i < DataSource.Engineers.Count; ++i)

            if (DataSource.Engineers[i].Id == item.Id)
            {
                
                Delete(item.Id);
                DataSource.Engineers.Insert(i, item);
                return;

            }
        throw new DalDoesNotExistException(@"Object of type ""Engineer"" with such ID does not exist");
    }
}

