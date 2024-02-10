

namespace DO;



/// <summary>
/// ID number of the task and dependencies between tasks
/// </summary>
/// <param name="Id"></Identification number of the task>
/// <param name="DependentTask"></id of dependent task>
/// <param name="DependsOnTask">ID number of a task that depends on this task>
public record Dependency(
   int Id, 
   int DependentTask,
   int DependsOnTask
  )
{
    public Dependency() : this(0, 0, 0) { }
}




