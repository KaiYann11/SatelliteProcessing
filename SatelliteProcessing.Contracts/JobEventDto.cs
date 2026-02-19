namespace SatelliteProcessing.Contracts;

/// <summary>
/// Represents a single event emitted during job processing, used for real-time monitoring.
/// </summary>
public sealed class JobEventDto
{
    /// <summary>
    /// Gets or sets the monotonic sequence number for polling.
    /// </summary>
    public long Sequence { get; set; }

    /// <summary>
    /// Gets or sets the unique event identifier.
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the associated job identifier.
    /// </summary>
    public Guid JobId { get; set; }

    /// <summary>
    /// Gets or sets the stage this event relates to, if any.
    /// </summary>
    public ProcessingStage? Stage { get; set; }

    /// <summary>
    /// Gets or sets the stage status this event relates to, if any.
    /// </summary>
    public ProcessingStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the event classification.
    /// </summary>
    public JobEventType EventType { get; set; }

    /// <summary>
    /// Gets or sets the time when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; set; }

    /// <summary>
    /// Gets or sets an optional detail message.
    /// </summary>
    public string? Message { get; set; }
}
