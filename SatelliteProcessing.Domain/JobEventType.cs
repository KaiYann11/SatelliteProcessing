// 파일: SatelliteProcessing.Domain/JobEventType.cs
// 설명: 도메인에서 사용되는 작업 이벤트 유형을 정의하는 열거형입니다.

namespace SatelliteProcessing.Domain;

/// <summary>
/// Describes the type of event emitted during job processing.
/// </summary>
public enum JobEventType
{
    /// <summary>
    /// Job was created and queued.
    /// </summary>
    JobCreated = 0,

    /// <summary>
    /// A pipeline stage started.
    /// </summary>
    StageStarted = 1,

    /// <summary>
    /// A pipeline stage completed successfully.
    /// </summary>
    StageCompleted = 2,

    /// <summary>
    /// A pipeline stage failed.
    /// </summary>
    StageFailed = 3,

    /// <summary>
    /// Job completed successfully.
    /// </summary>
    JobCompleted = 4,

    /// <summary>
    /// Job failed and will not continue.
    /// </summary>
    JobFailed = 5,

    /// <summary>
    /// Job was canceled.
    /// </summary>
    JobCanceled = 6
}
