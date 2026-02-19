using System.Linq;
using SatelliteProcessing.Application.Abstractions;
using SatelliteProcessing.Application.Models;
using SatelliteProcessing.Domain;
using SatelliteProcessing.Infrastructure.Options;

// 파일: SatelliteProcessing.Infrastructure/Processing/SimulatedStageProcessor.cs
// 설명: 시뮬레이션용 단계 프로세서 구현입니다. 지연과 실패율을 모방하여 테스트에 사용됩니다.

namespace SatelliteProcessing.Infrastructure.Processing;

/// <summary>
/// Simulates stage processing by waiting and randomly failing based on configuration.
/// </summary>
public sealed class SimulatedStageProcessor : IStageProcessor
{
    private readonly StageSimulationProfile _defaultProfile;
    private readonly Dictionary<ProcessingStage, StageSimulationProfile> _profiles;

    /// <summary>
    /// Initializes a new instance using the configured simulation options.
    /// </summary>
    /// <param name="options">Simulation options that control timing and failure rates.</param>
    public SimulatedStageProcessor(StageSimulationOptions options)
    {
        _defaultProfile = new StageSimulationProfile
        {
            Stage = ProcessingStage.DataIngestion,
            MinDelayMs = options.DefaultMinDelayMs,
            MaxDelayMs = options.DefaultMaxDelayMs,
            FailureRate = options.DefaultFailureRate
        };

        _profiles = options.Profiles.ToDictionary(profile => profile.Stage);
    }

    /// <inheritdoc />
    public async Task<StageProcessingResult> ProcessAsync(
        StageProcessingContext context,
        CancellationToken cancellationToken)
    {
        // Select stage-specific settings or fall back to defaults.
        var profile = _profiles.TryGetValue(context.Stage, out var configured)
            ? configured
            : _defaultProfile;

        var minDelay = Math.Min(profile.MinDelayMs, profile.MaxDelayMs);
        var maxDelay = Math.Max(profile.MinDelayMs, profile.MaxDelayMs);
        var delayMs = Random.Shared.Next(minDelay, maxDelay + 1);

        // Simulate work by delaying for the configured duration.
        await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);

        var failureRate = Math.Clamp(profile.FailureRate, 0.0, 1.0);
        if (Random.Shared.NextDouble() < failureRate)
        {
            return StageProcessingResult.Failure($"Simulated failure at {context.Stage}.");
        }

        return StageProcessingResult.Success($"Simulated output produced for {context.Stage}.");
    }
}
