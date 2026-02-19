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
