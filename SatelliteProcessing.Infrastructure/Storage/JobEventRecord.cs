using SatelliteProcessing.Domain;

namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Serializable representation of a job event for persistence.
/// </summary>
public sealed class JobEventRecord
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
    /// Gets or sets the optional detail message.
    /// </summary>
    public string? Message { get; set; }
}
