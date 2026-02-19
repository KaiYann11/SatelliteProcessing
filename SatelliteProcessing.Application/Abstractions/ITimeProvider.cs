// 파일: SatelliteProcessing.Application/Abstractions/ITimeProvider.cs
// 설명: 시간 제공자 인터페이스입니다. 테스트용으로 시간을 주입할 수 있게 해줍니다.

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
