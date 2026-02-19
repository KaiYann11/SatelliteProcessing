namespace SatelliteProcessing.Application.Models;

/// <summary>
/// Captures the input required to create a new processing job.
/// </summary>
public sealed class CreateJobCommand
{
    /// <summary>
    /// Initializes a new instance of the command.
    /// </summary>
    /// <param name="satelliteName">Name or identifier of the satellite source.</param>
    /// <param name="rawDataName">Display name of the raw data payload.</param>
    /// <param name="rawDataSizeBytes">Approximate size of the raw data payload.</param>
    public CreateJobCommand(string satelliteName, string rawDataName, long rawDataSizeBytes)
    {
        SatelliteName = satelliteName;
        RawDataName = rawDataName;
        RawDataSizeBytes = rawDataSizeBytes;
    }

    /// <summary>
    /// Gets the satellite source name or identifier.
    /// </summary>
    public string SatelliteName { get; }

    /// <summary>
    /// Gets the raw data display name.
    /// </summary>
    public string RawDataName { get; }

    /// <summary>
    /// Gets the approximate raw data size in bytes.
    /// </summary>
    public long RawDataSizeBytes { get; }
}
