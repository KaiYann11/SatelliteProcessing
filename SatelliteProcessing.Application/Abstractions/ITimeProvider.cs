namespace SatelliteProcessing.Application.Abstractions;

/// <summary>
/// Abstracts the system clock for testability and deterministic pipelines.
/// </summary>
public interface ITimeProvider
{
    /// <summary>
    /// Gets the current UTC timestamp.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}
