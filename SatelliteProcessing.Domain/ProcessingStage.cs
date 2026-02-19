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
