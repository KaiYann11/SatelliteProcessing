namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Defines the persisted payload for job storage.
/// </summary>
public sealed class JobStoreModel
{
    /// <summary>
    /// Gets or sets the job records stored in the file.
    /// </summary>
    public List<JobRecord> Jobs { get; set; } = new();
}
