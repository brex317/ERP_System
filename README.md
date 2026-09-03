# RARAS Employee Management System (EMS)

A full-stack Enterprise Resource Planning (ERP) & Employee Management System built with **Angular 18**, **.NET 8 C# Web API**, and **PostgreSQL 18**.

---

## 🏛 Architecture Overview

```
ERP_System_For_Raras_Technologies/
├── backend/
│   └── src/
│       ├── Raras.EMS.API/           # ASP.NET Core Web API (Controllers, Swagger, CORS)
│       ├── Raras.EMS.Application/   # Business Logic & CQRS Handlers
│       ├── Raras.EMS.Domain/        # Domain Entities & Interfaces
│       ├── Raras.EMS.Infrastructure/# EF Core DbContext & PostgreSQL Repositories
│       └── Raras.EMS.Shared/        # Shared DTOs & Constants
├── database/
│   ├── schemas/
│   │   └── 01_init.sql              # Database Tables (departments, employees, attendance, leave)
│   └── seed/
│       └── 01_seed_data.sql         # Seed records for dynamic stats
└── frontend/
    └── raras-ems-web/
        └── src/app/
            ├── core/                # Angular Services, Models & Auth Guards
            │   ├── models/          # TypeScript Interfaces (User, DashboardStats)
            │   ├── services/        # AuthService & DashboardService
            │   └── guards/          # AuthGuard protecting authenticated routes
            ├── features/            # Feature Components
            │   ├── auth/login/      # LoginComponent (Form, Validation, Styling)
            │   └── dashboard/       # DashboardComponent (Stat Cards, PostgreSQL Binding)
            ├── layout/              # App Layout Components
            │   ├── header/          # HeaderComponent (Profile, Notifications, Search)
            │   ├── sidebar/         # SidebarComponent (Module Navigation)
            │   └── main-layout/     # MainLayoutComponent (Responsive Container)
            └── shared/              # Reusable UI Elements
                └── components/
                    └── functionality-help/ # Help Popover Dropdown (ⓘ Need help?)
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

# 2. Run table schemas
psql -U postgres -d raras_ems_db -f "database/schemas/01_init.sql"

# 3. Seed data
psql -U postgres -d raras_ems_db -f "database/seed/01_seed_data.sql"
```

---

### 3. Backend Setup (.NET 8 Web API)

```bash
cd backend/src/Raras.EMS.API

# Restore & Build
dotnet build

# Run API Server (Listening on http://localhost:5000)
dotnet run --launch-profile http
```

**Swagger Documentation**: Available at `http://localhost:5000/swagger` when running.

---

### 4. Frontend Setup (Angular)

```bash
cd frontend/raras-ems-web

# Install dependencies
npm install

# Run Development Server (Listening on http://localhost:4200)
npm start
```

---

## 📡 API Endpoint Reference

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/auth/login` | Authenticate user & return token + profile (`Berihu`, `BE`) |
| `GET` | `/api/dashboard/stats` | Fetch live PostgreSQL statistics (`totalEmployees`, `totalDepartments`, `presentToday`, `onLeave`) |

---

## ✨ Features Implemented

1. **Authentication & Security**:
   - **Login Component**: Styled Angular form with validation and session management.
   - **AuthGuard**: Protects `/dashboard` routes from unauthorized access.
2. **Dynamic Dashboard**:
   - **Stat Cards**: Displays live numbers (`Total Employees: 248`, `Departments: 12`, `Present Today: 221`, `On Leave: 18`) directly queried from PostgreSQL tables via EF Core.
   - **Contextual Help Popover (`ⓘ Need help?`)**: Reusable component offering quick step-by-step guidance on hover/focus.
3. **Modular Angular Architecture**:
   - Organized cleanly into `core`, `features`, `layout`, and `shared` modules.
