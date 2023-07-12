namespace Reprise
{
    /// <summary>
    /// Identifies a job that runs before the server is started.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RunBeforeStartAttribute : Attribute
    {
    }

    /// <summary>
    /// Identifies a job that runs after the server is started.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RunOnStartAttribute : Attribute
    {
    }

    /// <summary>
    /// Identifies a job that runs on a schedule.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CronAttribute : Attribute
    {
        internal string Expression { get; }

        /// <summary>
        /// Creates a new <see cref="CronAttribute"/>.
        /// </summary>
        public CronAttribute(string expression)
        {
            Expression = expression;
        }
    }
}
