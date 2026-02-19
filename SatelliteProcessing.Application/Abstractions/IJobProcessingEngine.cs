// 파일: SatelliteProcessing.Application/Abstractions/IJobProcessingEngine.cs
// 설명: 작업 처리 엔진의 추상 인터페이스입니다. 작업을 실제로 실행하는 책임을 집니다.

namespace SatelliteProcessing.Application.Abstractions;

/// <summary>
/// Defines the orchestration service that runs a job through the pipeline.
/// </summary>
public interface IJobProcessingEngine
{
    /// <summary>
    /// Executes the full pipeline for a single job.
    /// </summary>
    /// <param name="jobId">Identifier of the job to process.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task ProcessJobAsync(Guid jobId, CancellationToken cancellationToken);
}
