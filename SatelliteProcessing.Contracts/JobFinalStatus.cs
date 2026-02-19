namespace SatelliteProcessing.Contracts;

/// <summary>
/// Represents the overall final result of a job.
/// </summary>
public enum JobFinalStatus
{
    /// <summary>
    /// Job completed successfully.
    /// </summary>
    Success = 0,

    /// <summary>
    /// Job failed during one of the stages.
    /// </summary>
    Failed = 1,

    /// <summary>
    /// Job was canceled by an operator or system policy.
    /// </summary>
    Canceled = 2
}
