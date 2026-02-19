using SatelliteProcessing.Domain;

// 파일: SatelliteProcessing.Application/Models/StageProcessingContext.cs
// 설명: 개별 단계가 처리될 때 전달되는 컨텍스트 정보를 포함하는 모델입니다.

namespace SatelliteProcessing.Application.Models;

/// <summary>
/// Provides metadata needed to process a specific pipeline stage.
/// </summary>
public sealed class StageProcessingContext
{
    /// <summary>
    /// Initializes a new instance of the context.
    /// </summary>
    /// <param name="jobId">The job identifier.</param>
    /// <param name="stage">The pipeline stage to execute.</param>
    /// <param name="satelliteName">The satellite source name.</param>
    /// <param name="rawDataName">The raw data display name.</param>
    /// <param name="rawDataSizeBytes">The raw data size in bytes.</param>
    public StageProcessingContext(
        Guid jobId,
        ProcessingStage stage,
        string satelliteName,
        string rawDataName,
        long rawDataSizeBytes)
    {
        JobId = jobId;
        Stage = stage;
        SatelliteName = satelliteName;
        RawDataName = rawDataName;
        RawDataSizeBytes = rawDataSizeBytes;
    }

    /// <summary>
    /// Gets the job identifier.
    /// </summary>
    public Guid JobId { get; }

    /// <summary>
    /// Gets the pipeline stage being processed.
    /// </summary>
    public ProcessingStage Stage { get; }

    /// <summary>
    /// Gets the satellite source name.
    /// </summary>
    public string SatelliteName { get; }

    /// <summary>
    /// Gets the raw data display name.
    /// </summary>
    public string RawDataName { get; }

    /// <summary>
    /// Gets the raw data size in bytes.
    /// </summary>
    public long RawDataSizeBytes { get; }
}
