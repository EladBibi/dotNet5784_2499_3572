

namespace DalApi;
using DO;
public interface ICrud<T> where T : class
{
    int Create(T item); //Creates new entity object in DAL
    T? Read(int id); //Reads entity object by its ID 
     T? Read(Func<T, bool> filter);//The function returns  a member that meet a certain condition.

    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null);//The function returns all members that meet a certain condition,
                                                          //if there is no condition the original list will be returned
    void Update(T item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}


