// 파일: SatelliteProcessing.Infrastructure/Storage/JobEventStoreModel.cs
// 설명: 이벤트 저장소에 기록되는 이벤트 모델(직렬화용)입니다.

namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Defines the persisted payload for job event storage.
/// </summary>
public sealed class JobEventStoreModel
{
    /// <summary>
    /// Gets or sets the next sequence number to assign.
    /// </summary>
    public long NextSequence { get; set; } = 1;

    /// <summary>
    /// Gets or sets the stored event records.
    /// </summary>
    public List<JobEventRecord> Events { get; set; } = new();
}
