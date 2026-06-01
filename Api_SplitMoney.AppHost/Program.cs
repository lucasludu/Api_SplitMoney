var builder = DistributedApplication.CreateBuilder(args);

var webapi = builder.AddProject<Projects.WebApi>("webapi");

builder.AddExecutable("frontend", "dotnet", "c:\\Users\\Usuario\\source\\repos\\lucasludu\\View_SplitMoney", "run", "--project", "SplitMoney.Client.csproj", "-f", "net10.0-windows10.0.19041.0")
    .WithReference(webapi);

builder.Build().Run();
