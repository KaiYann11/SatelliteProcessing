using SatelliteProcessing.Domain;
using System.Linq;

namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Serializable representation of a processing job for persistence.
/// </summary>
public sealed class JobRecord
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
    /// Gets or sets the current pipeline stage.
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
    /// Gets or sets the pipeline stage records for this job.
    /// </summary>
    public List<JobStageRecordData> Stages { get; set; } = new();

    /// <summary>
    /// Creates a serializable record from a domain job.
    /// </summary>
    /// <param name="job">The domain job to convert.</param>
    /// <returns>The serialized job record.</returns>
    public static JobRecord FromDomain(ProcessingJob job)
    {
        return new JobRecord
        {
            Id = job.Id,
            SatelliteName = job.SatelliteName,
            RawDataName = job.RawDataName,
            RawDataSizeBytes = job.RawDataSizeBytes,
            CreatedAt = job.CreatedAt,
            CurrentStage = job.CurrentStage,
            FinalStatus = job.FinalStatus,
            CompletedAt = job.CompletedAt,
            Stages = job.Stages.Select(stage => new JobStageRecordData
            {
                Stage = stage.Stage,
                Status = stage.Status,
                StartedAt = stage.StartedAt,
                CompletedAt = stage.CompletedAt,
                ErrorMessage = stage.ErrorMessage
            }).ToList()
        };
    }

    /// <summary>
    /// Rebuilds a domain job from the persisted record.
    /// </summary>
    /// <returns>The reconstructed domain job.</returns>
    public ProcessingJob ToDomain()
    {
        // Initialize a fresh job with default pending stages.
        var job = new ProcessingJob(Id, SatelliteName, RawDataName, RawDataSizeBytes, CreatedAt);

        foreach (var stageRecord in Stages.OrderBy(stage => stage.Stage))
        {
            switch (stageRecord.Status)
            {
                case ProcessingStatus.Pending:
                    continue;
                case ProcessingStatus.Processing:
                    job.StartStage(stageRecord.Stage, stageRecord.StartedAt ?? CreatedAt);
                    break;
                case ProcessingStatus.Success:
                    job.StartStage(stageRecord.Stage, stageRecord.StartedAt ?? CreatedAt);
                    job.CompleteStage(stageRecord.Stage, stageRecord.CompletedAt ?? stageRecord.StartedAt ?? CreatedAt);
                    break;
                case ProcessingStatus.Failed:
                    job.StartStage(stageRecord.Stage, stageRecord.StartedAt ?? CreatedAt);
                    job.FailStage(
                        stageRecord.Stage,
                        stageRecord.CompletedAt ?? stageRecord.StartedAt ?? CreatedAt,
                        stageRecord.ErrorMessage ?? "Stage failed.");
                    break;
                case ProcessingStatus.Canceled:
                    job.StartStage(stageRecord.Stage, stageRecord.StartedAt ?? CreatedAt);
                    job.Cancel(stageRecord.CompletedAt ?? stageRecord.StartedAt ?? CreatedAt);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported stage status: {stageRecord.Status}.");
            }
        }

        return job;
    }
}
