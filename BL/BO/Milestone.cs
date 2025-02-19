﻿
namespace BO;
/// <summary>
/// 
/// </summary>
public class Milestone
{


    public int Id { get; init; }

    public string? Alias { get; set; }
    public string? Description { get; set; }
  
    public string? Remarks { get; set; }

    public DateTime? CreatedAtDate { get; init; }

    
    public DateTime? CompleteDate { get; set; }
    public DateTime? DeadLineDate { get; set; }
    public DateTime? ForeCastDate { get; init; }
   
   
    public BO.Status? Status { get; set; }
    public List<BO.TaskInList>? Dependencies { get; init; } = null;
   public double CompletionPercentage { get; set; }
    //public override string ToString() => this.ToStringProperty();

}
