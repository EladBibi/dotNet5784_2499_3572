

namespace BO;
/// <summary>
/// 
/// </summary>
public class Task
{
  /* public Task(int id, string? alias, string? description, string? deliverables,
            string? remarks, DateTime? createdAtDate, DateTime? scheduledDate, DateTime? startDate, DateTime? completeDate,
           DateTime? foreCastDate, BO.EngineerExperience? complexity, TimeSpan? requiredEffortTime, BO.Status? status,
           List<BO.TaskInList>? dependencies, BO.MilestoneInTask? milestone, BO.EngineerInTask? engineer)
    {
        Id = id;
        Alias = alias;
        Description = description;
        Deliverables = deliverables;
        Remarks = remarks;
        CreatedAtDate = createdAtDate;
        ScheduledDate = scheduledDate;
        StartDate = startDate;
        CompleteDate = completeDate;
        ForeCastDate = foreCastDate;
        Complexity = complexity;
        RequiredEffortTime = requiredEffortTime;
        Status = status;
        Dependencies = dependencies;
        Milestone = milestone;
        Engineer = engineer;
    }
  */






    public int Id { get; init; }

    public string? Alias { get; set; }
    public string? Description { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }

    public DateTime? CreatedAtDate { get; init; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompleteDate { get; set; }

    public DateTime? ForeCastDate { get; init; }
    public BO.EngineerExperience? Complexity { get; set; }
    public TimeSpan? RequiredEffortTime { get; init; }
    public BO.Status? Status { get; set; }
    public List<BO.TaskInList>? Dependencies { get; init; } = null;
    //public BO.MilestoneInTask? Milestone { get; init; } = null;
    public BO.EngineerInTask? Engineer { get; set; } = null;
    //public override string ToString() => this.ToStringProperty();
}
