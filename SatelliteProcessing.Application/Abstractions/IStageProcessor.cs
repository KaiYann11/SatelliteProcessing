using SatelliteProcessing.Domain;

namespace SatelliteProcessing.Application.Abstractions;

/// <summary>
/// Defines the processing implementation for a single pipeline stage.
/// </summary>
public interface IStageProcessor
{
    /// <summary>
    /// Executes the processing logic for the specified stage.
    /// </summary>
    /// <param name="context">Context containing job and stage metadata.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The result of the stage execution.</returns>
    Task<StageProcessingResult> ProcessAsync(
        StageProcessingContext context,
        CancellationToken cancellationToken);
}
