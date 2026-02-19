namespace SatelliteProcessing.Application.Abstractions;

/// <summary>
/// Defines an abstract queue for job distribution to processing workers.
/// </summary>
public interface IJobQueue
{
    /// <summary>
    /// Enqueues a job identifier for processing.
    /// </summary>
    /// <param name="jobId">The identifier of the job to enqueue.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task EnqueueAsync(Guid jobId, CancellationToken cancellationToken);

    /// <summary>
    /// Dequeues a job identifier for processing, waiting until one is available.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The next job identifier.</returns>
    Task<Guid> DequeueAsync(CancellationToken cancellationToken);
}
