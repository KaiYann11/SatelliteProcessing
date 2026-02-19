using SatelliteProcessing.Domain;

// 파일: SatelliteProcessing.Infrastructure/Storage/JobStageRecordData.cs
// 설명: 작업 단계 기록을 파일에 저장하기 위한 데이터 구조입니다.

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
