// 파일: SatelliteProcessing.Domain/JobFinalStatus.cs
// 설명: 작업의 최종 상태(성공, 실패 등)를 나타내는 열거형입니다.

namespace SatelliteProcessing.Domain;

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
