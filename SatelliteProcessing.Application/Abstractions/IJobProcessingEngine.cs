namespace SatelliteProcessing.Application.Abstractions;

/// <summary>
/// Defines the orchestration service that runs a job through the pipeline.
/// </summary>
public interface IJobProcessingEngine
{
    /// <summary>
    /// Executes the full pipeline for a single job.
    /// </summary>
    /// <param name="jobId">Identifier of the job to process.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task ProcessJobAsync(Guid jobId, CancellationToken cancellationToken);
}
