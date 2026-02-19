// 파일: SatelliteProcessing.Contracts/JobStageDto.cs
// 설명: 작업의 각 단계 정보를 전송하기 위한 DTO입니다. 단계 상태와 결과를 포함합니다.

namespace SatelliteProcessing.Contracts;

/// <summary>
/// Describes a single stage in the processing pipeline for a job.
/// </summary>
public sealed class JobStageDto
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
    /// Gets or sets the timestamp when the stage started processing.
    /// </summary>
    public DateTimeOffset? StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the stage completed processing.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the optional error message if the stage failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
