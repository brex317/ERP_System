# RARAS Employee Management System (EMS)

A full-stack Enterprise Resource Planning (ERP) & Employee Management System built with **Angular 18**, **.NET 8 C# Web API**, and **PostgreSQL 18**.

---

## 🏛 Architecture Overview

```
ERP_System/
├── backend/
│   ├── API/             # Web API Controllers (HelpController, AuthController, DashboardController), Program.cs
│   ├── Application/     # DTOs (HelpDtos), Service Contracts (IHelpService) & Services (HelpService with Caching)
│   ├── Domain/          # Core Domain Entities (Module, Page, Functionality, HelpContext, HelpStep, User, Role)
│   └── Infrastructure/  # Data Access (EmsDbContext, Fluent API), Repository Implementations & DbInitializer
├── database/
│   ├── schemas/
│   │   └── 01_init.sql  # 3NF Database Tables (modules, pages, functionalities, help_contexts, help_steps)
│   └── seed/
│       └── 01_seed_data.sql # 3NF Seed records for ERP modules and initial help steps
└── frontend/
    └── src/app/
        ├── core/        # Angular Services (HelpContextResolverService), Models & Auth Guards
        ├── features/    # Feature Modules (Dashboard, Employees, Departments, Attendance, Leave, Payroll, Auth)
        ├── layout/      # Header, Sidebar, MainLayout
        └── shared/      # Reusable Components (Dynamic Help Popover Widget)
```

---

## 🚀 Getting Started

### 1. Prerequisites
- **PostgreSQL 18+** installed and running on `localhost:5432`
- **.NET 8 SDK** installed (`dotnet --version`)
- **Node.js v18+ & npm** installed (`node -v`)

---

### 2. Database Setup (PostgreSQL)

Execute the initialization and seed scripts using `psql`:

```bash
# 1. Create database
psql -U postgres -c "CREATE DATABASE raras_ems_db;"

# 2. Run table schemas (3NF Normalized)
psql -U postgres -d raras_ems_db -f "database/schemas/01_init.sql"

# 3. Seed data
psql -U postgres -d raras_ems_db -f "database/seed/01_seed_data.sql"
```

*Note: On API startup, `DbInitializer` also automatically migrates existing databases to 3NF schemas and populates default seed data.*

---

### 3. Backend Setup (.NET 8 Web API)

```bash
cd backend

# Restore & Build
dotnet restore
dotnet build

# Run API Server
dotnet run
```

**Swagger Documentation**: Available at `http://localhost:5000/swagger` when running.

---

### 4. Frontend Setup (Angular)

```bash
cd frontend

# Install dependencies
npm install

# Run Development Server (Listening on http://localhost:4200)
npm start
```

---

## 📡 API Endpoint Reference

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/auth/login` | Authenticate user & return token + user profile |
| `GET` | `/api/dashboard/stats` | Fetch live PostgreSQL statistics (`totalEmployees`, `totalDepartments`, `presentToday`, `onLeave`) |
| `GET` | `/api/help` | Query help context hierarchically via `moduleKey`, `pageKey`, `functionalityKey` params |
| `GET` | `/api/help/{moduleKey}/{pageKey}` | Query help context for a specific page level |
| `GET` | `/api/help/{moduleKey}/{pageKey}/{functionalityKey}` | Query help context for a specific functionality level |

---

## ✨ Features Implemented

### 1. Normalized, Route-Driven Help System (3NF)
- **Zero Hardcoded Keys in Components**: All module, page, and functionality keys are defined ONCE in `app.routes.ts` `data: { module, page, functionality }` attributes and resolved automatically at runtime.
- **3NF Relational Database Schema**: Structured into `modules`, `pages`, `functionalities`, `help_contexts`, and `help_steps` with foreign keys and a `CHECK` constraint (`chk_help_context_single_target`).
- **Hierarchical Fallback Resolution**: Queries automatically resolve matching content from **Functionality $\rightarrow$ Page $\rightarrow$ Module $\rightarrow$ Safe Default**.
- **Backend High Performance Caching**: Powered by `IMemoryCache` in `.NET 8` for near-instant responses.
- **Angular Dynamic Context Resolver**: `HelpContextResolverService` inspects the active route snapshot tree on navigation and delivers current keys to page components and `<app-functionality-help>` widgets.
- **Selective Input Overrides**: `<app-functionality-help>` widgets support optional `@Input() functionalityKey` overrides for specific inline widgets while auto-resolving parent page and module keys.

### 2. Authentication & Authorization
- **JWT-Based Authentication**: Secure authentication pipeline with role-based user management (Admin, HR, Manager, Employee).
- **AuthGuard**: Route guard protecting internal system routes (`/dashboard`, `/employees`, `/departments`, `/attendance`, `/leave`, `/payroll`).

### 3. ERP Modules & Live Dashboards
- **Dynamic Stats & PostgreSQL Integration**: Real-time aggregation of active employee counts, attendance status, and leave requests.
- **Modular Angular Architecture**: Clean separation into `core`, `features`, `layout`, and `shared` modules for scalability and easy expansion.
