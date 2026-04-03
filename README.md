# 💸 SplitMoney API - SaaS Finance Engine
**Plataforma profesional para la gestión y simplificación de gastos compartidos.**

SplitMoney es una Web API robusta construida con **.NET 10** y **Arquitectura Onion**, diseñada para resolver el problema de "quién le debe a quién" en viajes, cenas y gastos de convivencia.

---

## ✨ Características Principales

### 🧠 Motor de Gastos (Splits)
- **Distribución Equitativa**: Divide cuentas automáticamente entre todos los participantes.
- **Montos Exactos**: Asigna deudas específicas por usuario (ej: consumo en bar).
- **Porcentajes**: Distribuye gastos basados en ratios personalizados (ej: alquiler 60/40).

### 🛡️ Sistema Premium & Auditoría
- **Modelo Freemium**: Control de acceso basado en roles (`FreeUser` vs `PremiumUser`).
- **Límites Dinámicos**: Los usuarios gratuitos están limitados a un máximo de 3 grupos activos y 5 miembros por grupo.
- **Auditoría de Gastos (VIP)**: Trazabilidad completa de cambios. Los usuarios Premium pueden ver quién editó un monto o título, qué valor tenía antes y cuándo se realizó el cambio.
- **Suscripciones**: Sistema de upgrade a premium integrado vía API.

### 📊 BalanceEngine (Simplificación de Deudas)
- Cálculo en tiempo real de deudas netas para minimizar el número de transferencias necesarias entre participantes.

---

## 🏗️ Arquitectura del Proyecto (Onion Clean Architecture)

1. **Domain**: Entidades puras y lógica de negocio central (Group, Expense, Settlement, ExpenseAudit). Independiente de frameworks.
2. **Application**: Casos de uso implementados con el patrón **CQRS (MediatR)**, validaciones automáticas con FluentValidation y mapeos de datos.
3. **Persistence**: Acceso a datos con **Entity Framework Core**, implementación del patrón Repository/UoW y gestión de Identity.
4. **WebApi**: Endpoints RESTful versionados (V1) con seguridad JWT y documentación Swagger.

---

## 🛠️ Configuración y Ejecución

1. **Clonar Proyecto**:
   ```bash
   git clone https://github.com/lucasludu/Api_SplitMoney.git
   ```

2. **Base de Datos**:
   Configura la cadena de conexión en `WebApi/appsettings.json`. El proyecto soporta **SQLite** (por defecto para desarrollo) y **SQL Server**.

3. **Ejecutar Migraciones**:
   ```bash
   dotnet ef database update --project Persistence --startup-project WebApi
   ```

4. **Documentación Swagger**:
   Accede a `https://localhost:[puerto]/swagger` para probar los endpoints interactivos.

---

## 🚀 Tecnologías
- **Framework**: [.NET 10.0](https://dotnet.microsoft.com/)
- **Patrón CQRS**: [MediatR](https://github.com/jbogard/MediatR)
- **Seguridad**: JWT (JSON Web Token) & [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- **Acceso a Datos**: [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- **Validación**: [FluentValidation](https://fluentvalidation.net/)

---
🚀 *Desarrollado como una solución financiera escalable y transparente.* 
