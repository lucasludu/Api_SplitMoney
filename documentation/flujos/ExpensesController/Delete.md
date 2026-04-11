# ANÁLISIS TÉCNICO: PROCESO DE ELIMINACIÓN DE GASTO

## LÓGICA DE EJECUCIÓN (EXPENSESCONTROLLER.DELETE)

| Fase | Componente | Acción |
| :--- | :--- | :--- |
| **Entrada** | Web API | Recibe `Guid id` mediante el segmento de ruta. |
| **Seguridad** | Filtro de Autorización | Valida el token JWT del usuario (Atributo `[Authorize]`). |
| **Comando** | Application Layer | Instancia un objeto `DeleteExpenseCommand` con el ID proporcionado. |
| **Mediación** | MediatR | Despacha el comando al manejador (handler) correspondiente. |
| **Persistencia** | Repositorio/DB | El handler ejecuta la eliminación lógica o física en la base de datos. |
| **Salida** | Controller | Retorna una respuesta HTTP 200 (OK) con el resultado del comando. |

## DIAGRAMA DE FLUJO DE EJECUCIÓN

```mermaid
flowchart TD
    Start((Inicio)) -->|HTTP DELETE /api/v1/expenses/{id}| Auth{"¿Token Válido?"}
    
    Auth -->|No| Res401((Respuesta 401 Unauthorized))
    Auth -->|Sí| Binding["Validación de Tipo: Guid id"]
    
    Binding -->|ID Inválido| Res400((Respuesta 400 Bad Request))
    Binding -->|ID Válido| CmdInst["Instanciar DeleteExpenseCommand(id)"]
    
    CmdInst --> SendMediator["Mediator.Send(command)"]
    
    SendMediator --> Pipeline["Ejecución de Pipeline Behaviors (Validación/Logging)"]
    
    Pipeline --> Handler["DeleteExpenseCommandHandler"]
    
    Handler --> DBCheck{"¿Registro Existe?"}
    
    DBCheck -->|No| Res404((Respuesta 404 Not Found))
    
    DBCheck -->|Sí| DBExec[(Base de Datos: Soft/Hard Delete)]
    
    DBExec --> Success["Retornar Result Success"]
    
    Success --> Res200((Respuesta 200 OK))
    
    %% Manejo de Excepciones Globales
    Handler -.->|Excepción| GlobalError["Global Exception Handler"]
    GlobalError --> Res500((Respuesta 500 Error Interno))
```

## EXPLICACIÓN DEL FLUJO

1.  **Interceptación de Petición**: El flujo comienza cuando el cliente envía un `DELETE` al endpoint. El middleware de ASP.NET Core valida la autenticación del usuario antes de llegar al controlador.
2.  **Encapsulamiento (CQRS)**: El controlador no contiene lógica de negocio; su única responsabilidad es transformar el parámetro `id` de la URL en un objeto `DeleteExpenseCommand` y delegar la ejecución a **MediatR**.
3.  **Procesamiento Descentralizado**:
    *   **Pipeline Behaviors**: Antes de llegar al manejador, se pueden ejecutar validaciones automáticas (ej. FluentValidation).
    *   **Handler**: Es donde reside la lógica para verificar si el gasto existe en la **Base de Datos** y si el usuario tiene permisos para borrarlo.
4.  **Respuesta**: El resultado se propaga de vuelta hacia el controlador, el cual lo envuelve en un método `Ok()`, resultando en un código de estado HTTP 200 si la operación fue exitosa.