// 파일: SatelliteProcessing.Infrastructure/Storage/QueueStoreModel.cs
// 설명: 큐에 저장되는 항목 모델(직렬화용)입니다.

namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Defines the persisted payload for the file-backed queue.
/// </summary>
public sealed class QueueStoreModel
{
    /// <summary>
    /// Gets or sets the queued job identifiers in FIFO order.
    /// </summary>
    public List<Guid> Items { get; set; } = new();
}
