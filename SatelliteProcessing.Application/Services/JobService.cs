using SatelliteProcessing.Application.Abstractions;
using SatelliteProcessing.Application.Models;
using SatelliteProcessing.Domain;

// 파일: SatelliteProcessing.Application/Services/JobService.cs
// 설명: 작업 생성 및 조회 등 애플리케이션 수준의 비즈니스 로직을 제공하는 서비스 구현입니다.

namespace SatelliteProcessing.Application.Services;

/// <summary>
/// Implements job creation and query use cases with persistence and queue integration.
/// </summary>
public sealed class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly IJobQueue _jobQueue;
    private readonly IJobEventRepository _jobEventRepository;
    private readonly ITimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the service with required dependencies.
    /// </summary>
    /// <param name="jobRepository">Repository used to persist jobs.</param>
    /// <param name="jobQueue">Queue used to distribute jobs to workers.</param>
    /// <param name="jobEventRepository">Outbox used for real-time monitoring events.</param>
    /// <param name="timeProvider">Time provider for consistent timestamps.</param>
    public JobService(
        IJobRepository jobRepository,
        IJobQueue jobQueue,
        IJobEventRepository jobEventRepository,
        ITimeProvider timeProvider)
    {
        _jobRepository = jobRepository;
        _jobQueue = jobQueue;
        _jobEventRepository = jobEventRepository;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc />
    public async Task<ProcessingJob> CreateJobAsync(CreateJobCommand command, CancellationToken cancellationToken)
    {
        // Create a new job with all stages initialized to Pending.
        var job = new ProcessingJob(
            Guid.NewGuid(),
            command.SatelliteName,
            command.RawDataName,
            command.RawDataSizeBytes,
            _timeProvider.UtcNow);

        await _jobRepository.AddAsync(job, cancellationToken).ConfigureAwait(false);

        // Emit an outbox event for UI polling.
        await _jobEventRepository.AppendAsync(
            JobEventType.JobCreated,
            job.Id,
            stage: null,
            status: null,
            message: "Job created and queued.",
            occurredAt: _timeProvider.UtcNow,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        // Enqueue the job so a worker can process it.
        await _jobQueue.EnqueueAsync(job.Id, cancellationToken).ConfigureAwait(false);

        return job;
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ProcessingJob>> ListJobsAsync(CancellationToken cancellationToken)
    {
        return _jobRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<ProcessingJob?> GetJobAsync(Guid jobId, CancellationToken cancellationToken)
    {
        return _jobRepository.GetAsync(jobId, cancellationToken);
    }
}
