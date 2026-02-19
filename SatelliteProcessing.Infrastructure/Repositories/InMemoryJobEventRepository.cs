using System.Linq;
using SatelliteProcessing.Application.Abstractions;
using SatelliteProcessing.Domain;

// 파일: SatelliteProcessing.Infrastructure/Repositories/InMemoryJobEventRepository.cs
// 설명: 테스트용 인메모리 이벤트 저장소 구현입니다.

namespace SatelliteProcessing.Infrastructure.Repositories;

/// <summary>
/// Provides an in-memory event repository for single-node deployments and testing.
/// </summary>
public sealed class InMemoryJobEventRepository : IJobEventRepository
{
    private readonly List<JobEvent> _events = new();
    private long _sequence = 1;
    private readonly object _lock = new();

    /// <inheritdoc />
    public Task<JobEvent> AppendAsync(
        JobEventType eventType,
        Guid jobId,
        ProcessingStage? stage,
        ProcessingStatus? status,
        string? message,
        DateTimeOffset occurredAt,
        CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            var jobEvent = new JobEvent(
                _sequence++,
                Guid.NewGuid(),
                jobId,
                stage,
                status,
                eventType,
                occurredAt,
                message);

            _events.Add(jobEvent);
            return Task.FromResult(jobEvent);
        }
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<JobEvent>> ListAfterSequenceAsync(
        long afterSequence,
        int maxCount,
        CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            var events = _events
                .Where(item => item.Sequence > afterSequence)
                .OrderBy(item => item.Sequence)
                .Take(maxCount)
                .ToList();

            return Task.FromResult((IReadOnlyList<JobEvent>)events);
        }
    }
}
