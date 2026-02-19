using SatelliteProcessing.Application.Abstractions;

namespace SatelliteProcessing.Infrastructure.Time;

/// <summary>
/// Provides the system clock implementation for timestamps.
/// </summary>
public sealed class SystemTimeProvider : ITimeProvider
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
