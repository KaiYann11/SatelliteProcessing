using SatelliteProcessing.Application.Abstractions;
using SatelliteProcessing.Infrastructure.Options;
using SatelliteProcessing.Infrastructure.Storage;

// 파일: SatelliteProcessing.Infrastructure/Queues/FileJobQueue.cs
// 설명: 파일 시스템을 사용하여 작업을 큐잉하는 구현입니다. 디스크 기반 영속성을 제공합니다.

namespace SatelliteProcessing.Infrastructure.Queues;

/// <summary>
/// Provides a file-backed queue for distributing jobs across processes.
/// </summary>
public sealed class FileJobQueue : IJobQueue
{
    private readonly JsonFileStore<QueueStoreModel> _store;
    private readonly TimeSpan _pollInterval;

    /// <summary>
    /// Initializes a new instance using the configured queue options.
    /// </summary>
    /// <param name="options">Queue options used to locate the backing file.</param>
    public FileJobQueue(QueueOptions options)
    {
        var filePath = Path.Combine(options.DataDirectory, "queue.json");
        var mutexName = MutexNameBuilder.Create("SatelliteProcessing.Queue", filePath);
        _store = new JsonFileStore<QueueStoreModel>(filePath, mutexName);
        _pollInterval = TimeSpan.FromMilliseconds(Math.Max(50, options.PollIntervalMs));
    }

    /// <inheritdoc />
    public Task EnqueueAsync(Guid jobId, CancellationToken cancellationToken)
    {
        return _store.UpdateAsync(model =>
        {
            model.Items.Add(jobId);
            return 0;
        }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Guid> DequeueAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            var jobId = await _store.UpdateAsync(model =>
            {
                if (model.Items.Count == 0)
                {
                    return Guid.Empty;
                }

                var next = model.Items[0];
                model.Items.RemoveAt(0);
                return next;
            }, cancellationToken).ConfigureAwait(false);

            if (jobId != Guid.Empty)
            {
                return jobId;
            }

            await Task.Delay(_pollInterval, cancellationToken).ConfigureAwait(false);
        }
    }
}
