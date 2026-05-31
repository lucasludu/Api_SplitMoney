using Application;
using Persistence;
using Persistence.Seed;
using Shared;
using WebApi.Extensions;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfraestructure(builder.Configuration);
builder.Services.AddSharedInfraestructure(builder.Configuration);
builder.Services.AddApiVersioningExtension();
builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
    {
        p.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

builder.Services.PostConfigure<Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions>(options =>
{
    options.ConfigObject.Urls = null; // Limpiar cualquier configuración previa (ej. de ApiExplorer)
    
    var controllerTypes = typeof(Program).Assembly.GetTypes()
        .Where(type => typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(type) && !type.IsAbstract);

    foreach (var type in controllerTypes)
    {
        var controllerName = type.Name.Replace("Controller", "");
        options.SwaggerEndpoint($"/swagger/{controllerName}/swagger.json", controllerName);
    }
});

var app = builder.Build();

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeed.SeedAsync(services);
    await CategorySeed.SeedAsync(services);
}

// Middleware para redirigir "/" a "/api-docs" sin generar un endpoint visible en Swagger
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/api-docs");
        return;
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(variable =>
    {
        variable.RoutePrefix = "api-docs";
        variable.DefaultModelsExpandDepth(-1);
    });
}
// app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
