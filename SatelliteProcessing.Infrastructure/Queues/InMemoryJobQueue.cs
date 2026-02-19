using System.Threading.Channels;
using SatelliteProcessing.Application.Abstractions;

// 파일: SatelliteProcessing.Infrastructure/Queues/InMemoryJobQueue.cs
// 설명: 인메모리 기반 작업 큐 구현입니다. 빠른 로컬 개발 및 테스트용입니다.

namespace SatelliteProcessing.Infrastructure.Queues;

/// <summary>
/// Provides an in-memory queue for distributing jobs to workers.
/// </summary>
public sealed class InMemoryJobQueue : IJobQueue
{
    private readonly Channel<Guid> _channel;

    /// <summary>
    /// Initializes a new instance backed by an unbounded channel.
    /// </summary>
    public InMemoryJobQueue()
    {
        _channel = Channel.CreateUnbounded<Guid>();
    }

    /// <inheritdoc />
    public async Task EnqueueAsync(Guid jobId, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(jobId, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Guid> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
    }
}
