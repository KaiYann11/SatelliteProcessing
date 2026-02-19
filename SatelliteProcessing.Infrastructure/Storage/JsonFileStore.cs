using System.Text.Json;

namespace SatelliteProcessing.Infrastructure.Storage;

/// <summary>
/// Provides JSON-based persistence with a shared file lock.
/// </summary>
public sealed class JsonFileStore<TModel>
    where TModel : class, new()
{
    private readonly string _filePath;
    private readonly string _mutexName;
    private readonly JsonSerializerOptions _serializerOptions;

    /// <summary>
    /// Initializes a new JSON store with a file path and mutex name.
    /// </summary>
    /// <param name="filePath">The file used to store the JSON payload.</param>
    /// <param name="mutexName">The mutex name used for cross-process synchronization.</param>
    public JsonFileStore(string filePath, string mutexName)
    {
        _filePath = filePath;
        _mutexName = mutexName;
        _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    /// <summary>
    /// Loads the stored model or returns a new instance if the file is missing.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The persisted model.</returns>
    public async Task<TModel> LoadAsync(CancellationToken cancellationToken)
    {
        using var mutexLock = new FileMutexLock(_mutexName);
        mutexLock.Wait(cancellationToken);

        return await LoadUnlockedAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Saves the provided model to disk with an exclusive lock.
    /// </summary>
    /// <param name="model">The model to persist.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    public async Task SaveAsync(TModel model, CancellationToken cancellationToken)
    {
        using var mutexLock = new FileMutexLock(_mutexName);
        mutexLock.Wait(cancellationToken);

        await SaveUnlockedAsync(model, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes a read-modify-write operation under a single lock.
    /// </summary>
    /// <typeparam name="TResult">The result type returned by the update function.</typeparam>
    /// <param name="updateFunc">Function that updates the model and returns a result.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The result produced by the update function.</returns>
    public async Task<TResult> UpdateAsync<TResult>(
        Func<TModel, TResult> updateFunc,
        CancellationToken cancellationToken)
    {
        using var mutexLock = new FileMutexLock(_mutexName);
        mutexLock.Wait(cancellationToken);

        var model = await LoadUnlockedAsync(cancellationToken).ConfigureAwait(false);
        var result = updateFunc(model);
        await SaveUnlockedAsync(model, cancellationToken).ConfigureAwait(false);
        return result;
    }

    private async Task<TModel> LoadUnlockedAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
        {
            return new TModel();
        }

        var json = await File.ReadAllTextAsync(_filePath, cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new TModel();
        }

        var model = JsonSerializer.Deserialize<TModel>(json, _serializerOptions);
        return model ?? new TModel();
    }

    private async Task SaveUnlockedAsync(TModel model, CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(model, _serializerOptions);
        await File.WriteAllTextAsync(_filePath, json, cancellationToken).ConfigureAwait(false);
    }
}
