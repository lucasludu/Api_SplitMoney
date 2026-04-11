# ANÁLISIS TÉCNICO: AuthController.Register

El método `Register` actúa como un punto de entrada (Entry Point) que delega la responsabilidad de la lógica de negocio a un manejador de comandos mediante el patrón **Mediator**. A continuación, se detalla el flujo de ejecución desde la recepción de la solicitud hasta la respuesta final.

## Diagrama de Flujo de Ejecución

```mermaid
flowchart TD
    Start(("Inicio")) --> P1["Recibir HTTP POST /register"]
    P1 --> P2["Validar deserialización de 'RegisterUserRequest'"]
    
    P2 --> P3["Instanciar 'RegisterUserCommand'"]
    P3 --> P4["Mediator.Send(command)"]
    
    subgraph Capa_Aplicacion ["Capa de Aplicación (MediatR Handler)"]
        H1["Ejecutar 'RegisterUserCommandHandler'"]
        H1 --> V1{"¿Datos válidos?"}
        V1 -->|No| E1["Generar error de validación"]
        V1 -->|Sí| DB1[("Verificar existencia / Guardar en DB")]
        DB1 --> V2{"¿Operación exitosa?"}
        V2 -->|No| E2["Capturar excepción o error de dominio"]
        V2 -->|Sí| S1["Generar resultado exitoso"]
    end

    P4 --> Result{"¿result.Succeeded?"}
    
    E1 --> Result
    E2 --> Result
    S1 --> Result

    Result -->|No| R400["Retornar 400 'BadRequest(result)'"]
    Result -->|Sí| R200["Retornar 200 'OK(result)'"]
    
    R400 --> Fin(("Fin"))
    R200 --> Fin
```

## Análisis de la Lógica

1.  **Recepción de Datos**: El controlador recibe un objeto `RegisterUserRequest`. La infraestructura de ASP.NET Core se encarga de la validación básica del formato (JSON).
2.  **Desacoplamiento**: Se utiliza `IMediator` para enviar un `RegisterUserCommand`. Esto separa la capa de transporte (Web API) de la capa de servicios/lógica de negocio.
3.  **Manejo de Respuesta**: El resultado (`result`) encapsula tanto el éxito como el fallo de la operación.
4.  **Flujos de Error**: 
    *   Si la propiedad `Succeeded` es `false`, se asume que hubo errores de validación, colisiones de datos (ej. email duplicado) o fallos de infraestructura, devolviendo un código de estado **400**.
5.  **Flujo de Éxito**: 
    *   Si la operación en la base de datos es confirmada, el sistema devuelve un código **200** junto con el objeto de respuesta definido en el comando.

### Tabla de Componentes

| Componente | Responsabilidad |
| :--- | :--- |
| `RegisterUserRequest` | DTO que transporta los datos de entrada del cliente. |
| `RegisterUserCommand` | Representación de la intención de registro dentro del dominio de la aplicación. |
| `Mediator` | Bus de mensajes interno que localiza el Handler correspondiente. |
| `Succeeded` | Propiedad booleana que determina el flujo de la respuesta HTTP. |