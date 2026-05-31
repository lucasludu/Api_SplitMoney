using Microsoft.OpenApi.Models;
using System.Reflection;

namespace WebApi.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(variable =>
            {
                // Removido el documento v1 que mostraba todos los endpoints juntos

                // Registrar dinámicamente un documento por cada controlador
                var controllerTypes = typeof(Program).Assembly.GetTypes()
                    .Where(type => typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(type) && !type.IsAbstract);

                foreach (var type in controllerTypes)
                {
                    var controllerName = type.Name.Replace("Controller", "");
                    variable.SwaggerDoc(controllerName, new OpenApiInfo
                    {
                        Title = $"SplitMoney API - {controllerName}",
                        Description = $"Endpoints relacionados con {controllerName}",
                        Version = "v1"
                    });
                }

                // Filtrar endpoints según el documento seleccionado
                variable.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (apiDesc.ActionDescriptor.RouteValues.TryGetValue("controller", out var controllerName))
                    {
                        return string.Equals(controllerName, docName, StringComparison.OrdinalIgnoreCase);
                    }

                    return false;
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                variable.IncludeXmlComments(xmlPath);

                variable.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                variable.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
