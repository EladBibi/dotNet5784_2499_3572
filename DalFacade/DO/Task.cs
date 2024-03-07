



namespace DO;



/// Here the characteristics of the task to be performed are defined as follows:
/// <param name="Id"></Id of the task> 
/// <param name="EngineerId"></Identity card of the engineer responsible for the task
/// <param name="Alias"></A short, unique name>
/// <param name="Description"></Description of the task>
/// <param name="Deliverables"></deliverables - a string describing the results or items provided at the end of the task.>
/// <param name="Remarks"></remarks>
/// <param name="IsMilestone"></milestone of the task>
/// <param name="CreatedAtDate"></indicates the time when the task was created by the administrator>
/// <param name="StartDate"></Start date>
/// <param name="scheduledDate"></Planned date for the start of work>
/// <param name="DeadLineDate"></Possible final end date>
///  <param name="CompleteDate" <>Actual work completion date
/// <param name="Complexity"></The difficulty level of the task - defines the minimum engineer level that can work on it.>
/// <param name="RequiredEffortTime"></The amount of time required to perform the task>

public record Task
    (
    int Id,//מספר מזהה ייחודי- מספר רץ אוטומטי
    int EngineerId,
    string? Alias = null,
    string? Description = null,
    string? Deliverables = null,
    string? Remarks = null,
    // bool IsMilestone=false,
    DateTime? CreatedAtDate = null,
    DateTime? scheduledDate = null,
    DateTime? StartDate = null,
    DateTime? CompleteDate = null,

    DO.EngineerExperience? Complexity = null,
    TimeSpan? RequiredEffortTime = null
    )
{
    public Task() : this(0, 0) { }

}











