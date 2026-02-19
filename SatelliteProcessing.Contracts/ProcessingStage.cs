// 파일: SatelliteProcessing.Contracts/ProcessingStage.cs
// 설명: 처리 단계 정보를 나타내는 계약용 열거형입니다 (API DTO에서 사용됨).

namespace SatelliteProcessing.Contracts;

/// <summary>
/// Defines the ordered stages of the satellite image processing pipeline.
/// </summary>
public enum ProcessingStage
{
    /// <summary>
    /// Raw data ingestion stage.
    /// </summary>
    DataIngestion = 0,

    /// <summary>
    /// Input data validation stage.
    /// </summary>
    InputValidation = 1,

    /// <summary>
    /// Blur processing stage.
    /// </summary>
    Blur = 2,

    /// <summary>
    /// Mosaic processing stage.
    /// </summary>
    Mosaic = 3,

    /// <summary>
    /// Output merging stage.
    /// </summary>
    OutputMerge = 4,

    /// <summary>
    /// Output validation stage.
    /// </summary>
    OutputValidation = 5,

    /// <summary>
    /// Distribution (deploy) stage.
    /// </summary>
    Distribution = 6
}
