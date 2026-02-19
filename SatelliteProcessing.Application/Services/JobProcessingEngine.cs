using SatelliteProcessing.Application.Abstractions;
using SatelliteProcessing.Application.Models;
using SatelliteProcessing.Domain;

// 파일: SatelliteProcessing.Application/Services/JobProcessingEngine.cs
// 설명: 작업 처리 엔진 구현입니다. 각 처리 단계를 순차적으로 실행하고 이벤트를 생성합니다.

namespace SatelliteProcessing.Application.Services;

/// <summary>
/// Orchestrates the execution of a job through the processing pipeline.
/// </summary>
public sealed class JobProcessingEngine : IJobProcessingEngine
{
    private readonly IJobRepository _jobRepository;
    private readonly IJobEventRepository _eventRepository;
    private readonly IStageProcessor _stageProcessor;
    private readonly ITimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance with required dependencies.
    /// </summary>
    /// <param name="jobRepository">Repository used to store job state.</param>
    /// <param name="eventRepository">Outbox used for real-time monitoring events.</param>
    /// <param name="stageProcessor">Processor used to execute each pipeline stage.</param>
    /// <param name="timeProvider">Time provider for consistent timestamps.</param>
    public JobProcessingEngine(
        IJobRepository jobRepository,
        IJobEventRepository eventRepository,
        IStageProcessor stageProcessor,
        ITimeProvider timeProvider)
    {
        _jobRepository = jobRepository;
        _eventRepository = eventRepository;
        _stageProcessor = stageProcessor;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc />
    public async Task ProcessJobAsync(Guid jobId, CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetAsync(jobId, cancellationToken).ConfigureAwait(false);
        if (job is null)
        {
            return;
        }

        foreach (var stage in ProcessingJob.PipelineOrder)
        {
            if (job.FinalStatus is not null)
            {
                break;
            }

            var stageRecord = job.GetStage(stage);
            if (stageRecord.Status == ProcessingStatus.Success)
            {
                continue;
            }

            var startedAt = stageRecord.StartedAt ?? _timeProvider.UtcNow;
            job.StartStage(stage, startedAt);

            await _jobRepository.UpdateAsync(job, cancellationToken).ConfigureAwait(false);
            await _eventRepository.AppendAsync(
                JobEventType.StageStarted,
                job.Id,
                stage,
                ProcessingStatus.Processing,
                message: $"Stage {stage} started.",
                occurredAt: startedAt,
                cancellationToken: cancellationToken).ConfigureAwait(false);

            StageProcessingResult result;
            try
            {
                result = await _stageProcessor.ProcessAsync(
                    new StageProcessingContext(
                        job.Id,
                        stage,
                        job.SatelliteName,
                        job.RawDataName,
                        job.RawDataSizeBytes),
                    cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                result = StageProcessingResult.Failure(ex.Message);
            }

            if (result.IsSuccess)
            {
                var completedAt = _timeProvider.UtcNow;
                job.CompleteStage(stage, completedAt);

                await _jobRepository.UpdateAsync(job, cancellationToken).ConfigureAwait(false);
                await _eventRepository.AppendAsync(
                    JobEventType.StageCompleted,
                    job.Id,
                    stage,
                    ProcessingStatus.Success,
                    message: result.Message,
                    occurredAt: completedAt,
                    cancellationToken: cancellationToken).ConfigureAwait(false);

                if (job.FinalStatus == JobFinalStatus.Success)
                {
                    await _eventRepository.AppendAsync(
                        JobEventType.JobCompleted,
                        job.Id,
                        stage: null,
                        status: null,
                        message: "Job completed successfully.",
                        occurredAt: completedAt,
                        cancellationToken: cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                var completedAt = _timeProvider.UtcNow;
                job.FailStage(stage, completedAt, result.Message ?? "Stage failed.");

                await _jobRepository.UpdateAsync(job, cancellationToken).ConfigureAwait(false);
                await _eventRepository.AppendAsync(
                    JobEventType.StageFailed,
                    job.Id,
                    stage,
                    ProcessingStatus.Failed,
                    message: result.Message,
                    occurredAt: completedAt,
                    cancellationToken: cancellationToken).ConfigureAwait(false);

                await _eventRepository.AppendAsync(
                    JobEventType.JobFailed,
                    job.Id,
                    stage: null,
                    status: null,
                    message: "Job failed.",
                    occurredAt: completedAt,
                    cancellationToken: cancellationToken).ConfigureAwait(false);

                break;
            }
        }
    }
}
