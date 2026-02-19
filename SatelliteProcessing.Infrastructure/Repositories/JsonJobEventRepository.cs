using System.Linq;
using SatelliteProcessing.Application.Abstractions;
using SatelliteProcessing.Domain;
using SatelliteProcessing.Infrastructure.Options;
using SatelliteProcessing.Infrastructure.Storage;

namespace SatelliteProcessing.Infrastructure.Repositories;

/// <summary>
/// Provides a JSON file-based event repository for simple persistence.
/// </summary>
public sealed class JsonJobEventRepository : IJobEventRepository
{
    private readonly JsonFileStore<JobEventStoreModel> _store;
    private readonly int _maxEventCount;

    /// <summary>
    /// Initializes a new repository using the configured storage options.
    /// </summary>
    /// <param name="options">Storage options used to locate the backing file.</param>
    public JsonJobEventRepository(StorageOptions options)
    {
        var filePath = Path.Combine(options.DataDirectory, "events.json");
        var mutexName = MutexNameBuilder.Create("SatelliteProcessing.Events", filePath);
        _store = new JsonFileStore<JobEventStoreModel>(filePath, mutexName);
        _maxEventCount = options.MaxEventCount;
    }

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
        return _store.UpdateAsync(model =>
        {
            var sequence = model.NextSequence++;
            var record = new JobEventRecord
            {
                Sequence = sequence,
                EventId = Guid.NewGuid(),
                JobId = jobId,
                Stage = stage,
                Status = status,
                EventType = eventType,
                OccurredAt = occurredAt,
                Message = message
            };

            model.Events.Add(record);
            TrimEvents(model);

            return new JobEvent(
                record.Sequence,
                record.EventId,
                record.JobId,
                record.Stage,
                record.Status,
                record.EventType,
                record.OccurredAt,
                record.Message);
        }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<JobEvent>> ListAfterSequenceAsync(
        long afterSequence,
        int maxCount,
        CancellationToken cancellationToken)
    {
        var model = await _store.LoadAsync(cancellationToken).ConfigureAwait(false);

        var events = model.Events
            .Where(item => item.Sequence > afterSequence)
            .OrderBy(item => item.Sequence)
            .Take(maxCount)
            .Select(item => new JobEvent(
                item.Sequence,
                item.EventId,
                item.JobId,
                item.Stage,
                item.Status,
                item.EventType,
                item.OccurredAt,
                item.Message))
            .ToList();

        return events;
    }

    private void TrimEvents(JobEventStoreModel model)
    {
        if (_maxEventCount <= 0)
        {
            return;
        }

        var overflow = model.Events.Count - _maxEventCount;
        if (overflow > 0)
        {
            model.Events.RemoveRange(0, overflow);
        }
    }
}
