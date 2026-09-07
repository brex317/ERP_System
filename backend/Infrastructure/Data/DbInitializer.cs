using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Models.Entities;
using Raras.EMS.API.Services;

namespace Raras.EMS.API.Data;

public static class DbInitializer
{
    public static void Initialize(EmsDbContext db, IPasswordHasher passwordHasher)
    {
        try
        {
            EnsureTablesCreated(db);
            SeedRolesAndAdminUser(db, passwordHasher);
            SeedHelpData(db);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DbInitializer Warning] Database initialization failed: {ex.Message}");
        }
    }

    private static void EnsureTablesCreated(EmsDbContext db)
    {
        db.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS roles (
                id SERIAL PRIMARY KEY,
                name VARCHAR(50) NOT NULL UNIQUE,
                description TEXT,
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS users (
                id SERIAL PRIMARY KEY,
                email VARCHAR(100) NOT NULL UNIQUE,
                username VARCHAR(100) NOT NULL UNIQUE,
                password_hash VARCHAR(255) NOT NULL,
                first_name VARCHAR(50) NOT NULL,
                last_name VARCHAR(50) NOT NULL,
                role_id INT NOT NULL REFERENCES roles(id) ON DELETE RESTRICT,
                is_active BOOLEAN NOT NULL DEFAULT TRUE,
                employee_id INT REFERENCES employees(id) ON DELETE SET NULL,
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS modules (
                id SERIAL PRIMARY KEY,
                key VARCHAR(100) NOT NULL UNIQUE,
                display_name VARCHAR(150) NOT NULL,
                icon VARCHAR(50),
                sort_order INT NOT NULL DEFAULT 0,
                is_active BOOLEAN NOT NULL DEFAULT TRUE
            );

            CREATE TABLE IF NOT EXISTS pages (
                id SERIAL PRIMARY KEY,
                module_id INT NOT NULL REFERENCES modules(id) ON DELETE CASCADE,
                key VARCHAR(100) NOT NULL,
                display_name VARCHAR(150) NOT NULL,
                route_path VARCHAR(255),
                sort_order INT NOT NULL DEFAULT 0,
                CONSTRAINT unique_module_page UNIQUE(module_id, key)
            );

            CREATE TABLE IF NOT EXISTS functionalities (
                id SERIAL PRIMARY KEY,
                page_id INT NOT NULL REFERENCES pages(id) ON DELETE CASCADE,
                key VARCHAR(100) NOT NULL,
                display_name VARCHAR(150) NOT NULL,
                CONSTRAINT unique_page_functionality UNIQUE(page_id, key)
            );

            CREATE TABLE IF NOT EXISTS notifications (
                id SERIAL PRIMARY KEY,
                user_id INT REFERENCES users(id) ON DELETE CASCADE,
                title VARCHAR(150) NOT NULL,
                message TEXT NOT NULL,
                type VARCHAR(50) NOT NULL DEFAULT 'info',
                is_read BOOLEAN NOT NULL DEFAULT FALSE,
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS support_tickets (
                id SERIAL PRIMARY KEY,
                user_id INT REFERENCES users(id) ON DELETE CASCADE,
                subject VARCHAR(200) NOT NULL,
                category VARCHAR(100) NOT NULL DEFAULT 'General',
                priority VARCHAR(20) NOT NULL DEFAULT 'Medium',
                message TEXT NOT NULL,
                status VARCHAR(20) NOT NULL DEFAULT 'Open',
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS payroll (
                id SERIAL PRIMARY KEY,
                employee_id INT NOT NULL REFERENCES employees(id) ON DELETE CASCADE,
                pay_period VARCHAR(20) NOT NULL,
                base_salary NUMERIC(12,2) NOT NULL DEFAULT 0,
                allowances NUMERIC(12,2) NOT NULL DEFAULT 0,
                deductions NUMERIC(12,2) NOT NULL DEFAULT 0,
                net_pay NUMERIC(12,2) NOT NULL DEFAULT 0,
                status VARCHAR(20) NOT NULL DEFAULT 'Processed',
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
            );
        ");

        // Migration check for existing help_contexts table with string keys
        db.Database.ExecuteSqlRaw(@"
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1 
                    FROM information_schema.columns 
                    WHERE table_name='help_contexts' AND column_name='module_key'
                ) THEN
                    -- Populate 3NF tables from existing string keys if modules is empty
                    INSERT INTO modules (key, display_name, sort_order)
                    SELECT DISTINCT module_key, INITCAP(module_key), 0
                    FROM help_contexts
                    ON CONFLICT (key) DO NOTHING;

                    INSERT INTO pages (module_id, key, display_name, sort_order)
                    SELECT DISTINCT m.id, hc.page_key, INITCAP(hc.page_key), 0
                    FROM help_contexts hc
                    JOIN modules m ON m.key = hc.module_key
                    ON CONFLICT (module_id, key) DO NOTHING;

                    INSERT INTO functionalities (page_id, key, display_name)
                    SELECT DISTINCT p.id, hc.functionality_key, INITCAP(hc.functionality_key)
                    FROM help_contexts hc
                    JOIN modules m ON m.key = hc.module_key
                    JOIN pages p ON p.module_id = m.id AND p.key = hc.page_key
                    ON CONFLICT (page_id, key) DO NOTHING;

                    -- Add foreign key columns if not present
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='help_contexts' AND column_name='functionality_id') THEN
                        ALTER TABLE help_contexts ADD COLUMN functionality_id INT REFERENCES functionalities(id) ON DELETE CASCADE;
                    END IF;

                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='help_contexts' AND column_name='page_id') THEN
                        ALTER TABLE help_contexts ADD COLUMN page_id INT REFERENCES pages(id) ON DELETE CASCADE;
                    END IF;

                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='help_contexts' AND column_name='module_id') THEN
                        ALTER TABLE help_contexts ADD COLUMN module_id INT REFERENCES modules(id) ON DELETE CASCADE;
                    END IF;

                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='help_contexts' AND column_name='is_active') THEN
                        ALTER TABLE help_contexts ADD COLUMN is_active BOOLEAN NOT NULL DEFAULT TRUE;
                    END IF;

                    -- Map existing entries to functionality level
                    UPDATE help_contexts hc
                    SET functionality_id = f.id
                    FROM modules m
                    JOIN pages p ON p.module_id = m.id
                    JOIN functionalities f ON f.page_id = p.id
                    WHERE hc.module_key = m.key AND hc.page_key = p.key AND hc.functionality_key = f.key;

                    -- Drop legacy unique constraint and columns
                    ALTER TABLE help_contexts DROP CONSTRAINT IF EXISTS unique_help_context;
                    ALTER TABLE help_contexts DROP COLUMN IF EXISTS module_key;
                    ALTER TABLE help_contexts DROP COLUMN IF EXISTS page_key;
                    ALTER TABLE help_contexts DROP COLUMN IF EXISTS functionality_key;
                END IF;
            END $$;

            CREATE TABLE IF NOT EXISTS help_contexts (
                id SERIAL PRIMARY KEY,
                functionality_id INT REFERENCES functionalities(id) ON DELETE CASCADE,
                page_id INT REFERENCES pages(id) ON DELETE CASCADE,
                module_id INT REFERENCES modules(id) ON DELETE CASCADE,
                title VARCHAR(255) NOT NULL DEFAULT 'Quick steps',
                is_active BOOLEAN NOT NULL DEFAULT TRUE,
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                CONSTRAINT chk_help_context_single_target CHECK (
                    (CASE WHEN functionality_id IS NOT NULL THEN 1 ELSE 0 END +
                     CASE WHEN page_id IS NOT NULL THEN 1 ELSE 0 END +
                     CASE WHEN module_id IS NOT NULL THEN 1 ELSE 0 END) = 1
                )
            );

            CREATE TABLE IF NOT EXISTS help_steps (
                id SERIAL PRIMARY KEY,
                help_context_id INT NOT NULL REFERENCES help_contexts(id) ON DELETE CASCADE,
                step_number INT NOT NULL,
                step_text TEXT NOT NULL,
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                CONSTRAINT unique_help_context_step UNIQUE(help_context_id, step_number)
            );
        ");
    }

    private static void SeedRolesAndAdminUser(EmsDbContext db, IPasswordHasher passwordHasher)
    {
        if (!db.Roles.Any())
        {
            var roles = new List<Role>
            {
                new Role { Name = "Admin", Description = "System Administrator with full access" },
                new Role { Name = "HR", Description = "Human Resources Manager" },
                new Role { Name = "Manager", Description = "Department Manager" },
                new Role { Name = "Employee", Description = "Standard Employee" }
            };
            db.Roles.AddRange(roles);
            db.SaveChanges();
        }

        var adminRole = db.Roles.FirstOrDefault(r => r.Name == "Admin");
        if (adminRole != null && !db.Users.Any(u => u.Email == "admin@raras.com" || u.Username == "admin"))
        {
            var adminUser = new User
            {
                Email = "admin@raras.com",
                Username = "admin",
                PasswordHash = passwordHasher.HashPassword("admin123"),
                FirstName = "Admin",
                LastName = "User",
                RoleId = adminRole.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            db.Users.Add(adminUser);
            db.SaveChanges();
        }
    }

    public static void SeedHelpData(EmsDbContext db)
    {
        if (!db.Modules.Any())
        {
            var modules = new List<Module>
            {
                new Module { Key = "dashboard", DisplayName = "Dashboard", Icon = "layout-dashboard", SortOrder = 1 },
                new Module { Key = "employees", DisplayName = "Employees", Icon = "users", SortOrder = 2 },
                new Module { Key = "departments", DisplayName = "Departments", Icon = "building", SortOrder = 3 },
                new Module { Key = "attendance", DisplayName = "Attendance", Icon = "calendar-check", SortOrder = 4 },
                new Module { Key = "leave", DisplayName = "Leave Management", Icon = "calendar", SortOrder = 5 },
                new Module { Key = "payroll", DisplayName = "Payroll", Icon = "dollar-sign", SortOrder = 6 },
                new Module { Key = "auth", DisplayName = "Authentication", Icon = "lock", SortOrder = 7 }
            };
            db.Modules.AddRange(modules);
            db.SaveChanges();
        }

        if (!db.Pages.Any())
        {
            var dashMod = db.Modules.First(m => m.Key == "dashboard");
            var empMod = db.Modules.First(m => m.Key == "employees");
            var deptMod = db.Modules.First(m => m.Key == "departments");
            var attMod = db.Modules.First(m => m.Key == "attendance");
            var leaveMod = db.Modules.First(m => m.Key == "leave");
            var payMod = db.Modules.First(m => m.Key == "payroll");
            var authMod = db.Modules.First(m => m.Key == "auth");

            var pages = new List<Page>
            {
                new Page { ModuleId = dashMod.Id, Key = "overview", DisplayName = "Dashboard Overview", RoutePath = "/dashboard", SortOrder = 1 },
                new Page { ModuleId = empMod.Id, Key = "employee-list", DisplayName = "Employee List", RoutePath = "/employees", SortOrder = 1 },
                new Page { ModuleId = empMod.Id, Key = "employee-details", DisplayName = "Employee Details", RoutePath = "/employees/:id", SortOrder = 2 },
                new Page { ModuleId = deptMod.Id, Key = "department-list", DisplayName = "Department List", RoutePath = "/departments", SortOrder = 1 },
                new Page { ModuleId = attMod.Id, Key = "attendance-list", DisplayName = "Attendance List", RoutePath = "/attendance", SortOrder = 1 },
                new Page { ModuleId = leaveMod.Id, Key = "leave-list", DisplayName = "Leave List", RoutePath = "/leave", SortOrder = 1 },
                new Page { ModuleId = payMod.Id, Key = "payroll-list", DisplayName = "Payroll List", RoutePath = "/payroll", SortOrder = 1 },
                new Page { ModuleId = authMod.Id, Key = "login", DisplayName = "Login Page", RoutePath = "/login", SortOrder = 1 }
            };
            db.Pages.AddRange(pages);
            db.SaveChanges();
        }

        if (!db.Functionalities.Any())
        {
            var empListPage = db.Pages.First(p => p.Key == "employee-list");
            var empDetailsPage = db.Pages.First(p => p.Key == "employee-details");
            var deptListPage = db.Pages.First(p => p.Key == "department-list");
            var attListPage = db.Pages.First(p => p.Key == "attendance-list");
            var leaveListPage = db.Pages.First(p => p.Key == "leave-list");
            var payListPage = db.Pages.First(p => p.Key == "payroll-list");
            var loginPage = db.Pages.First(p => p.Key == "login");

            var functionalities = new List<Functionality>
            {
                new Functionality { PageId = empListPage.Id, Key = "manage-employees", DisplayName = "Manage Employees" },
                new Functionality { PageId = empDetailsPage.Id, Key = "add-document", DisplayName = "Add Employee Document" },
                new Functionality { PageId = deptListPage.Id, Key = "manage-departments", DisplayName = "Manage Departments" },
                new Functionality { PageId = attListPage.Id, Key = "manage-attendance", DisplayName = "Manage Attendance" },
                new Functionality { PageId = leaveListPage.Id, Key = "manage-leave", DisplayName = "Manage Leave Requests" },
                new Functionality { PageId = payListPage.Id, Key = "manage-payroll", DisplayName = "Manage Payroll" },
                new Functionality { PageId = loginPage.Id, Key = "login-form", DisplayName = "User Login Form" }
            };
            db.Functionalities.AddRange(functionalities);
            db.SaveChanges();
        }

        if (!db.HelpContexts.Any())
        {
            var dashMod = db.Modules.First(m => m.Key == "dashboard");
            var empListPage = db.Pages.First(p => p.Key == "employee-list");
            var addDocFunc = db.Functionalities.First(f => f.Key == "add-document");
            var deptListPage = db.Pages.First(p => p.Key == "department-list");
            var attListPage = db.Pages.First(p => p.Key == "attendance-list");
            var leaveListPage = db.Pages.First(p => p.Key == "leave-list");
            var payListPage = db.Pages.First(p => p.Key == "payroll-list");
            var loginFunc = db.Functionalities.First(f => f.Key == "login-form");

            var contexts = new List<HelpContext>
            {
                // Dashboard -> Module-level help
                new HelpContext
                {
                    ModuleId = dashMod.Id,
                    Title = "Quick steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Use the sidebar to open the module you need." },
                        new HelpStep { StepNumber = 2, StepText = "Review the dashboard overview and current system information." },
                        new HelpStep { StepNumber = 3, StepText = "Open Employees, Departments, Attendance, Leave, or Payroll as needed." },
                        new HelpStep { StepNumber = 4, StepText = "Use the ⓘ Need help? beside a function whenever you need guidance." }
                    }
                },
                // Employees List -> Page-level help
                new HelpContext
                {
                    PageId = empListPage.Id,
                    Title = "Quick steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Open Employees from the sidebar." },
                        new HelpStep { StepNumber = 2, StepText = "Click Add Employee to open the employee registration form." },
                        new HelpStep { StepNumber = 3, StepText = "Enter the required personal and employment information." },
                        new HelpStep { StepNumber = 4, StepText = "Select the employee's department and position." },
                        new HelpStep { StepNumber = 5, StepText = "Click Save Employee to complete registration." }
                    }
                },
                // Add Document -> Functionality-level help
                new HelpContext
                {
                    FunctionalityId = addDocFunc.Id,
                    Title = "Document upload steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Navigate to the target Employee Profile page." },
                        new HelpStep { StepNumber = 2, StepText = "Click the Documents tab or Add Document button." },
                        new HelpStep { StepNumber = 3, StepText = "Select the document type and file from your computer." },
                        new HelpStep { StepNumber = 4, StepText = "Click Upload Document to attach it to the employee profile." }
                    }
                },
                // Departments List -> Page-level help
                new HelpContext
                {
                    PageId = deptListPage.Id,
                    Title = "Quick steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Open Departments from the sidebar." },
                        new HelpStep { StepNumber = 2, StepText = "Click Add Department." },
                        new HelpStep { StepNumber = 3, StepText = "Enter the department name and required information." },
                        new HelpStep { StepNumber = 4, StepText = "Review the department details." },
                        new HelpStep { StepNumber = 5, StepText = "Save the department." }
                    }
                },
                // Attendance List -> Page-level help
                new HelpContext
                {
                    PageId = attListPage.Id,
                    Title = "Quick steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Open Attendance from the sidebar." },
                        new HelpStep { StepNumber = 2, StepText = "Select the employee whose attendance you want to record." },
                        new HelpStep { StepNumber = 3, StepText = "Select the correct attendance status." },
                        new HelpStep { StepNumber = 4, StepText = "Check the attendance date and details." },
                        new HelpStep { StepNumber = 5, StepText = "Save the attendance record." }
                    }
                },
                // Leave List -> Page-level help
                new HelpContext
                {
                    PageId = leaveListPage.Id,
                    Title = "Quick steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Open Leave Management from the sidebar." },
                        new HelpStep { StepNumber = 2, StepText = "Click New Request." },
                        new HelpStep { StepNumber = 3, StepText = "Select the employee and leave type." },
                        new HelpStep { StepNumber = 4, StepText = "Select the start and end dates." },
                        new HelpStep { StepNumber = 5, StepText = "Submit the leave request." }
                    }
                },
                // Payroll List -> Page-level help
                new HelpContext
                {
                    PageId = payListPage.Id,
                    Title = "Quick steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Open Payroll from the sidebar." },
                        new HelpStep { StepNumber = 2, StepText = "Review the employee salary information." },
                        new HelpStep { StepNumber = 3, StepText = "Verify the payroll details before processing." },
                        new HelpStep { StepNumber = 4, StepText = "Check the calculated payroll information." },
                        new HelpStep { StepNumber = 5, StepText = "Process payroll according to your organization workflow." }
                    }
                },
                // Auth Login -> Functionality-level help
                new HelpContext
                {
                    FunctionalityId = loginFunc.Id,
                    Title = "Quick steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Enter your username or email." },
                        new HelpStep { StepNumber = 2, StepText = "Enter your password." },
                        new HelpStep { StepNumber = 3, StepText = "Click Login." },
                        new HelpStep { StepNumber = 4, StepText = "The system validates your credentials." },
                        new HelpStep { StepNumber = 5, StepText = "If successful, you are redirected to the Dashboard." }
                    }
                }
            };

            db.HelpContexts.AddRange(contexts);
            db.SaveChanges();
        }
    }
}
