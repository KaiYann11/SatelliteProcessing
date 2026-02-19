// 파일: SatelliteProcessing.Worker/Program.cs
// 설명: 워커 애플리케이션 진입점입니다. DI 구성 및 서비스 시작을 담당합니다.

using SatelliteProcessing.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
