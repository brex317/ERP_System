-- PostgreSQL Seed Data Script for RARAS Employee Management System (EMS)

-- Clear existing data
TRUNCATE TABLE users, roles, help_steps, help_contexts, functionalities, pages, modules, leave_requests, attendance, employees, departments RESTART IDENTITY CASCADE;

-- Insert Roles
INSERT INTO roles (id, name, description) VALUES
(1, 'Admin', 'System Administrator with full access'),
(2, 'HR', 'Human Resources Manager'),
(3, 'Manager', 'Department Manager'),
(4, 'Employee', 'Standard Employee');
SELECT setval('roles_id_seq', (SELECT MAX(id) FROM roles));

-- Insert Admin User (password: admin123)
-- Hash will be generated/verified by C# PasswordHasher service
INSERT INTO users (id, email, username, password_hash, first_name, last_name, role_id, is_active) VALUES
(1, 'admin@raras.com', 'admin', 'UmFyYXNFbXNTYWx0MjAyNiE=.15V9zYk3g8mK9w2Pq+x7lQ40x9K7P8lY2fW1h4a5b6c=', 'Admin', 'User', 1, true);
SELECT setval('users_id_seq', (SELECT MAX(id) FROM users));


-- Insert 12 Departments
INSERT INTO departments (name, code, description) VALUES
('Information Technology', 'IT', 'Software development and technical infrastructure'),
('Finance & Accounting', 'FIN', 'Financial reporting, budget management, and accounting'),
('Human Resources', 'HR', 'Talent acquisition, employee relations, and HR compliance'),
('Operations', 'OPS', 'Day-to-day operations and workflow execution'),
('Sales & Business Dev', 'SALES', 'Client acquisition and revenue growth'),
('Marketing & PR', 'MKT', 'Brand strategy, social media, and market outreach'),
('Customer Support', 'SUPP', 'Client helpdesk and post-sales support'),
('Legal & Compliance', 'LEG', 'Legal oversight and regulatory compliance'),
('Research & Development', 'RD', 'Product innovation and engineering research'),
('Supply Chain & Logistics', 'LOG', 'Inventory, procurement, and logistics management'),
('Administration', 'ADMIN', 'General office management and facilities'),
('Quality Assurance', 'QA', 'Software and operational quality testing');

-- Insert 248 Employees
-- We insert 248 records (18 on leave, 221 present today, 9 absent/other)
DO $$
DECLARE
    i INT;
    dept_id INT;
    first_names TEXT[] := ARRAY['Abebe', 'Kebede', 'Tigist', 'Berihu', 'Sara', 'John', 'Michael', 'Almaz', 'Dawit', 'Eleni', 'Haile', 'Marta', 'Solomon', 'Tsion', 'Yared'];
    last_names TEXT[] := ARRAY['Tadesse', 'Bekele', 'Alemu', 'Gebre', 'Smith', 'Johnson', 'Worku', 'Kassa', 'Girma', 'Haile', 'Tefera', 'Desta', 'Mengistu', 'Assefa', 'Zerihun'];
    positions TEXT[] := ARRAY['Software Developer', 'Accountant', 'HR Specialist', 'Operations Manager', 'Sales Executive', 'Marketing Lead', 'Support Specialist', 'QA Engineer', 'Project Manager', 'Data Analyst'];
    fn TEXT;
    ln TEXT;
    pos TEXT;
BEGIN
    FOR i IN 1..248 LOOP
        dept_id := (i % 12) + 1;
        fn := first_names[(i % ARRAY_LENGTH(first_names, 1)) + 1];
        ln := last_names[(i % ARRAY_LENGTH(last_names, 1)) + 1];
        pos := positions[(i % ARRAY_LENGTH(positions, 1)) + 1];
        
        INSERT INTO employees (first_name, last_name, email, department_id, position, status, hire_date)
        VALUES (
            fn,
            ln || i,
            LOWER(fn) || '.' || LOWER(ln) || i || '@raras.com',
            dept_id,
            pos,
            'Active',
            CURRENT_DATE - (i * INTERVAL '3 days')
        );
    END LOOP;
END $$;

-- Insert Attendance for Today: 221 Present
DO $$
DECLARE
    emp_id INT;
BEGIN
    FOR emp_id IN 1..221 LOOP
        INSERT INTO attendance (employee_id, date, status, check_in)
        VALUES (emp_id, CURRENT_DATE, 'Present', '08:30:00');
    END LOOP;
END $$;

-- Insert 18 Active Approved Leave Requests for Today (Employees 222 to 239)
DO $$
DECLARE
    emp_id INT;
BEGIN
    FOR emp_id IN 222..239 LOOP
        INSERT INTO leave_requests (employee_id, leave_type, start_date, end_date, status, reason)
        VALUES (emp_id, 'Annual Leave', CURRENT_DATE - INTERVAL '1 day', CURRENT_DATE + INTERVAL '5 days', 'Approved', 'Scheduled vacation');

        INSERT INTO attendance (employee_id, date, status)
        VALUES (emp_id, CURRENT_DATE, 'On Leave');
    END LOOP;
END $$;

-- Remaining employees (240 to 248) logged as Absent/Late
DO $$
DECLARE
    emp_id INT;
BEGIN
    FOR emp_id IN 240..248 LOOP
        INSERT INTO attendance (employee_id, date, status)
        VALUES (emp_id, CURRENT_DATE, 'Absent');
    END LOOP;
END $$;

-- Insert 3NF Modules
INSERT INTO modules (id, key, display_name, icon, sort_order) VALUES
(1, 'dashboard', 'Dashboard', 'layout-dashboard', 1),
(2, 'employees', 'Employees', 'users', 2),
(3, 'departments', 'Departments', 'building', 3),
(4, 'attendance', 'Attendance', 'calendar-check', 4),
(5, 'leave', 'Leave Management', 'calendar', 5),
(6, 'payroll', 'Payroll', 'dollar-sign', 6),
(7, 'auth', 'Authentication', 'lock', 7);
SELECT setval('modules_id_seq', (SELECT MAX(id) FROM modules));

-- Insert 3NF Pages
INSERT INTO pages (id, module_id, key, display_name, route_path, sort_order) VALUES
(1, 1, 'overview', 'Dashboard Overview', '/dashboard', 1),
(2, 2, 'employee-list', 'Employee List', '/employees', 1),
(3, 2, 'employee-details', 'Employee Details', '/employees/:id', 2),
(4, 3, 'department-list', 'Department List', '/departments', 1),
(5, 4, 'attendance-list', 'Attendance List', '/attendance', 1),
(6, 5, 'leave-list', 'Leave List', '/leave', 1),
(7, 6, 'payroll-list', 'Payroll List', '/payroll', 1),
(8, 7, 'login', 'Login Page', '/login', 1);
SELECT setval('pages_id_seq', (SELECT MAX(id) FROM pages));

-- Insert 3NF Functionalities
INSERT INTO functionalities (id, page_id, key, display_name) VALUES
(1, 2, 'manage-employees', 'Manage Employees'),
(2, 3, 'add-document', 'Add Employee Document'),
(3, 4, 'manage-departments', 'Manage Departments'),
(4, 5, 'manage-attendance', 'Manage Attendance'),
(5, 6, 'manage-leave', 'Manage Leave Requests'),
(6, 7, 'manage-payroll', 'Manage Payroll'),
(7, 8, 'login-form', 'User Login Form');
SELECT setval('functionalities_id_seq', (SELECT MAX(id) FROM functionalities));

-- Insert Help Contexts (Module, Page, or Functionality levels)
-- 1. Dashboard Context -> Module-level help (module_id = 1)
INSERT INTO help_contexts (id, module_id, title) VALUES
(1, 1, 'Quick steps');

INSERT INTO help_steps (help_context_id, step_number, step_text) VALUES
(1, 1, 'Use the sidebar to open the module you need.'),
(1, 2, 'Review the dashboard overview and current system information.'),
(1, 3, 'Open Employees, Departments, Attendance, Leave, or Payroll as needed.'),
(1, 4, 'Use the ⓘ Need help? beside a function whenever you need guidance.');

-- 2. Employees Context -> Page-level help (page_id = 2 [employee-list])
INSERT INTO help_contexts (id, page_id, title) VALUES
(2, 2, 'Quick steps');

INSERT INTO help_steps (help_context_id, step_number, step_text) VALUES
(2, 1, 'Open Employees from the sidebar.'),
(2, 2, 'Click Add Employee to open the employee registration form.'),
(2, 3, 'Enter the required personal and employment information.'),
(2, 4, 'Select the employee’s department and position.'),
(2, 5, 'Click Save Employee to complete registration.');

-- 3. Employee Details / Add Document Context -> Functionality-level help (functionality_id = 2 [add-document])
INSERT INTO help_contexts (id, functionality_id, title) VALUES
(3, 2, 'Document upload steps');

INSERT INTO help_steps (help_context_id, step_number, step_text) VALUES
(3, 1, 'Navigate to the target Employee Profile page.'),
(3, 2, 'Click the Documents tab or Add Document button.'),
(3, 3, 'Select the document type and file from your computer.'),
(3, 4, 'Click Upload Document to attach it to the employee profile.');

-- 4. Departments Context -> Page-level help (page_id = 4 [department-list])
INSERT INTO help_contexts (id, page_id, title) VALUES
(4, 4, 'Quick steps');

INSERT INTO help_steps (help_context_id, step_number, step_text) VALUES
(4, 1, 'Open Departments from the sidebar.'),
(4, 2, 'Click Add Department.'),
(4, 3, 'Enter the department name and required information.'),
(4, 4, 'Review the department details.'),
(4, 5, 'Save the department.');

-- 5. Attendance Context -> Page-level help (page_id = 5 [attendance-list])
INSERT INTO help_contexts (id, page_id, title) VALUES
(5, 5, 'Quick steps');

INSERT INTO help_steps (help_context_id, step_number, step_text) VALUES
(5, 1, 'Open Attendance from the sidebar.'),
(5, 2, 'Select the employee whose attendance you want to record.'),
(5, 3, 'Select the correct attendance status.'),
(5, 4, 'Check the attendance date and details.'),
(5, 5, 'Save the attendance record.');

-- 6. Leave Context -> Page-level help (page_id = 6 [leave-list])
INSERT INTO help_contexts (id, page_id, title) VALUES
(6, 6, 'Quick steps');

INSERT INTO help_steps (help_context_id, step_number, step_text) VALUES
(6, 1, 'Open Leave Management from the sidebar.'),
(6, 2, 'Click New Request.'),
(6, 3, 'Select the employee and leave type.'),
(6, 4, 'Select the start and end dates.'),
(6, 5, 'Submit the leave request.');

-- 7. Payroll Context -> Page-level help (page_id = 7 [payroll-list])
INSERT INTO help_contexts (id, page_id, title) VALUES
(7, 7, 'Quick steps');

INSERT INTO help_steps (help_context_id, step_number, step_text) VALUES
(7, 1, 'Open Payroll from the sidebar.'),
(7, 2, 'Review the employee salary information.'),
(7, 3, 'Verify the payroll details before processing.'),
(7, 4, 'Check the calculated payroll information.'),
(7, 5, 'Process payroll according to your organization workflow.');

-- 8. Login Context -> Functionality-level help (functionality_id = 7 [login-form])
INSERT INTO help_contexts (id, functionality_id, title) VALUES
(8, 7, 'Quick steps');

INSERT INTO help_steps (help_context_id, step_number, step_text) VALUES
(8, 1, 'Enter your username or email.'),
(8, 2, 'Enter your password.'),
(8, 3, 'Click Login.'),
(8, 4, 'The system validates your credentials.'),
(8, 5, 'If successful, you are redirected to the Dashboard.');

SELECT setval('help_contexts_id_seq', (SELECT MAX(id) FROM help_contexts));



