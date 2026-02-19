// 파일: SatelliteProcessing.Client.Web/Program.cs
// 설명: Blazor Web 클라이언트의 진입점입니다. 앱 호스트 및 라우팅을 설정합니다.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
