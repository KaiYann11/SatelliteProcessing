namespace SatelliteProcessing.Domain;

/// <summary>
/// Represents a single event emitted during job processing, used for real-time monitoring.
/// </summary>
public sealed class JobEvent
{
    /// <summary>
    /// Initializes a new event with the required fields.
    /// </summary>
    /// <param name="sequence">Monotonic sequence number used for polling.</param>
    /// <param name="eventId">Unique identifier for the event.</param>
    /// <param name="jobId">Identifier of the job the event belongs to.</param>
    /// <param name="stage">Optional stage related to the event.</param>
    /// <param name="status">Optional status related to the event.</param>
    /// <param name="eventType">The event classification.</param>
    /// <param name="occurredAt">Timestamp when the event occurred.</param>
    /// <param name="message">Optional detail message.</param>
    public JobEvent(
        long sequence,
        Guid eventId,
        Guid jobId,
        ProcessingStage? stage,
        ProcessingStatus? status,
        JobEventType eventType,
        DateTimeOffset occurredAt,
        string? message)
    {
        Sequence = sequence;
        EventId = eventId;
        JobId = jobId;
        Stage = stage;
        Status = status;
        EventType = eventType;
        OccurredAt = occurredAt;
        Message = message;
    }

    /// <summary>
    /// Gets the monotonic sequence number for polling.
    /// </summary>
    public long Sequence { get; }

    /// <summary>
    /// Gets the unique event identifier.
    /// </summary>
    public Guid EventId { get; }

    /// <summary>
    /// Gets the associated job identifier.
    /// </summary>
    public Guid JobId { get; }

    /// <summary>
    /// Gets the stage this event relates to, if any.
    /// </summary>
    public ProcessingStage? Stage { get; }

    /// <summary>
    /// Gets the stage status this event relates to, if any.
    /// </summary>
    public ProcessingStatus? Status { get; }

    /// <summary>
    /// Gets the event classification.
    /// </summary>
    public JobEventType EventType { get; }

    /// <summary>
    /// Gets the time when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; }

    /// <summary>
    /// Gets an optional detail message.
    /// </summary>
    public string? Message { get; }
}
