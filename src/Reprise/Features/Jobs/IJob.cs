namespace Reprise
{
    /// <summary>
    /// Specifies the contract for running jobs.
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// Runs the job.
        /// </summary>
        Task Run(CancellationToken cancellationToken);
    }
}
