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
            DROP TABLE IF EXISTS help_steps, help_contexts, functionalities, pages CASCADE;
            DROP TABLE IF EXISTS help_details, help_headers, feature_specifications, features CASCADE;

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

            CREATE TABLE IF NOT EXISTS features (
                id SERIAL PRIMARY KEY,
                module_id INT NOT NULL REFERENCES modules(id) ON DELETE CASCADE,
                key VARCHAR(100) NOT NULL,
                display_name VARCHAR(150) NOT NULL,
                route_path VARCHAR(255),
                sort_order INT NOT NULL DEFAULT 0,
                CONSTRAINT unique_module_feature UNIQUE(module_id, key)
            );

            CREATE TABLE IF NOT EXISTS feature_specifications (
                id SERIAL PRIMARY KEY,
                feature_id INT NOT NULL REFERENCES features(id) ON DELETE CASCADE,
                key VARCHAR(100) NOT NULL,
                display_name VARCHAR(150) NOT NULL,
                CONSTRAINT unique_feature_specification UNIQUE(feature_id, key)
            );

            CREATE TABLE IF NOT EXISTS help_headers (
                id SERIAL PRIMARY KEY,
                feature_specification_id INT REFERENCES feature_specifications(id) ON DELETE CASCADE,
                feature_id INT REFERENCES features(id) ON DELETE CASCADE,
                module_id INT REFERENCES modules(id) ON DELETE CASCADE,
                title VARCHAR(255) NOT NULL DEFAULT 'Quick steps',
                is_active BOOLEAN NOT NULL DEFAULT TRUE,
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                CONSTRAINT chk_help_header_single_target CHECK (
                    (CASE WHEN feature_specification_id IS NOT NULL THEN 1 ELSE 0 END +
                     CASE WHEN feature_id IS NOT NULL THEN 1 ELSE 0 END +
                     CASE WHEN module_id IS NOT NULL THEN 1 ELSE 0 END) = 1
                )
            );

            CREATE TABLE IF NOT EXISTS help_details (
                id SERIAL PRIMARY KEY,
                help_header_id INT NOT NULL REFERENCES help_headers(id) ON DELETE CASCADE,
                step_number INT NOT NULL,
                step_text TEXT NOT NULL,
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                CONSTRAINT unique_help_header_detail UNIQUE(help_header_id, step_number)
            );

            CREATE INDEX IF NOT EXISTS idx_features_key ON features (lower(key));
            CREATE INDEX IF NOT EXISTS idx_feature_specifications_key ON feature_specifications (lower(key));
            CREATE INDEX IF NOT EXISTS idx_modules_key ON modules (lower(key));
            CREATE INDEX IF NOT EXISTS idx_help_headers_module ON help_headers (module_id) WHERE module_id IS NOT NULL;
            CREATE INDEX IF NOT EXISTS idx_help_headers_feature ON help_headers (feature_id) WHERE feature_id IS NOT NULL;
            CREATE INDEX IF NOT EXISTS idx_help_headers_feature_spec ON help_headers (feature_specification_id) WHERE feature_specification_id IS NOT NULL;
            CREATE INDEX IF NOT EXISTS idx_help_details_header ON help_details (help_header_id);

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

        if (!db.Features.Any())
        {
            var dashMod = db.Modules.First(m => m.Key == "dashboard");
            var empMod = db.Modules.First(m => m.Key == "employees");
            var deptMod = db.Modules.First(m => m.Key == "departments");
            var attMod = db.Modules.First(m => m.Key == "attendance");
            var leaveMod = db.Modules.First(m => m.Key == "leave");
            var payMod = db.Modules.First(m => m.Key == "payroll");
            var authMod = db.Modules.First(m => m.Key == "auth");

            var features = new List<Feature>
            {
                new Feature { ModuleId = dashMod.Id, Key = "overview", DisplayName = "Dashboard Overview", RoutePath = "/dashboard", SortOrder = 1 },
                new Feature { ModuleId = empMod.Id, Key = "employee-list", DisplayName = "Employee List", RoutePath = "/employees", SortOrder = 1 },
                new Feature { ModuleId = empMod.Id, Key = "employee-details", DisplayName = "Employee Details", RoutePath = "/employees/:id", SortOrder = 2 },
                new Feature { ModuleId = deptMod.Id, Key = "department-list", DisplayName = "Department List", RoutePath = "/departments", SortOrder = 1 },
                new Feature { ModuleId = attMod.Id, Key = "attendance-list", DisplayName = "Attendance List", RoutePath = "/attendance", SortOrder = 1 },
                new Feature { ModuleId = leaveMod.Id, Key = "leave-list", DisplayName = "Leave List", RoutePath = "/leave", SortOrder = 1 },
                new Feature { ModuleId = payMod.Id, Key = "payroll-list", DisplayName = "Payroll List", RoutePath = "/payroll", SortOrder = 1 },
                new Feature { ModuleId = authMod.Id, Key = "login", DisplayName = "Login Page", RoutePath = "/login", SortOrder = 1 }
            };
            db.Features.AddRange(features);
            db.SaveChanges();
        }

        if (!db.FeatureSpecifications.Any())
        {
            var empListFeature = db.Features.First(p => p.Key == "employee-list");
            var empDetailsFeature = db.Features.First(p => p.Key == "employee-details");
            var deptListFeature = db.Features.First(p => p.Key == "department-list");
            var attListFeature = db.Features.First(p => p.Key == "attendance-list");
            var leaveListFeature = db.Features.First(p => p.Key == "leave-list");
            var payListFeature = db.Features.First(p => p.Key == "payroll-list");
            var loginFeature = db.Features.First(p => p.Key == "login");

            var featureSpecifications = new List<FeatureSpecification>
            {
                new FeatureSpecification { FeatureId = empListFeature.Id, Key = "manage-employees", DisplayName = "Manage Employees" },
                new FeatureSpecification { FeatureId = empDetailsFeature.Id, Key = "add-document", DisplayName = "Add Employee Document" },
                new FeatureSpecification { FeatureId = deptListFeature.Id, Key = "manage-departments", DisplayName = "Manage Departments" },
                new FeatureSpecification { FeatureId = attListFeature.Id, Key = "manage-attendance", DisplayName = "Manage Attendance" },
                new FeatureSpecification { FeatureId = leaveListFeature.Id, Key = "manage-leave", DisplayName = "Manage Leave Requests" },
                new FeatureSpecification { FeatureId = payListFeature.Id, Key = "manage-payroll", DisplayName = "Manage Payroll" },
                new FeatureSpecification { FeatureId = loginFeature.Id, Key = "login-form", DisplayName = "User Login Form" },
                new FeatureSpecification { FeatureId = deptListFeature.Id, Key = "add-department", DisplayName = "Add Department" },
                new FeatureSpecification { FeatureId = empListFeature.Id, Key = "add-employee", DisplayName = "Add Employee" },
                new FeatureSpecification { FeatureId = attListFeature.Id, Key = "manual-entry-correction", DisplayName = "Manual Entry / Correction" },
                new FeatureSpecification { FeatureId = leaveListFeature.Id, Key = "request-leave", DisplayName = "Request Leave" },
                new FeatureSpecification { FeatureId = payListFeature.Id, Key = "process-payroll", DisplayName = "Process Payroll" }
            };
            db.FeatureSpecifications.AddRange(featureSpecifications);
            db.SaveChanges();
        }

        if (!db.HelpHeaders.Any())
        {
            var dashMod = db.Modules.First(m => m.Key == "dashboard");
            var empListFeature = db.Features.First(p => p.Key == "employee-list");
            var addDocSpec = db.FeatureSpecifications.First(f => f.Key == "add-document");
            var deptListFeature = db.Features.First(p => p.Key == "department-list");
            var attListFeature = db.Features.First(p => p.Key == "attendance-list");
            var leaveListFeature = db.Features.First(p => p.Key == "leave-list");
            var payListFeature = db.Features.First(p => p.Key == "payroll-list");
            var loginSpec = db.FeatureSpecifications.First(f => f.Key == "login-form");

            var addDeptSpec = db.FeatureSpecifications.First(f => f.Key == "add-department");
            var addEmpSpec = db.FeatureSpecifications.First(f => f.Key == "add-employee");
            var manualAttSpec = db.FeatureSpecifications.First(f => f.Key == "manual-entry-correction");
            var reqLeaveSpec = db.FeatureSpecifications.First(f => f.Key == "request-leave");
            var procPaySpec = db.FeatureSpecifications.First(f => f.Key == "process-payroll");

            var headers = new List<HelpHeader>
            {
                // Dashboard -> Module-level help
                new HelpHeader
                {
                    ModuleId = dashMod.Id,
                    Title = "Quick steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Use the sidebar to open the module you need." },
                        new HelpDetail { StepNumber = 2, StepText = "Review the dashboard overview and current system information." },
                        new HelpDetail { StepNumber = 3, StepText = "Open Employees, Departments, Attendance, Leave, or Payroll as needed." },
                        new HelpDetail { StepNumber = 4, StepText = "Use the ⓘ Need help? beside a function whenever you need guidance." }
                    }
                },
                // Employees List -> Feature-level help
                new HelpHeader
                {
                    FeatureId = empListFeature.Id,
                    Title = "Quick steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Open Employees from the sidebar." },
                        new HelpDetail { StepNumber = 2, StepText = "Click Add Employee to open the employee registration form." },
                        new HelpDetail { StepNumber = 3, StepText = "Enter the required personal and employment information." },
                        new HelpDetail { StepNumber = 4, StepText = "Select the employee's department and position." },
                        new HelpDetail { StepNumber = 5, StepText = "Click Save Employee to complete registration." }
                    }
                },
                // Add Document -> Feature-specification-level help
                new HelpHeader
                {
                    FeatureSpecificationId = addDocSpec.Id,
                    Title = "Document upload steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Navigate to the target Employee Profile page." },
                        new HelpDetail { StepNumber = 2, StepText = "Click the Documents tab or Add Document button." },
                        new HelpDetail { StepNumber = 3, StepText = "Select the document type and file from your computer." },
                        new HelpDetail { StepNumber = 4, StepText = "Click Upload Document to attach it to the employee profile." }
                    }
                },
                // Departments List -> Feature-level help
                new HelpHeader
                {
                    FeatureId = deptListFeature.Id,
                    Title = "Quick steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Open Departments from the sidebar." },
                        new HelpDetail { StepNumber = 2, StepText = "Click Add Department." },
                        new HelpDetail { StepNumber = 3, StepText = "Enter the department name and required information." },
                        new HelpDetail { StepNumber = 4, StepText = "Review the department details." },
                        new HelpDetail { StepNumber = 5, StepText = "Save the department." }
                    }
                },
                // Attendance List -> Feature-level help
                new HelpHeader
                {
                    FeatureId = attListFeature.Id,
                    Title = "Quick steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Open Attendance from the sidebar." },
                        new HelpDetail { StepNumber = 2, StepText = "Select the employee whose attendance you want to record." },
                        new HelpDetail { StepNumber = 3, StepText = "Select the correct attendance status." },
                        new HelpDetail { StepNumber = 4, StepText = "Check the attendance date and details." },
                        new HelpDetail { StepNumber = 5, StepText = "Save the attendance record." }
                    }
                },
                // Leave List -> Feature-level help
                new HelpHeader
                {
                    FeatureId = leaveListFeature.Id,
                    Title = "Quick steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Open Leave Management from the sidebar." },
                        new HelpDetail { StepNumber = 2, StepText = "Click New Request." },
                        new HelpDetail { StepNumber = 3, StepText = "Select the employee and leave type." },
                        new HelpDetail { StepNumber = 4, StepText = "Select the start and end dates." },
                        new HelpDetail { StepNumber = 5, StepText = "Submit the leave request." }
                    }
                },
                // Payroll List -> Feature-level help
                new HelpHeader
                {
                    FeatureId = payListFeature.Id,
                    Title = "Quick steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Open Payroll from the sidebar." },
                        new HelpDetail { StepNumber = 2, StepText = "Review the employee salary information." },
                        new HelpDetail { StepNumber = 3, StepText = "Verify the payroll details before processing." },
                        new HelpDetail { StepNumber = 4, StepText = "Check the calculated payroll information." },
                        new HelpDetail { StepNumber = 5, StepText = "Process payroll according to your organization workflow." }
                    }
                },
                // Auth Login -> Feature-specification-level help
                new HelpHeader
                {
                    FeatureSpecificationId = loginSpec.Id,
                    Title = "Quick steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Enter your username or email." },
                        new HelpDetail { StepNumber = 2, StepText = "Enter your password." },
                        new HelpDetail { StepNumber = 3, StepText = "Click Login." },
                        new HelpDetail { StepNumber = 4, StepText = "The system validates your credentials." },
                        new HelpDetail { StepNumber = 5, StepText = "If successful, you are redirected to the Dashboard." }
                    }
                },
                // 9. Add Department Form -> Feature-specification-level help
                new HelpHeader
                {
                    FeatureSpecificationId = addDeptSpec.Id,
                    Title = "Add Department steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Click the ➕ Add Department button on the Departments Management page." },
                        new HelpDetail { StepNumber = 2, StepText = "Enter the required Department Name (e.g. Information Technology) and Department Code (e.g. IT)." },
                        new HelpDetail { StepNumber = 3, StepText = "Optionally provide a short Description detailing unit functions and responsibilities." },
                        new HelpDetail { StepNumber = 4, StepText = "Click Create Department to save the new department record." }
                    }
                },
                // 10. Add Employee Form -> Feature-specification-level help
                new HelpHeader
                {
                    FeatureSpecificationId = addEmpSpec.Id,
                    Title = "Add Employee steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Click the ➕ Add Employee button on the Employee Directory page." },
                        new HelpDetail { StepNumber = 2, StepText = "Enter the employee's First Name, Last Name, and mandatory Email Address." },
                        new HelpDetail { StepNumber = 3, StepText = "Select the assigned Department and enter their Position / Job Title." },
                        new HelpDetail { StepNumber = 4, StepText = "Set the account Status to Active or Inactive." },
                        new HelpDetail { StepNumber = 5, StepText = "Click Create Employee to finalize registration." }
                    }
                },
                // 11. Manual Entry / Correction Form -> Feature-specification-level help
                new HelpHeader
                {
                    FeatureSpecificationId = manualAttSpec.Id,
                    Title = "Attendance log steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Click the ➕ Manual Entry / Correction button on the Attendance page." },
                        new HelpDetail { StepNumber = 2, StepText = "Select the target Employee from the dropdown menu." },
                        new HelpDetail { StepNumber = 3, StepText = "Enter or adjust the Check In Time and Check Out Time fields." },
                        new HelpDetail { StepNumber = 4, StepText = "Choose the appropriate attendance Status (Present, Late, On Leave, or Absent)." },
                        new HelpDetail { StepNumber = 5, StepText = "Click Save Record to update the employee attendance entry." }
                    }
                },
                // 12. Request Leave Form -> Feature-specification-level help
                new HelpHeader
                {
                    FeatureSpecificationId = reqLeaveSpec.Id,
                    Title = "Leave request steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Click the ➕ Request Leave button on the Leave Management page." },
                        new HelpDetail { StepNumber = 2, StepText = "Select the applying Employee and choose the appropriate Leave Type." },
                        new HelpDetail { StepNumber = 3, StepText = "Specify the Start Date and End Date for the requested leave duration." },
                        new HelpDetail { StepNumber = 4, StepText = "Enter a detailed Reason for Leave explaining the request justification." },
                        new HelpDetail { StepNumber = 5, StepText = "Click Submit Request to send for manager approval." }
                    }
                },
                // 13. Process Payroll Form -> Feature-specification-level help
                new HelpHeader
                {
                    FeatureSpecificationId = procPaySpec.Id,
                    Title = "Payroll process steps",
                    Details = new List<HelpDetail>
                    {
                        new HelpDetail { StepNumber = 1, StepText = "Click the 💰 Process Payroll button on the Payroll & Salary Processing page." },
                        new HelpDetail { StepNumber = 2, StepText = "Select the target Employee and confirm the Pay Period (e.g. 2026-09)." },
                        new HelpDetail { StepNumber = 3, StepText = "Enter the Base Salary ($) along with any applicable Allowances ($)." },
                        new HelpDetail { StepNumber = 4, StepText = "Enter total Tax & Pension Deductions ($) to preview the Net Salary calculation." },
                        new HelpDetail { StepNumber = 5, StepText = "Click Process Salary to generate and record the monthly payslip statement." }
                    }
                }
            };

            db.HelpHeaders.AddRange(headers);
            db.SaveChanges();
        }
    }
}
