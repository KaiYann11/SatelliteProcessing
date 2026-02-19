using SatelliteProcessing.Application.Models;
using SatelliteProcessing.Domain;

namespace SatelliteProcessing.Application.Services;

/// <summary>
/// Defines application use cases for creating and querying jobs.
/// </summary>
public interface IJobService
{
    /// <summary>
    /// Creates a new job, persists it, and queues it for processing.
    /// </summary>
    /// <param name="command">Input data required to create the job.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The newly created job.</returns>
    Task<ProcessingJob> CreateJobAsync(CreateJobCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all jobs for monitoring.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of all jobs.</returns>
    Task<IReadOnlyList<ProcessingJob>> ListJobsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a single job by identifier.
    /// </summary>
    /// <param name="jobId">The job identifier to retrieve.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The job if found; otherwise null.</returns>
    Task<ProcessingJob?> GetJobAsync(Guid jobId, CancellationToken cancellationToken);
}
