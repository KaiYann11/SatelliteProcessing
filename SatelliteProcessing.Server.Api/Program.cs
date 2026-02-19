// 파일: SatelliteProcessing.Server.Api/Program.cs
// 설명: API 호스트의 진입점입니다. 웹 앱을 구성하고 DI 및 미들웨어를 설정합니다.

using SatelliteProcessing.Application.Abstractions;
using SatelliteProcessing.Application.Services;
using SatelliteProcessing.Contracts;
using SatelliteProcessing.Infrastructure.Options;
using SatelliteProcessing.Infrastructure.Processing;
using SatelliteProcessing.Infrastructure.Queues;
using SatelliteProcessing.Infrastructure.Repositories;
using SatelliteProcessing.Infrastructure.Storage;
using SatelliteProcessing.Infrastructure.Time;
using SatelliteProcessing.Server.Api.Mappings;

// Build the API host that coordinates job creation and monitoring.
var builder = WebApplication.CreateBuilder(args);

// Load and normalize storage, queue, and simulation options.
var storageOptions = LoadStorageOptions(builder.Configuration);
var queueOptions = LoadQueueOptions(builder.Configuration);
var simulationOptions = LoadSimulationOptions(builder.Configuration);

storageOptions.DataDirectory = StoragePathResolver.ResolveDataDirectory(
    storageOptions.DataDirectory,
    Directory.GetCurrentDirectory());
queueOptions.DataDirectory = StoragePathResolver.ResolveDataDirectory(
    queueOptions.DataDirectory,
    Directory.GetCurrentDirectory());

// Register configuration and infrastructure services.
builder.Services.AddSingleton(storageOptions);
builder.Services.AddSingleton(queueOptions);
builder.Services.AddSingleton(simulationOptions);

builder.Services.AddSingleton<ITimeProvider, SystemTimeProvider>();
builder.Services.AddSingleton<IJobRepository>(_ => CreateJobRepository(storageOptions));
builder.Services.AddSingleton<IJobEventRepository>(_ => CreateJobEventRepository(storageOptions));
builder.Services.AddSingleton<IJobQueue>(_ => CreateJobQueue(queueOptions));
builder.Services.AddSingleton<IJobService, JobService>();
builder.Services.AddSingleton<IStageProcessor>(_ => new SimulatedStageProcessor(simulationOptions));

// Enable CORS so the web client can call the API from another origin.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Simple heartbeat endpoint for health checks and load balancers.
app.MapGet("/", () => Results.Ok("Satellite Processing API is running."));

// Creates a new job and enqueues it for processing.
app.MapPost("/api/jobs", async (
    CreateJobRequest request,
    IJobService jobService,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.SatelliteName) ||
        string.IsNullOrWhiteSpace(request.RawDataName) ||
        request.RawDataSizeBytes <= 0)
    {
        return Results.BadRequest("SatelliteName, RawDataName, and RawDataSizeBytes are required.");
    }

    var job = await jobService.CreateJobAsync(
        ApiMappings.ToCommand(request),
        cancellationToken).ConfigureAwait(false);

    return Results.Created($"/api/jobs/{job.Id}", ApiMappings.ToDto(job));
});

// Lists all jobs for monitoring dashboards.
app.MapGet("/api/jobs", async (
    IJobService jobService,
    CancellationToken cancellationToken) =>
{
    var jobs = await jobService.ListJobsAsync(cancellationToken).ConfigureAwait(false);
    return Results.Ok(jobs.Select(ApiMappings.ToDto).ToList());
});

// Retrieves a single job by identifier.
app.MapGet("/api/jobs/{jobId:guid}", async (
    Guid jobId,
    IJobService jobService,
    CancellationToken cancellationToken) =>
{
    var job = await jobService.GetJobAsync(jobId, cancellationToken).ConfigureAwait(false);
    return job is null ? Results.NotFound() : Results.Ok(ApiMappings.ToDto(job));
});

// Provides incremental events for real-time monitoring clients.
app.MapGet("/api/events", async (
    long? afterSequence,
    int? maxCount,
    IJobEventRepository eventRepository,
    CancellationToken cancellationToken) =>
{
    var after = Math.Max(0, afterSequence ?? 0);
    var take = Math.Clamp(maxCount ?? 200, 1, 500);

    var events = await eventRepository
        .ListAfterSequenceAsync(after, take, cancellationToken)
        .ConfigureAwait(false);

    return Results.Ok(events.Select(ApiMappings.ToDto).ToList());
});

app.Run();

// Creates the configured job repository.
static IJobRepository CreateJobRepository(StorageOptions options)
{
    return options.Mode.Equals("JsonFile", StringComparison.OrdinalIgnoreCase)
        ? new JsonJobRepository(options)
        : new InMemoryJobRepository();
}

// Creates the configured event repository.
static IJobEventRepository CreateJobEventRepository(StorageOptions options)
{
    return options.Mode.Equals("JsonFile", StringComparison.OrdinalIgnoreCase)
        ? new JsonJobEventRepository(options)
        : new InMemoryJobEventRepository();
}

// Creates the configured job queue.
static IJobQueue CreateJobQueue(QueueOptions options)
{
    return options.Mode.Equals("File", StringComparison.OrdinalIgnoreCase)
        ? new FileJobQueue(options)
        : new InMemoryJobQueue();
}

// Loads storage options with fallback defaults.
static StorageOptions LoadStorageOptions(IConfiguration configuration)
{
    var options = new StorageOptions();
    var section = configuration.GetSection("Storage");

    options.Mode = section["Mode"] ?? options.Mode;
    options.DataDirectory = section["DataDirectory"] ?? options.DataDirectory;

    if (int.TryParse(section["MaxEventCount"], out var maxEvents))
    {
        options.MaxEventCount = maxEvents;
    }

    return options;
}

// Loads queue options with fallback defaults.
static QueueOptions LoadQueueOptions(IConfiguration configuration)
{
    var options = new QueueOptions();
    var section = configuration.GetSection("Queue");

    options.Mode = section["Mode"] ?? options.Mode;
    options.DataDirectory = section["DataDirectory"] ?? options.DataDirectory;

    if (int.TryParse(section["PollIntervalMs"], out var pollInterval))
    {
        options.PollIntervalMs = pollInterval;
    }

    return options;
}

// Loads stage simulation options with fallback defaults.
static StageSimulationOptions LoadSimulationOptions(IConfiguration configuration)
{
    var options = new StageSimulationOptions();
    var section = configuration.GetSection("StageSimulation");

    if (int.TryParse(section["DefaultMinDelayMs"], out var minDelay))
    {
        options.DefaultMinDelayMs = minDelay;
    }

    if (int.TryParse(section["DefaultMaxDelayMs"], out var maxDelay))
    {
        options.DefaultMaxDelayMs = maxDelay;
    }

    if (double.TryParse(section["DefaultFailureRate"], out var failureRate))
    {
        options.DefaultFailureRate = failureRate;
    }

    return options;
}
