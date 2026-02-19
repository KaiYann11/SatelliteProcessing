namespace SatelliteProcessing.Contracts;

/// <summary>
/// Represents the execution status of a processing stage.
/// </summary>
public enum ProcessingStatus
{
    /// <summary>
    /// Stage is waiting to be processed.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Stage is currently in progress.
    /// </summary>
    Processing = 1,

    /// <summary>
    /// Stage completed successfully.
    /// </summary>
    Success = 2,

    /// <summary>
    /// Stage failed during processing.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Stage was canceled before completion.
    /// </summary>
    Canceled = 4
}
