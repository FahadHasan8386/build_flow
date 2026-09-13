# 🚀 BuildFlow

**Enterprise-Grade Multi-Tenant SaaS Backend for Project & Task Management**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue)](#-architecture)
[![Pattern](https://img.shields.io/badge/Pattern-CQRS%20%2B%20MediatR-orange)](#-tech-stack)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

BuildFlow is a production-grade **Multi-Tenant SaaS backend** built with **ASP.NET Core Web API**. It follows **Clean Architecture** and **CQRS** to help organizations manage tenants, users, projects, tasks, comments, and notifications — with strict data isolation and fine-grained access control between tenants.

---

## 📖 Table of Contents

- [Key Features](#-key-features)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [API Overview](#-api-overview)
- [Testing](#-testing)
- [Roadmap](#-roadmap)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🔥 Key Features

### 🏢 Multi-Tenancy & Identity Management
- **Tenant Onboarding** — Dynamic registration flow for creating new organizations (tenants).
- **Role-Based Access Control (RBAC)** — Admin/User role authorization enforcing secure boundaries.
- **User Management** — Tenant Owners/Admins can create, update, activate, and deactivate team members.
- **Secure Authentication** — JWT access + refresh token flow for enterprise-grade session security.
- **Tenant Data Isolation** — Every query and command is scoped to the authenticated tenant context.

### 📋 Project & Task Management
- **Project Lifecycle** — Create, update, list, and soft-delete multi-tenant projects.
- **Member Management** — Assign project members and control contextual, project-level access.
- **Task Workflow** — Create tasks, track status and priority, and assign tasks to tenant users.
- **Task Comments** — Collaborative, threaded commenting on tasks.

### 🔔 System Utilities
- **Notifications** — Read/unread tracking for user actions and project updates.
- **Auditable Operations** — Consistent command/query separation for traceable business logic.

---

## 🏗 Architecture

BuildFlow follows **Clean Architecture** principles, keeping business logic independent of frameworks, UI, and databases, combined with **CQRS** (Command Query Responsibility Segregation) via **MediatR** to separate reads from writes.

```
┌─────────────────────────────────────────────┐
│                BuildFlow.Api                 │  Controllers, Middlewares, Swagger
├─────────────────────────────────────────────┤
│            BuildFlow.Application             │  CQRS Handlers, DTOs, Validators
├─────────────────────────────────────────────┤
│            BuildFlow.Infrastructure          │  Dapper Repositories, External Services
├─────────────────────────────────────────────┤
│              BuildFlow.Domain                │  Entities, Interfaces, Domain Logic
├─────────────────────────────────────────────┤
│               BuildFlow.Shared               │  Cross-cutting utilities & constants
└─────────────────────────────────────────────┘
```

**Dependency Rule:** Outer layers depend on inner layers — never the reverse. The `Domain` layer has zero external dependencies, keeping core business rules framework-agnostic and easy to test.

---

## 🛠 Tech Stack

| Layer            | Technology                                  |
|-------------------|----------------------------------------------|
| Framework         | ASP.NET Core 8 Web API                       |
| Language          | C#                                            |
| Architecture      | Clean Architecture (Domain / Application / Infrastructure / API) |
| Design Pattern    | CQRS with MediatR                            |
| Data Access       | Dapper (SQL Server) — optimized, high-performance queries |
| Authentication    | JWT (Access + Refresh Tokens)                |
| API Documentation | Swagger / OpenAPI                            |
| API Testing       | Bruno / Postman                              |

---

## 📂 Project Structure

```
BuildFlow/
├── src/
│   └── BuildFlow/
│       ├── BuildFlow.Domain/          # Core entities, interfaces & domain logic
│       ├── BuildFlow.Application/     # CQRS Commands/Queries, DTOs, Validators
│       ├── BuildFlow.Infrastructure/  # Repositories, Dapper connections & Services
│       ├── BuildFlow.Shared/          # Shared utilities, constants & exceptions
│       └── BuildFlow.Api/             # Controllers, Middlewares & Swagger config
├── back_up/                           # Backup/reference files
├── LICENSE
└── README.md
```

---

## ⚡ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or full instance)
- [Bruno](https://www.usebruno.com/) or [Postman](https://www.postman.com/) for API testing

### Installation

```bash
# 1. Clone the repository
git clone https://github.com/FahadHasan8386/build_flow.git
cd build_flow/src/BuildFlow

# 2. Restore dependencies
dotnet restore

# 3. Update the connection string in appsettings.json (see Configuration below)

# 4. Apply database schema / migrations (if applicable)
# Run the SQL scripts under the Infrastructure project, or your migration tool of choice

# 5. Run the API
dotnet run --project BuildFlow.Api
```

The API will start on `https://localhost:{port}` and Swagger UI will be available at `/swagger`.

---

## ⚙ Configuration

Update `BuildFlow.Api/appsettings.json` (or use environment-specific settings / user secrets) with your own values:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=BuildFlowDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Issuer": "BuildFlow",
    "Audience": "BuildFlowClient",
    "SecretKey": "REPLACE_WITH_A_STRONG_SECRET",
    "AccessTokenExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 7
  }
}
```

> ⚠️ Never commit real secrets. Use `dotnet user-secrets` locally and environment variables / a secrets manager in production.

---

## 📡 API Overview

| Module            | Sample Endpoints                                              |
|--------------------|-----------------------------------------------------------------|
| **Tenants**        | `POST /api/tenants/register`                                   |
| **Auth**           | `POST /api/auth/login` · `POST /api/auth/refresh-token`        |
| **Users**          | `POST /api/users` · `PUT /api/users/{id}` · `PATCH /api/users/{id}/status` |
| **Projects**       | `GET /api/projects` · `POST /api/projects` · `PUT /api/projects/{id}` · `DELETE /api/projects/{id}` |
| **Project Members**| `POST /api/projects/{id}/members` · `DELETE /api/projects/{id}/members/{userId}` |
| **Tasks**          | `POST /api/tasks` · `PUT /api/tasks/{id}/status` · `PUT /api/tasks/{id}/assign` |
| **Comments**       | `POST /api/tasks/{id}/comments` · `GET /api/tasks/{id}/comments` |
| **Notifications**  | `GET /api/notifications` · `PATCH /api/notifications/{id}/read` |

> Full request/response contracts are documented via **Swagger** at runtime, and sample collections are provided for **Bruno/Postman**.

---

## 🧪 Testing

```bash
# Run unit/integration tests (if a test project is present)
dotnet test
```

API collections for manual/exploratory testing can be found under the project's Bruno/Postman collection folder (see repository files).

---

## 🗺 Roadmap

- [ ] Add automated unit & integration test coverage
- [ ] Docker Compose setup for local SQL Server + API
- [ ] CI/CD pipeline (GitHub Actions)
- [ ] Rate limiting & API versioning
- [ ] Real-time notifications via SignalR

---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -m "Add your feature"`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

<p align="center">Built with ❤️ using ASP.NET Core & Clean Architecture</p>
