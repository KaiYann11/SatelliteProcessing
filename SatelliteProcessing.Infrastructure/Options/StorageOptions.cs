namespace SatelliteProcessing.Infrastructure.Options;

/// <summary>
/// Provides configuration for job and event storage.
/// </summary>
public sealed class StorageOptions
{
    /// <summary>
    /// Gets or sets the storage mode, such as InMemory or JsonFile.
    /// </summary>
    public string Mode { get; set; } = "JsonFile";

    /// <summary>
    /// Gets or sets the base directory for storage files.
    /// </summary>
    public string DataDirectory { get; set; } = "data";

    /// <summary>
    /// Gets or sets the maximum number of events to retain.
    /// </summary>
    public int MaxEventCount { get; set; } = 5000;
}
