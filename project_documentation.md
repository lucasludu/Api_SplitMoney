# Project Documentation for NotebookLM: SplitMoney API

## 📌 Project Overview
**SplitMoney** is a robust, scalable backend for an expense-sharing application (similar to Splitwise) built using **.NET 10** and following **Onion Architecture** (Clean Architecture) principles. The primary goal is to provide a clean service to track expenses, manage groups, and simplify debts between friends or members.

## 🏗️ Architecture: Onion Architecture
The project is strictly organized into layers to ensure separation of concerns, testability, and maintainability:

1.  **Domain (Core)**: 
    - Contains business entities, constants, and types.
    - Completely independent of other layers and frameworks.
    - Includes entities like `Expense`, `Group`, `ApplicationUser`, and `Settlement`.

2.  **Application (Business Logic)**:
    - Implements business cases using the **CQRS (Command Query Responsibility Segregation)** pattern with **MediatR**.
    - Handles data validation via **FluentValidation**.
    - Uses **AutoMapper** for DTO (Data Transfer Object) transformations.
    - Defines interfaces for external services and repositories.

3.  **Persistence (Infrastructure)**:
    - Implements data access using **Entity Framework Core 10**.
    - Uses **SQLite** for development (with decimal-to-double converters for compatibility).
    - Contains the database context (`ApplicationDbContext`), repository implementations, and data seeding.
    - Manages Identity (Users and Roles).

4.  **WebApi (Presentation)**:
    - The entry point of the application.
    - Contains controllers organized by versions (v1).
    - Configures **Swagger** for interactive API documentation.
    - Implements global error handling via middleware.
    - Handles **JWT (JSON Web Token)** authentication and authorization.

5.  **Shared**:
    - Infrastructure services common to all layers (e.g., `DateTimeService`).

## 📊 Domain Model (Entities)

| Entity | Description |
| :--- | :--- |
| **ApplicationUser** | Extends ASP.NET Identity User. Represents a person in the system. |
| **Group** | A collection of users who share expenses. |
| **GroupMember** | Junction table for Users and Groups, managing membership and roles. |
| **Expense** | Represents a financial transaction within a group or between users. |
| **ExpenseSplit** | Details how an expense is divided among multiple users. |
| **Balance** | Tracks the net debt/credit status for a user within a group. |
| **Settlement** | Records a payment made to resolve an outstanding debt. |
| **Category** | Categorizes expenses (e.g., Food, Travel, Rent). |

## 🛠️ Technology Stack

- **Framework:** .NET 10.0
- **Database:** SQLite (Development) / EF Core 10
- **Patterns:** CQRS, Onion Architecture, Repository Pattern, Specification Pattern.
- **Security:** ASP.NET Core Identity & JWT.
- **API Documentation:** Swagger (Swashbuckle).
- **Validation:** FluentValidation 12.
- **Messaging/Mediation:** MediatR 14.
- **Mapping:** AutoMapper 16.

## ✨ Current Features Implemented

1.  **Authentication & Authorization:**
    - User registration and login.
    - JWT token generation and validation.
    - Role-based access control (Admin, User).
2.  **Expense Management:**
    - Initial commands for creating expenses (`CreateExpenseCommand`).
    - Basic splitting logic.
3.  **Database Configuration:**
    - Fully configured EF Core context.
    - Automatic creation of audit fields (`Created`, `LastModified`) using `BaseEntity`.
    - SQLite integration with necessary value converters.
4.  **Modular Service Registration:**
    - Dependency injection organized via `ServiceExtensions` in each layer.

## 🚀 Execution & Developer Workflow

1.  **Database Migrations:** Managed through EF Core tools.
2.  **Interactive API:** Accessible via `/swagger` once the app is running.
3.  **Global Exception Handling:** Ensures consistent JSON responses in case of errors.
4.  **Automatic Seeding:** Pre-populates default roles and an admin user.

---
*Document generated for the purpose of ingestion into NotebookLM.*
