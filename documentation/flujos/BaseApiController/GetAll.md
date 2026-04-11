# ANÁLISIS TÉCNICO: MÉTODO GETALL EN BASEAPICONTROLLER

El método `GetAll` en un controlador que hereda de `BaseApiController` utiliza el patrón **Mediator** para desacoplar la capa de transporte de la lógica de negocio. La resolución del servicio se realiza mediante *Lazy Loading* a través del contenedor de dependencias de ASP.NET Core.

### Diagrama de Flujo de Ejecución (Mermaid)

```mermaid
flowchart TD
    Start(("Inicio: Llamada a GetAll")) --> Request["Recibir Solicitud HTTP GET"]
    Request --> MedCheck{"¿_mediator es nulo?"}
    
    MedCheck -->|Sí| Resolve["Obtener IMediator de HttpContext.RequestServices"]
    MedCheck -->|No| CreateQuery["Instanciar GetAllQuery/Request"]
    
    Resolve --> CreateQuery
    
    CreateQuery --> MedSend["Llamar Mediator.Send(query)"]
    
    subgraph MediatR["Pipeline de MediatR"]
        direction TB
        Behaviors["Ejecutar Behaviors (Validación, Logging, Caching)"]
        Handler["Invocar QueryHandler correspondiente"]
        Behaviors --> Handler
    end
    
    MedSend --> MediatR
    
    MediatR --> DBQuery[(Acceso a Base de Datos)]
    DBQuery --> TryCatch{¿Error en ejecución?}
    
    TryCatch -->|No| MapDTO["Mapear Entidades a DTOs"]
    TryCatch -->|Sí| HandleExc["Capturar Excepción / Manejar Error"]
    
    MapDTO --> ResultCheck{"¿Existen registros?"}
    
    ResultCheck -->|Sí| Response200["Retornar Ok(Data)"]
    ResultCheck -->|No| Response404["Retornar NotFound o Lista Vacía"]
    
    HandleExc --> ResponseErr["Retornar BadRequest o StatusCode 500"]
    
    Response200 --> End(("Fin"))
    Response404 --> End
    ResponseErr --> End
```

### Explicación de la Lógica de Ejecución

| Fase | Descripción Técnica |
| :--- | :--- |
| **Resolución de Dependencias** | El `BaseApiController` utiliza el patrón de inyección de servicios por propiedad con evaluación perezosa. Si `_mediator` es nulo, se resuelve desde el `IServiceProvider` de la solicitud actual. |
| **Desacoplamiento** | El controlador no conoce la lógica de acceso a datos. Solo crea un objeto `Query` y lo envía al bus de mensajes (`MediatR`). |
| **Pipeline de MediatR** | Antes de llegar al manejador, la solicitud pasa por *Behaviors* que pueden realizar validaciones automáticas o gestión de logs. |
| **Manejo de Resultados** | La respuesta se encapsula usualmente en un objeto de resultado (como `Result<T>`) para estandarizar las respuestas HTTP (200, 400, 404) y la estructura de errores. |
| **Persistencia** | El `QueryHandler` interactúa con el `DbContext` o Repositorios para materializar la consulta, aplicando filtros de solo lectura (`AsNoTracking`). |