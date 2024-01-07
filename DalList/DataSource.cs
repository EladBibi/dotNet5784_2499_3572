namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        internal const int FirstTask = 0;
        private static int nextTask = FirstTask;
        internal static int NextTask { get => ++nextTask; }


        internal const int startDependency = 0;
        private static int nextDependency = startDependency;
        internal static int NextDependency { get => nextDependency++; }
        


    }


    internal static List<DO.Engineer?> Engineers { get; } = new();
    internal static List<DO.Task?> Tasks { get; } = new();
    internal static List<DO.Dependency?> Dependencies { get; } = new();
}
