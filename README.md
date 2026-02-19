# SatelliteProcessing

[![CI](https://github.com/KaiYann11/SatelliteProcessing/actions/workflows/ci.yml/badge.svg)](https://github.com/KaiYann11/SatelliteProcessing/actions)

Small, modular satellite-job processing sample implemented in .NET. The solution demonstrates a staged processing pipeline, pluggable persistence/queue adapters, and a minimal API + worker pair.

## Repository layout
- `SatelliteProcessing.Application` — application core, engine interfaces and services
- `SatelliteProcessing.Contracts` — DTOs and API contract types
- `SatelliteProcessing.Domain` — domain models and enums
- `SatelliteProcessing.Infrastructure` — file/in-memory adapters for queues, repositories, storage, and time
- `SatelliteProcessing.Server.Api` — HTTP API for job creation and status
- `SatelliteProcessing.Worker` — background worker that executes staged processing
- `SatelliteProcessing.Client.Web` / `Client.Wpf` — example client UIs

## Quick start (Windows)

Prerequisites: .NET SDK (6.0 or later) installed.

1. Restore and build the solution:

```powershell
dotnet restore
dotnet build SatelliteProcessing.sln --configuration Debug
```

2. Run the API for local testing:

```powershell
cd SatelliteProcessing.Server.Api
dotnet run
```

3. In another terminal run the worker:

```powershell
cd SatelliteProcessing.Worker
dotnet run
```

4. Create a job via the client or POST to the API endpoint described in `Server.Api` (see `appsettings.json` for port).

## Development notes
- The processing pipeline is orchestrated by `IJobProcessingEngine` and executed in stages via `IStageProcessor` implementations. See `SatelliteProcessing.Application` for engine details.
- Default infrastructure implementations are file/JSON and in-memory for easy local development. Replace via DI to use cloud providers.
- Add new stage processors by implementing `IStageProcessor` and registering it with DI.

## CI
This repository includes a GitHub Actions workflow that restores dependencies and builds the solution. It will run on every push and PR to `main`.

## Contributing
- Open an issue or PR. Keep changes focused and include tests where applicable.

## License
Specify your license or add a `LICENSE` file.
