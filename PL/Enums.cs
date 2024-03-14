
using System.Collections;
namespace PL;

internal class LevelsCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums =
(Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}


internal class LevelsCollection_List : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience_List> s_enums =
(Enum.GetValues(typeof(BO.EngineerExperience_List)) as IEnumerable<BO.EngineerExperience_List>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

//public enum Status

internal class StatusCollection : IEnumerable
{
    static readonly IEnumerable<BO.Status> s_enums =
(Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();

}

