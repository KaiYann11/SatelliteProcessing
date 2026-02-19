using System.Collections.Concurrent;
using System.Linq;
using SatelliteProcessing.Application.Abstractions;
using SatelliteProcessing.Domain;

// 파일: SatelliteProcessing.Infrastructure/Repositories/InMemoryJobRepository.cs
// 설명: 테스트 및 개발용 인메모리 작업 저장소 구현입니다.

namespace SatelliteProcessing.Infrastructure.Repositories;

/// <summary>
/// Provides an in-memory job repository for single-node deployments and testing.
/// </summary>
public sealed class InMemoryJobRepository : IJobRepository
{
    private readonly ConcurrentDictionary<Guid, ProcessingJob> _jobs = new();

    /// <inheritdoc />
    public Task AddAsync(ProcessingJob job, CancellationToken cancellationToken)
    {
        if (!_jobs.TryAdd(job.Id, job))
        {
            throw new InvalidOperationException($"Job {job.Id} already exists.");
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(ProcessingJob job, CancellationToken cancellationToken)
    {
        _jobs[job.Id] = job;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<ProcessingJob?> GetAsync(Guid jobId, CancellationToken cancellationToken)
    {
        _jobs.TryGetValue(jobId, out var job);
        return Task.FromResult(job);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ProcessingJob>> ListAsync(CancellationToken cancellationToken)
    {
        var ordered = _jobs.Values
            .OrderByDescending(job => job.CreatedAt)
            .ToList();

        return Task.FromResult((IReadOnlyList<ProcessingJob>)ordered);
    }
}
