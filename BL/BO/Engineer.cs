

using DO;

namespace BO;
/// <summary>
/// 
/// </summary>
public class Engineer
{
    public int Id { get; init; }
    public double Cost{ get; set; }
    public string? name { get; set; }
    public string? Email { get; set; }
    public EngineerExperience? Level { get; set; }
    public TaskInEngineer? Task { get; set; } = null;
    //public override string ToString() => this.ToStringProperty();

}
