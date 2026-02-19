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
