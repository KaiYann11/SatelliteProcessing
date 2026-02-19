// 파일: SatelliteProcessing.Contracts/CreateJobRequest.cs
// 설명: API를 통해 작업 생성을 요청할 때 사용되는 DTO입니다.

namespace SatelliteProcessing.Contracts;

/// <summary>
/// Represents the payload used by clients to request a new job.
/// </summary>
public sealed class CreateJobRequest
{
    /// <summary>
    /// Gets or sets the satellite source name or identifier.
    /// </summary>
    public string SatelliteName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the raw data display name.
    /// </summary>
    public string RawDataName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the approximate raw data size in bytes.
    /// </summary>
    public long RawDataSizeBytes { get; set; }
}
