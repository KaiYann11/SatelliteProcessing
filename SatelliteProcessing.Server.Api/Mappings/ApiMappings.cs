using System.Linq;
using SatelliteProcessing.Application.Models;
using SatelliteProcessing.Contracts;
using SatelliteProcessing.Domain;

namespace SatelliteProcessing.Server.Api.Mappings;

/// <summary>
/// Provides mapping helpers between domain objects and API contracts.
/// </summary>
internal static class ApiMappings
{
    /// <summary>
    /// Converts a create request into an application command.
    /// </summary>
    /// <param name="request">The incoming API request payload.</param>
    /// <returns>The application command.</returns>
    public static CreateJobCommand ToCommand(CreateJobRequest request)
    {
        return new CreateJobCommand(
            request.SatelliteName,
            request.RawDataName,
            request.RawDataSizeBytes);
    }

    /// <summary>
    /// Converts a domain job into a transport-friendly DTO.
    /// </summary>
    /// <param name="job">The domain job to convert.</param>
    /// <returns>The job DTO.</returns>
    public static JobDto ToDto(ProcessingJob job)
    {
        return new JobDto
        {
            Id = job.Id,
            SatelliteName = job.SatelliteName,
            RawDataName = job.RawDataName,
            RawDataSizeBytes = job.RawDataSizeBytes,
            CreatedAt = job.CreatedAt,
            CurrentStage = (ProcessingStage)job.CurrentStage,
            FinalStatus = job.FinalStatus is null ? null : (JobFinalStatus?)job.FinalStatus,
            CompletedAt = job.CompletedAt,
            Stages = job.Stages.Select(ToDto).ToList()
        };
    }

    /// <summary>
    /// Converts a domain stage record into a DTO.
    /// </summary>
    /// <param name="stage">The stage record to convert.</param>
    /// <returns>The stage DTO.</returns>
    public static JobStageDto ToDto(JobStageRecord stage)
    {
        return new JobStageDto
        {
            Stage = (ProcessingStage)stage.Stage,
            Status = (ProcessingStatus)stage.Status,
            StartedAt = stage.StartedAt,
            CompletedAt = stage.CompletedAt,
            ErrorMessage = stage.ErrorMessage
        };
    }

    /// <summary>
    /// Converts a domain event into a DTO.
    /// </summary>
    /// <param name="jobEvent">The event to convert.</param>
    /// <returns>The event DTO.</returns>
    public static JobEventDto ToDto(JobEvent jobEvent)
    {
        return new JobEventDto
        {
            Sequence = jobEvent.Sequence,
            EventId = jobEvent.EventId,
            JobId = jobEvent.JobId,
            Stage = jobEvent.Stage is null ? null : (ProcessingStage?)jobEvent.Stage,
            Status = jobEvent.Status is null ? null : (ProcessingStatus?)jobEvent.Status,
            EventType = (JobEventType)jobEvent.EventType,
            OccurredAt = jobEvent.OccurredAt,
            Message = jobEvent.Message
        };
    }
}
