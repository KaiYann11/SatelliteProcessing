using SatelliteProcessing.Domain;

namespace SatelliteProcessing.Application.Abstractions;

/// <summary>
/// Defines persistence operations for job event outbox records.
/// </summary>
public interface IJobEventRepository
{
    /// <summary>
    /// Appends a new event to the outbox.
    /// </summary>
    /// <param name="eventType">The classification of the event.</param>
    /// <param name="jobId">The job associated with the event.</param>
    /// <param name="stage">Optional pipeline stage related to the event.</param>
    /// <param name="status">Optional processing status related to the event.</param>
    /// <param name="message">Optional detail message.</param>
    /// <param name="occurredAt">Timestamp when the event occurred.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The stored event instance with sequence assigned.</returns>
    Task<JobEvent> AppendAsync(
        JobEventType eventType,
        Guid jobId,
        ProcessingStage? stage,
        ProcessingStatus? status,
        string? message,
        DateTimeOffset occurredAt,
        CancellationToken cancellationToken);

    /// <summary>
    /// Lists events with sequence greater than the provided value.
    /// </summary>
    /// <param name="afterSequence">The last seen sequence number.</param>
    /// <param name="maxCount">Maximum number of events to return.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of events in sequence order.</returns>
    Task<IReadOnlyList<JobEvent>> ListAfterSequenceAsync(
        long afterSequence,
        int maxCount,
        CancellationToken cancellationToken);
}
