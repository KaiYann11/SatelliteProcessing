using System.Collections.ObjectModel;
using System.Linq;

namespace SatelliteProcessing.Domain;

/// <summary>
/// Represents a single satellite image processing job and its pipeline state.
/// </summary>
public sealed class ProcessingJob
{
    private readonly List<JobStageRecord> _stages;

    /// <summary>
    /// Initializes a new instance of the job with pending stages.
    /// </summary>
    /// <param name="id">Unique identifier for the job.</param>
    /// <param name="satelliteName">Name or identifier of the satellite source.</param>
    /// <param name="rawDataName">Human-readable name for the raw data payload.</param>
    /// <param name="rawDataSizeBytes">Approximate size of the raw data payload.</param>
    /// <param name="createdAt">The time when the job was created.</param>
    public ProcessingJob(
        Guid id,
        string satelliteName,
        string rawDataName,
        long rawDataSizeBytes,
        DateTimeOffset createdAt)
    {
        Id = id;
        SatelliteName = satelliteName;
        RawDataName = rawDataName;
        RawDataSizeBytes = rawDataSizeBytes;
        CreatedAt = createdAt;
        CurrentStage = ProcessingStage.DataIngestion;
        _stages = CreateDefaultStages();
    }

    /// <summary>
    /// Gets the immutable pipeline order for all jobs.
    /// </summary>
    public static IReadOnlyList<ProcessingStage> PipelineOrder { get; } = new[]
    {
        ProcessingStage.DataIngestion,
        ProcessingStage.InputValidation,
        ProcessingStage.Blur,
        ProcessingStage.Mosaic,
        ProcessingStage.OutputMerge,
        ProcessingStage.OutputValidation,
        ProcessingStage.Distribution
    };

    /// <summary>
    /// Gets the unique job identifier.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the satellite source name or identifier.
    /// </summary>
    public string SatelliteName { get; }

    /// <summary>
    /// Gets the raw data display name.
    /// </summary>
    public string RawDataName { get; }

    /// <summary>
    /// Gets the approximate raw data size in bytes.
    /// </summary>
    public long RawDataSizeBytes { get; }

    /// <summary>
    /// Gets the time the job was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Gets the current stage being processed.
    /// </summary>
    public ProcessingStage CurrentStage { get; private set; }

    /// <summary>
    /// Gets the final job status if the pipeline completed or terminated.
    /// </summary>
    public JobFinalStatus? FinalStatus { get; private set; }

    /// <summary>
    /// Gets the completion time of the overall job.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; private set; }

    /// <summary>
    /// Gets the immutable list of stage records.
    /// </summary>
    public IReadOnlyList<JobStageRecord> Stages => new ReadOnlyCollection<JobStageRecord>(_stages);

    /// <summary>
    /// Returns the stage record for the requested pipeline stage.
    /// </summary>
    /// <param name="stage">The stage identifier to locate.</param>
    /// <returns>The matching stage record.</returns>
    public JobStageRecord GetStage(ProcessingStage stage)
    {
        return _stages.Single(record => record.Stage == stage);
    }

    /// <summary>
    /// Marks a stage as currently processing and updates the current stage pointer.
    /// </summary>
    /// <param name="stage">The stage to mark as in progress.</param>
    /// <param name="startedAt">The timestamp when processing began.</param>
    public void StartStage(ProcessingStage stage, DateTimeOffset startedAt)
    {
        GetStage(stage).MarkProcessing(startedAt);
        CurrentStage = stage;
    }

    /// <summary>
    /// Marks a stage as successfully completed and updates final status if it is the last stage.
    /// </summary>
    /// <param name="stage">The stage to mark as completed.</param>
    /// <param name="completedAt">The timestamp when processing finished.</param>
    public void CompleteStage(ProcessingStage stage, DateTimeOffset completedAt)
    {
        GetStage(stage).MarkSuccess(completedAt);

        // If the final stage completes, mark the overall job as successful.
        if (stage == PipelineOrder[^1])
        {
            FinalStatus = JobFinalStatus.Success;
            CompletedAt = completedAt;
        }
    }

    /// <summary>
    /// Marks a stage as failed and sets the overall job status to failed.
    /// </summary>
    /// <param name="stage">The stage that failed.</param>
    /// <param name="completedAt">The timestamp when the failure occurred.</param>
    /// <param name="errorMessage">Error details for diagnostics.</param>
    public void FailStage(ProcessingStage stage, DateTimeOffset completedAt, string errorMessage)
    {
        GetStage(stage).MarkFailed(completedAt, errorMessage);
        FinalStatus = JobFinalStatus.Failed;
        CompletedAt = completedAt;
    }

    /// <summary>
    /// Cancels the job and marks the current stage as canceled.
    /// </summary>
    /// <param name="completedAt">The timestamp when the cancellation finished.</param>
    public void Cancel(DateTimeOffset completedAt)
    {
        GetStage(CurrentStage).MarkCanceled(completedAt);
        FinalStatus = JobFinalStatus.Canceled;
        CompletedAt = completedAt;
    }

    /// <summary>
    /// Creates the default stage records for a new job.
    /// </summary>
    /// <returns>List of stage records with Pending status.</returns>
    private static List<JobStageRecord> CreateDefaultStages()
    {
        return PipelineOrder.Select(stage => new JobStageRecord(stage)).ToList();
    }
}
