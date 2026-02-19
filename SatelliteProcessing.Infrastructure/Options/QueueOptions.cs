// 파일: SatelliteProcessing.Infrastructure/Options/QueueOptions.cs
// 설명: 큐 구성 옵션 모델입니다 (모드, 디렉터리, 폴링 간격 등).

namespace SatelliteProcessing.Infrastructure.Options;

/// <summary>
/// Provides configuration for the job distribution queue.
/// </summary>
public sealed class QueueOptions
{
    /// <summary>
    /// Gets or sets the queue mode, such as InMemory or File.
    /// </summary>
    public string Mode { get; set; } = "File";

    /// <summary>
    /// Gets or sets the base directory for queue files.
    /// </summary>
    public string DataDirectory { get; set; } = "data";

    /// <summary>
    /// Gets or sets the delay in milliseconds between dequeue polling attempts.
    /// </summary>
    public int PollIntervalMs { get; set; } = 500;
}
