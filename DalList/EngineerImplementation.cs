
namespace Dal;
using DalApi;
using DO;

    public class EngineerImplementation : IEngineer
    {
        public int Create(Engineer item)
        {
            if (DataSource.Engineers.Exists(x => x.Id == item.Id))
                throw new Exception("An object of type Engineer with such an ID already exists");
            DataSource.Engineers.Add(item);
            return item.Id;
        }

        public void Delete(int id)
        {
            if (DataSource.Engineers.RemoveAll(x => x?.Id == id) == 0)
                throw new Exception(@"Object of type ""Engineer"" with such ID does not exist");
        }

        public Engineer? Read(int id)
        {
            Engineer? temp = DataSource.Engineers.Find(x => x?.Id == id);
            return temp;
        }

        public List<Engineer> ReadAll()
        {
            return new List<Engineer>(DataSource.Engineers);
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
            throw new Exception(@"Object of type ""Engineer"" with such ID does not exist");
        }
    }


