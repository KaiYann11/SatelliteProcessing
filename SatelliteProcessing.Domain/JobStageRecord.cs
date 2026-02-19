namespace SatelliteProcessing.Domain;

/// <summary>
/// Captures the execution details of a single pipeline stage.
/// </summary>
public sealed class JobStageRecord
{
    /// <summary>
    /// Initializes a new stage record with Pending status.
    /// </summary>
    /// <param name="stage">The pipeline stage this record describes.</param>
    public JobStageRecord(ProcessingStage stage)
    {
        Stage = stage;
        Status = ProcessingStatus.Pending;
    }

    /// <summary>
    /// Gets the pipeline stage identifier.
    /// </summary>
    public ProcessingStage Stage { get; }

    /// <summary>
    /// Gets the current status of the stage.
    /// </summary>
    public ProcessingStatus Status { get; private set; }

    /// <summary>
    /// Gets the timestamp when the stage started processing.
    /// </summary>
    public DateTimeOffset? StartedAt { get; private set; }

    /// <summary>
    /// Gets the timestamp when the stage finished processing.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; private set; }

    /// <summary>
    /// Gets an optional error message recorded on failure.
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Marks the stage as in progress and captures the start time.
    /// </summary>
    /// <param name="startedAt">The time when processing begins.</param>
    public void MarkProcessing(DateTimeOffset startedAt)
    {
        Status = ProcessingStatus.Processing;
        StartedAt = startedAt;
        CompletedAt = null;
        ErrorMessage = null;
    }

    /// <summary>
    /// Marks the stage as successfully completed and captures the completion time.
    /// </summary>
    /// <param name="completedAt">The time when processing ends.</param>
    public void MarkSuccess(DateTimeOffset completedAt)
    {
        Status = ProcessingStatus.Success;
        CompletedAt = completedAt;
        ErrorMessage = null;
    }

    /// <summary>
    /// Marks the stage as failed and captures the completion time and error.
    /// </summary>
    /// <param name="completedAt">The time when processing ends.</param>
    /// <param name="errorMessage">The error details to store for diagnostics.</param>
    public void MarkFailed(DateTimeOffset completedAt, string errorMessage)
    {
        Status = ProcessingStatus.Failed;
        CompletedAt = completedAt;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Marks the stage as canceled and captures the completion time.
    /// </summary>
    /// <param name="completedAt">The time when cancellation completes.</param>
    public void MarkCanceled(DateTimeOffset completedAt)
    {
        Status = ProcessingStatus.Canceled;
        CompletedAt = completedAt;
        ErrorMessage = null;
    }
}
