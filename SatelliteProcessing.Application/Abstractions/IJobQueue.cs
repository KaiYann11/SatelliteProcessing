// 파일: SatelliteProcessing.Application/Abstractions/IJobQueue.cs
// 설명: 작업 큐 추상 인터페이스입니다. 작업을 큐에 넣고 꺼내는 기능을 정의합니다.

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
