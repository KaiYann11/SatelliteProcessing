// 파일: SatelliteProcessing.Contracts/JobFinalStatus.cs
// 설명: 작업의 최종 상태를 표현하는 계약용 열거형입니다 (API 응답에서 사용됨).

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
