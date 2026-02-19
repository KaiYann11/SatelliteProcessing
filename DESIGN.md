# Design & Architecture

## Overview
This repository implements a small, modular satellite-job processing system. Jobs are created via API or client, enqueued, processed by staged processors in the Worker, and recorded in persistent storage.

## High-level components
- `Server.Api`: Receives `CreateJob` requests, validates and enqueues jobs.
- `Worker`: Background service that dequeues jobs and runs stage processors.
- `Application`: Core orchestration, engine interfaces (`IJobProcessingEngine`, `IStageProcessor`), and services.
- `Infrastructure`: Implementations for queues, repositories, storage adapters, and time provider.
- `Contracts` / `Domain`: DTOs and domain models representing jobs, stages, events, and statuses.

## Job processing flow
1. Client/API sends `CreateJobRequest` → `Server.Api`.
2. `Server.Api` invokes `IJobService.CreateJob()` in `Application`.
3. `Application` persists initial job record and enqueues job via `IJobQueue`.
4. `Worker` dequeues job and uses `JobProcessingEngine` (implements `IJobProcessingEngine`) to execute processing stages sequentially.
5. Each stage runs via `IStageProcessor.Process(StageProcessingContext)` and returns `StageProcessingResult`.
6. Results and `JobEvent` records are saved to the `IJobEventRepository` and `IJobRepository` to update job status.

## Storage and queue
- Default implementations are file/JSON-based and in-memory for tests.
- Replace with cloud-backed implementations by providing new implementations of `IJobQueue`, `IJobRepository`, and `IJobEventRepository`.

## Extensibility
- Add new stage processors by implementing `IStageProcessor` and registering them in DI.
- Swap queue/storage adapters via DI and configuration (`appsettings.json`).

## Observability
- Events stored in `JobEvent` store enable audit and replay.
- Add metrics and logging in `JobProcessingEngine` and `Worker` for latency and failure rates.

## Deployment
- `Server.Api` and `Worker` can be containerized. Provide environment variables for storage and queue endpoints.

## Local development
- Use `dotnet run` for `Server.Api` and `Worker` locally. Use in-memory queue and file-based storage for quick iteration.
