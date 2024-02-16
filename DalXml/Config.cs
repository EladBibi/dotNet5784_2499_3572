

namespace Dal;

/// <summary>
/// מכיוון שהנתונים נשמרים בקובץ ומתעדכנים בקובץ, אזי היתרון הוא שהם ישמרו מריצה לריצה. למשל: המספר הרץ האחרון מהריצה הקודמת ישאר ובריצה הבאה נמשיך להתקדם ממנו

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
}
