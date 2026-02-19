namespace SatelliteProcessing.Contracts;

/// <summary>
/// Represents a processing job with all pipeline stages for monitoring.
/// </summary>
public sealed class JobDto
{
    /// <summary>
    /// Gets or sets the unique job identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the satellite source name or identifier.
    /// </summary>
    public string SatelliteName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the raw data display name.
    /// </summary>
    public string RawDataName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the approximate raw data size in bytes.
    /// </summary>
    public long RawDataSizeBytes { get; set; }

    /// <summary>
    /// Gets or sets the time the job was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the current pipeline stage being processed.
    /// </summary>
    public ProcessingStage CurrentStage { get; set; }

    /// <summary>
    /// Gets or sets the final job status if completed or terminated.
    /// </summary>
    public JobFinalStatus? FinalStatus { get; set; }

    /// <summary>
    /// Gets or sets the time the job completed.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the ordered list of pipeline stages.
    /// </summary>
    public List<JobStageDto> Stages { get; set; } = new();
}
