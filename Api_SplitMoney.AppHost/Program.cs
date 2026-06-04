var builder = DistributedApplication.CreateBuilder(args);

var webapi = builder.AddProject<Projects.WebApi>("webapi");

var targetFramework = Environment.GetEnvironmentVariable("MAUI_TARGET_FRAMEWORK") ?? "net10.0-windows10.0.19041.0";

builder.AddExecutable("frontend", "dotnet", "c:\\Users\\Usuario\\source\\repos\\lucasludu\\View_SplitMoney", "run", "--project", "SplitMoney.Client.csproj", "-f", targetFramework)
    .WithReference(webapi);

builder.Build().Run();
