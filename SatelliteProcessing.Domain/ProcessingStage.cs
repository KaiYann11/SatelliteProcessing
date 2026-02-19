// 파일: SatelliteProcessing.Domain/ProcessingStage.cs
// 설명: 처리 파이프라인의 각 단계를 정의하는 열거형입니다. 단계 순서를 나타냅니다.

namespace SatelliteProcessing.Domain;

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
