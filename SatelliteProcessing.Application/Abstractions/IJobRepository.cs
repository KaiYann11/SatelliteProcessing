using SatelliteProcessing.Domain;

// 파일: SatelliteProcessing.Application/Abstractions/IJobRepository.cs
// 설명: 작업 저장소에 대한 추상 인터페이스입니다. 영속화 및 조회 기능을 제공합니다.

namespace SatelliteProcessing.Application.Abstractions;

/// <summary>
/// Defines persistence operations for processing jobs.
/// </summary>
public interface IJobRepository
{
    /// <summary>
    /// Adds a new job to storage.
    /// </summary>
    /// <param name="job">The job to persist.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task AddAsync(ProcessingJob job, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing job in storage.
    /// </summary>
    /// <param name="job">The job to update.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UpdateAsync(ProcessingJob job, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a job by its identifier.
    /// </summary>
    /// <param name="jobId">The job identifier to look up.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The job if found; otherwise null.</returns>
    Task<ProcessingJob?> GetAsync(Guid jobId, CancellationToken cancellationToken);

    /// <summary>
    /// Lists all jobs currently stored.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of jobs.</returns>
    Task<IReadOnlyList<ProcessingJob>> ListAsync(CancellationToken cancellationToken);
}
