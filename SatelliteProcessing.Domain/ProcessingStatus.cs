// 파일: SatelliteProcessing.Domain/ProcessingStatus.cs
// 설명: 처리 단계의 실행 상태를 나타내는 열거형입니다. 각 값은 단계의 현재 상태를 표현합니다.

namespace SatelliteProcessing.Domain;

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
