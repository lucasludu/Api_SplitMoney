# .NET 10 Web API Template - Clean Architecture

Este es un template base para el desarrollo de Web APIs robustas y escalables utilizando **.NET 10**, siguiendo los principios de **Clean Architecture** (Arquitectura Cebolla). El proyecto está diseñado para facilitar la separación de responsabilidades, la testabilidad y el mantenimiento a largo plazo.

## 🚀 Tecnologías Utilizadas

*   **Framework:** [.NET 10.0](https://dotnet.microsoft.com/)
*   **Acceso a Datos:** [Entity Framework Core 10](https://learn.microsoft.com/en-us/ef/core/)
*   **Patrón CQRS:** [MediatR 14](https://github.com/jbogard/MediatR)
*   **Mapeo de Objetos:** [AutoMapper 16](https://automapper.org/)
*   **Validación:** [FluentValidation 12](https://fluentvalidation.net/)
*   **Documentación API:** [Swagger (Swashbuckle)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
*   **Seguridad:** [JWT (JSON Web Token)](https://jwt.io/) & [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
*   **Patrón Specification:** [Ardalis.Specification](https://github.com/ardalis/Specification)
*   **Versionamiento:** [ASP.NET API Versioning](https://github.com/dotnet/aspnet-api-versioning)

## 🏗️ Estructura del Proyecto

El proyecto se divide en 5 capas principales:

### 1. Domain (Dominio)
Contiene las entidades del negocio, constantes y tipos comunes que son independientes de cualquier otra capa.
*   **Entities:** Definición de modelos de base de datos (`ApplicationUser`, `ApplicationRole`).
*   **Common:** Clases base como `BaseEntity`.

### 2. Application (Aplicación)
Contiene la lógica de negocio, interfaces y casos de uso.
*   **Features:** Implementación de CQRS (Commands y Queries) organizados por módulos.
*   **Behaviours:** Middlewares de MediatR para validación automática (`ValidationBehaviour`).
*   **Interfaces:** Definiciones de contratos para servicios y repositorios.
*   **Mappings:** Perfiles de AutoMapper para transformar Entidades a DTOs.
*   **Wrappers:** Clases para estandarizar las respuestas de la API (`Response`, `PagedResponse`).

### 3. Persistence (Persistencia)
Implementación del acceso a datos y configuración de la base de datos.
*   **Contexts:** El `ApplicationDbContext` de EF Core.
*   **Repository:** Implementación genérica del patrón Repositorio.
*   **Seed:** Datos iniciales para la base de datos (Identity roles y usuarios).
*   **Services:** Implementación de servicios de infraestructura como `AuthService`.

### 4. Shared (Compartido)
Servicios e infraestructura común que puede ser utilizada por otras capas pero no contiene lógica de negocio core.
*   **Services:** Implementaciones como `DateTimeService`.

### 5. WebApi (Presentación)
Punto de entrada de la aplicación ASP.NET Core.
*   **Controllers:** Endpoints de la API organizados por versiones (V1).
*   **Middleware:** Manejo global de excepciones (`ErrorHandlerMiddleware`).
*   **Extensions:** Configuración de servicios de forma modular (JWT, Swagger, Mantenimiento).

## ✨ Características Principales

*   ✅ **Clean Architecture:** Separación estricta de responsabilidades.
*   ✅ **CQRS:** Separación de operaciones de lectura y escritura mediante MediatR.
*   ✅ **Validación Automática:** Validaciones de FluentValidation ejecutadas automáticamente antes de procesar cualquier Command.
*   ✅ **Repositorio Genérico:** Soporte para operaciones CRUD y filtrado avanzado mediante Specifications.
*   ✅ **Seguridad Robusta:** Autenticación basada en Roles y JWT pre-configurada.
*   ✅ **Manejo de Errores Global:** Middleware centralizado para transformar excepciones en respuestas JSON consistentes.
*   ✅ **Paginación:** Sistema nativo de respuestas paginadas para listados extensos.
*   ✅ **Seed Data:** Generación automática de roles y usuarios administrativos al iniciar.

## 🛠️ Configuración y Ejecución

1.  **Requisitos:**
    *   [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
    *   SQL Server (o LocalDB)

2.  **Base de Datos:**
    Asegúrate de configurar la cadena de conexión en `WebApi/appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=...;Database=API_Template;..."
    }
    ```

3.  **Ejecutar Migraciones:**
    ```bash
    dotnet ef database update --project Persistence --startup-project WebApi
    ```

4.  **Iniciar Aplicación:**
    ```bash
    dotnet run --project WebApi
    ```

5.  **Swagger:**
    Una vez iniciada, accede a `https://localhost:[puerto]/swagger` para visualizar la documentación interactiva.

---
🚀 *Desarrollado como template base profesional para proyectos .NET 10.*
