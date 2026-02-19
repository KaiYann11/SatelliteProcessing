# SatelliteProcessing

Small sample satellite-processing pipeline implemented in .NET (multiple projects).

## What this repo contains
- `SatelliteProcessing.Application` - application core and services
- `SatelliteProcessing.Contracts` - DTOs and API contracts
- `SatelliteProcessing.Domain` - domain models
- `SatelliteProcessing.Infrastructure` - storage, queues, repositories
- `SatelliteProcessing.Server.Api` - HTTP API
- `SatelliteProcessing.Worker` - background worker process
- `SatelliteProcessing.Client.Web` / `Client.Wpf` - example clients

## Quick start (Windows)
1. Ensure .NET SDK installed (6/7+). Build solution:
```powershell
dotnet build SatelliteProcessing.sln
```
2. Run API for local testing:
```powershell
cd SatelliteProcessing.Server.Api
dotnet run
```
3. Run worker in another terminal:
```powershell
cd SatelliteProcessing.Worker
dotnet run
```

## Development notes
- Projects are class-library and console/web templates; edit and run via Visual Studio or `dotnet` CLI.
- See `DESIGN.md` for architecture and processing flow.

## Contributing
PRs welcome. Keep changes focused and include tests where applicable.
