using SatelliteProcessing.Domain;

namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Serializable representation of a stage record for persistence.
/// </summary>
public sealed class JobStageRecordData
{
    /// <summary>
    /// Gets or sets the pipeline stage identifier.
    /// </summary>
    public ProcessingStage Stage { get; set; }

    /// <summary>
    /// Gets or sets the current status of the stage.
    /// </summary>
    public ProcessingStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the stage started.
    /// </summary>
    public DateTimeOffset? StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the stage completed.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the optional error message for failed stages.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
