using System.Linq;
using SatelliteProcessing.Application.Abstractions;
using SatelliteProcessing.Domain;
using SatelliteProcessing.Infrastructure.Options;
using SatelliteProcessing.Infrastructure.Storage;

// 파일: SatelliteProcessing.Infrastructure/Repositories/JsonJobRepository.cs
// 설명: JSON 파일 기반 작업 저장소 구현입니다. 작업 데이터를 파일로 직렬화/역직렬화합니다.

namespace SatelliteProcessing.Infrastructure.Repositories;

/// <summary>
/// Provides a JSON file-based job repository for simple persistence.
/// </summary>
public sealed class JsonJobRepository : IJobRepository
{
    private readonly JsonFileStore<JobStoreModel> _store;

    /// <summary>
    /// Initializes a new repository using the configured storage options.
    /// </summary>
    /// <param name="options">Storage options used to locate the backing file.</param>
    public JsonJobRepository(StorageOptions options)
    {
        var filePath = Path.Combine(options.DataDirectory, "jobs.json");
        var mutexName = MutexNameBuilder.Create("SatelliteProcessing.Jobs", filePath);
        _store = new JsonFileStore<JobStoreModel>(filePath, mutexName);
    }

    /// <inheritdoc />
    public Task AddAsync(ProcessingJob job, CancellationToken cancellationToken)
    {
        var record = JobRecord.FromDomain(job);

        return _store.UpdateAsync(model =>
        {
            if (model.Jobs.Any(existing => existing.Id == record.Id))
            {
                throw new InvalidOperationException($"Job {record.Id} already exists.");
            }

            model.Jobs.Add(record);
            return 0;
        }, cancellationToken);
    }

    /// <inheritdoc />
    public Task UpdateAsync(ProcessingJob job, CancellationToken cancellationToken)
    {
        var record = JobRecord.FromDomain(job);

        return _store.UpdateAsync(model =>
        {
            var index = model.Jobs.FindIndex(existing => existing.Id == record.Id);
            if (index < 0)
            {
                model.Jobs.Add(record);
            }
            else
            {
                model.Jobs[index] = record;
            }

            return 0;
        }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ProcessingJob?> GetAsync(Guid jobId, CancellationToken cancellationToken)
    {
        var model = await _store.LoadAsync(cancellationToken).ConfigureAwait(false);
        var record = model.Jobs.FirstOrDefault(job => job.Id == jobId);
        return record?.ToDomain();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ProcessingJob>> ListAsync(CancellationToken cancellationToken)
    {
        var model = await _store.LoadAsync(cancellationToken).ConfigureAwait(false);

        var jobs = model.Jobs
            .Select(record => record.ToDomain())
            .OrderByDescending(job => job.CreatedAt)
            .ToList();

        return jobs;
    }
}
