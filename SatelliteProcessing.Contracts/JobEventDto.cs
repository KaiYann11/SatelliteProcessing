// 파일: SatelliteProcessing.Contracts/JobEventDto.cs
// 설명: 작업 이벤트를 전송하기 위한 DTO입니다. 이벤트 시퀀스와 타입, 메시지를 포함합니다.

namespace SatelliteProcessing.Contracts;

/// <summary>
/// Represents a single event emitted during job processing, used for real-time monitoring.
/// </summary>
public sealed class JobEventDto
{
    /// <summary>
    /// Gets or sets the monotonic sequence number for polling.
    /// </summary>
    public long Sequence { get; set; }

    /// <summary>
    /// Gets or sets the unique event identifier.
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the associated job identifier.
    /// </summary>
    public Guid JobId { get; set; }

    /// <summary>
    /// Gets or sets the stage this event relates to, if any.
    /// </summary>
    public ProcessingStage? Stage { get; set; }

    /// <summary>
    /// Gets or sets the stage status this event relates to, if any.
    /// </summary>
    public ProcessingStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the event classification.
    /// </summary>
    public JobEventType EventType { get; set; }

    /// <summary>
    /// Gets or sets the time when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; set; }

    /// <summary>
    /// Gets or sets an optional detail message.
    /// </summary>
    public string? Message { get; set; }
}
