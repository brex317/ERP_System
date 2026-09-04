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

            CREATE TABLE IF NOT EXISTS help_contexts (
                id SERIAL PRIMARY KEY,
                module_key VARCHAR(100) NOT NULL,
                page_key VARCHAR(100) NOT NULL,
                functionality_key VARCHAR(100) NOT NULL,
                title VARCHAR(255) NOT NULL DEFAULT 'Quick steps',
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                CONSTRAINT unique_help_context UNIQUE(module_key, page_key, functionality_key)
            );

            CREATE TABLE IF NOT EXISTS help_steps (
                id SERIAL PRIMARY KEY,
                help_context_id INT NOT NULL REFERENCES help_contexts(id) ON DELETE CASCADE,
                step_number INT NOT NULL,
                step_text TEXT NOT NULL,
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
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
        // Seed auth login help context if missing
        bool hasAuthHelp = db.HelpContexts.Any(c => c.ModuleKey == "auth" && c.PageKey == "login" && c.FunctionalityKey == "login-form");
        if (!hasAuthHelp)
        {
            var authHelp = new HelpContext
            {
                ModuleKey = "auth",
                PageKey = "login",
                FunctionalityKey = "login-form",
                Title = "Quick steps",
                Steps = new List<HelpStep>
                {
                    new HelpStep { StepNumber = 1, StepText = "Enter your username or email." },
                    new HelpStep { StepNumber = 2, StepText = "Enter your password." },
                    new HelpStep { StepNumber = 3, StepText = "Click Login." },
                    new HelpStep { StepNumber = 4, StepText = "The system validates your credentials." },
                    new HelpStep { StepNumber = 5, StepText = "If successful, you are redirected to the Dashboard." }
                }
            };
            db.HelpContexts.Add(authHelp);
            db.SaveChanges();
        }

        if (!db.HelpContexts.Any(c => c.ModuleKey == "dashboard"))
        {
            var contexts = new List<HelpContext>
            {
                new HelpContext
                {
                    ModuleKey = "dashboard",
                    PageKey = "overview",
                    FunctionalityKey = "general",
                    Title = "Quick steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Use the sidebar to open the module you need." },
                        new HelpStep { StepNumber = 2, StepText = "Review the dashboard overview and current system information." },
                        new HelpStep { StepNumber = 3, StepText = "Open Employees, Departments, Attendance, Leave, or Payroll as needed." },
                        new HelpStep { StepNumber = 4, StepText = "Use the ⓘ Need help? beside a function whenever you need guidance." }
                    }
                },
                new HelpContext
                {
                    ModuleKey = "employees",
                    PageKey = "employee-list",
                    FunctionalityKey = "manage-employees",
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
                new HelpContext
                {
                    ModuleKey = "employees",
                    PageKey = "employee-details",
                    FunctionalityKey = "add-document",
                    Title = "Document upload steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Navigate to the target Employee Profile page." },
                        new HelpStep { StepNumber = 2, StepText = "Click the Documents tab or Add Document button." },
                        new HelpStep { StepNumber = 3, StepText = "Select the document type and file from your computer." },
                        new HelpStep { StepNumber = 4, StepText = "Click Upload Document to attach it to the employee profile." }
                    }
                },
                new HelpContext
                {
                    ModuleKey = "departments",
                    PageKey = "department-list",
                    FunctionalityKey = "manage-departments",
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
                new HelpContext
                {
                    ModuleKey = "attendance",
                    PageKey = "attendance-list",
                    FunctionalityKey = "manage-attendance",
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
                new HelpContext
                {
                    ModuleKey = "leave",
                    PageKey = "leave-list",
                    FunctionalityKey = "manage-leave",
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
                new HelpContext
                {
                    ModuleKey = "payroll",
                    PageKey = "payroll-list",
                    FunctionalityKey = "manage-payroll",
                    Title = "Quick steps",
                    Steps = new List<HelpStep>
                    {
                        new HelpStep { StepNumber = 1, StepText = "Open Payroll from the sidebar." },
                        new HelpStep { StepNumber = 2, StepText = "Review the employee salary information." },
                        new HelpStep { StepNumber = 3, StepText = "Verify the payroll details before processing." },
                        new HelpStep { StepNumber = 4, StepText = "Check the calculated payroll information." },
                        new HelpStep { StepNumber = 5, StepText = "Process payroll according to your organization workflow." }
                    }
                }
            };

            db.HelpContexts.AddRange(contexts);
            db.SaveChanges();
        }
    }
}
