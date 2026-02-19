using SatelliteProcessing.Application.Abstractions;

// 파일: SatelliteProcessing.Infrastructure/Time/SystemTimeProvider.cs
// 설명: 시스템 시간을 제공하는 기본 `ITimeProvider` 구현입니다.

namespace SatelliteProcessing.Infrastructure.Time;

/// <summary>
/// Provides the system clock implementation for timestamps.
/// </summary>
public sealed class SystemTimeProvider : ITimeProvider
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
