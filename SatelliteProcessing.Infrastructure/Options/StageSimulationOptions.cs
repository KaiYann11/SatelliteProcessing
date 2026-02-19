using SatelliteProcessing.Domain;

namespace SatelliteProcessing.Infrastructure.Options;

/// <summary>
/// Provides configuration for simulated stage processing behavior.
/// </summary>
public sealed class StageSimulationOptions
{
    /// <summary>
    /// Gets or sets the default minimum delay for a stage in milliseconds.
    /// </summary>
    public int DefaultMinDelayMs { get; set; } = 400;

    /// <summary>
    /// Gets or sets the default maximum delay for a stage in milliseconds.
    /// </summary>
    public int DefaultMaxDelayMs { get; set; } = 1500;

    /// <summary>
    /// Gets or sets the default failure rate for a stage between 0.0 and 1.0.
    /// </summary>
    public double DefaultFailureRate { get; set; } = 0.05;

    /// <summary>
    /// Gets or sets the stage-specific simulation profiles.
    /// </summary>
    public List<StageSimulationProfile> Profiles { get; set; } = new();
}

/// <summary>
/// Describes the simulated behavior for a specific pipeline stage.
/// </summary>
public sealed class StageSimulationProfile
{
    /// <summary>
    /// Gets or sets the stage that this profile applies to.
    /// </summary>
    public ProcessingStage Stage { get; set; }

    /// <summary>
    /// Gets or sets the minimum delay for this stage in milliseconds.
    /// </summary>
    public int MinDelayMs { get; set; }

    /// <summary>
    /// Gets or sets the maximum delay for this stage in milliseconds.
    /// </summary>
    public int MaxDelayMs { get; set; }

    /// <summary>
    /// Gets or sets the failure rate for this stage between 0.0 and 1.0.
    /// </summary>
    public double FailureRate { get; set; }
}
