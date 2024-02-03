


namespace BO;
/// <summary>
/// 
/// </summary>
public class TaskInList
{
    int Id { get; init; }
    string? Description { get; set; }
    string? Alias { get; set; }
    BO.Status Status { get; set; }
    //public override string ToString() => this.ToStringProperty();
}


