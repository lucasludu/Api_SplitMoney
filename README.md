# 💸 SplitMoney API - SaaS Finance Engine

**SplitMoney** es la infraestructura backend definitiva para la gestión compartida de gastos. Diseñada bajo un modelo **SaaS Freemium**, permite a grupos de amigos, convivientes y viajeros llevar un control exacto de sus deudas, simplificando la convivencia financiera.

---

## 🚀 ¿Qué es SplitMoney?
El proyecto es una Web API de alto rendimiento construida con **.NET 10** que resuelve el complejo problema de la distribución de deudas en grupos. No se trata solo de anotar gastos; es un motor lógico que calcula balances netos y ofrece transparencia total sobre los movimientos financieros.

### 🌟 Funcionalidades Principales

#### 🧠 Motor de Repartos (Split Engine)
Permite distribuir un gasto de tres formas distintas para adaptarse a cualquier situación real:
- **Equitativo**: El sistema divide el total automáticamente entre todos (o algunos) miembros.
- **Monto Exacto**: Asignación manual de deudas (ideal para cuando alguien consume un plato más caro en una cena).
- **Porcentual**: Distribución basada en ratios (ej: 70/30 para gastos de alquiler).

#### 📊 BalanceEngine (Simplificación de Deudas)
Calcula en tiempo real el estado de cuenta de cada usuario dentro de un grupo:
- ¿Cuánto puse?
- ¿Cuánto me deben?
- ¿A quién tengo que pagarle para quedar a mano?

#### 🛒 Liquidación de Deudas (Settlements)
Permite registrar pagos directos entre miembros para "saldar" las deudas pendientes, actualizando el balance del grupo de forma instantánea.

#### 🛡️ Modelo Freemium & Premium
El proyecto incluye una lógica de suscripción integrada:
- **Usuarios Free**: Límites dinámicos de creación de grupos (máx. 3) y miembros por grupo (máx. 5).
- **Usuarios Premium**: Acceso a grupos ilimitados, categorías personalizadas y el exclusivo **Registro de Auditoría**.

#### 🕵️ Registro de Auditoría (Transparency Logs)
Exclusivo para usuarios Premium. Permite ver el historial completo de un gasto: si alguien cambió el monto o el título, el sistema registra el valor anterior, el nuevo, el autor del cambio y la fecha exacta.

---

## 🏗️ Arquitectura y Calidad de Código
El proyecto implementa los estándares más altos de la industria:

- **Clean Architecture (Onion)**: Separación total de intereses para facilitar el mantenimiento y escalabilidad.
- **Patrón CQRS (MediatR)**: Desacoplamiento de las operaciones de lectura (Queries) y escritura (Commands).
- **Ardalis.Specification**: Lógica de acceso a datos encapsulada y reutilizable, evitando la fuga de lógica de Entity Framework hacia las capas superiores.
- **Validación Automática**: Uso de `FluentValidation` para asegurar la integridad de los datos antes de procesar cualquier comando.
- **Identity & Seguridad**: Autenticación basada en Roles y JWT con soporte para tokens de acceso y refresco.

## 🏛️ Arquitectura del Proyecto (Onion Clean Architecture)

1. **Domain**: Entidades puras y lógica de negocio central (Group, Expense, Settlement, ExpenseAudit). Independiente de frameworks.
2. **Application**: Casos de uso implementados con el patrón **CQRS (MediatR)**, validaciones automáticas con FluentValidation y mapeos de datos.
3. **Persistence**: Acceso a datos con **Entity Framework Core**, implementación del patrón Repository/UoW y gestión de Identity.
4. **WebApi**: Endpoints RESTful versionados (V1) con seguridad JWT y documentación Swagger.

> [!TIP]
> **¿Querés ver cómo funciona por dentro?**
> Mirá nuestra **[Guía de Arquitectura y Flujos Técnicos (Diagramas Mermaid)](./ARCHITECTURE.md)** para entender el viaje de los datos en la API. ✨📄

---

## 🛠️ Stack Tecnológico
- **Core**: .NET 10 (C#)
- **Base de Datos**: EF Core 10 (Provider flexible: SQLite / SQL Server)
- **Mediador**: MediatR 14
- **Seguridad**: ASP.NET Core Identity + JWT
- **Documentación**: Swagger / OpenAPI
- **Mapeos**: AutoMapper

---

## ⚙️ Configuración Rápida

1. **Requisitos**: SDK de .NET 10.
2. **Instalación**:
   ```bash
   git clone https://github.com/lucasludu/Api_SplitMoney.git
   cd Api_SplitMoney
   ```
3. **Persistencia**:
   Ejecuta las migraciones para crear la base de datos local:
   ```bash
   dotnet ef database update --project Persistence --startup-project WebApi
   ```
4. **Ejecución**:
   ```bash
   dotnet run --project WebApi
   ```

---

## 🌐 Endpoints Clave
- `POST /api/v1/Account/authenticate`: Inicio de sesión y obtención de Token.
- `GET /api/v1/Groups`: Listado de grupos del usuario con sus balances.
- `POST /api/v1/Expenses`: Creación de gastos con lógicas de reparto.
- `POST /api/v1/Groups/settle`: Registro de pagos de deudas.
- `GET /api/v1/Expenses/{id}/audit`: Historial de cambios de un gasto (Premium).

---
🚀 *SplitMoney API: Diseñado por desarrolladores para un mundo donde el dinero compartido ya no es un dolor de cabeza.*
