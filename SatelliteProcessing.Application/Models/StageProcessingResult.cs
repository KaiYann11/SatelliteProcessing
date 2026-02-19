namespace SatelliteProcessing.Application.Models;

/// <summary>
/// Represents the outcome of executing a pipeline stage.
/// </summary>
public sealed class StageProcessingResult
{
    /// <summary>
    /// Initializes a new result instance.
    /// </summary>
    /// <param name="isSuccess">True if the stage completed successfully.</param>
    /// <param name="message">Optional detail message.</param>
    private StageProcessingResult(bool isSuccess, string? message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    /// <summary>
    /// Gets a value indicating whether the stage completed successfully.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the optional detail message.
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// Creates a successful result with an optional message.
    /// </summary>
    /// <param name="message">Optional detail message.</param>
    /// <returns>A success result.</returns>
    public static StageProcessingResult Success(string? message = null)
    {
        return new StageProcessingResult(true, message);
    }

    /// <summary>
    /// Creates a failure result with a required message.
    /// </summary>
    /// <param name="message">Detail message describing the failure.</param>
    /// <returns>A failure result.</returns>
    public static StageProcessingResult Failure(string message)
    {
        return new StageProcessingResult(false, message);
    }
}
