

namespace DO;


/// <summary>
/// Engineer Entity represents a engineer with all its props
/// </summary>
/// <param name="Id"> Unique ID number of the engineer
/// <param name="Name">Private Name of the engineer>
/// <param name="cost" <cost per hour/param>
/// <param name="EngineerExperience"</Engineer level>
/// <param name="Email"></email>









public record Engineer
(
    
           int Id,
       double Cost,
    string? name =null,
    string? Email = null,
   DO.EngineerExperience? level = null

)
{
    public Engineer() : this(0, 0) { }
}

      
 



    






