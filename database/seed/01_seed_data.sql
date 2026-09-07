-- PostgreSQL Seed Data Script for RARAS Employee Management System (EMS)

-- Clear existing data
TRUNCATE TABLE users, roles, help_details, help_headers, feature_specifications, features, modules, leave_requests, attendance, employees, departments RESTART IDENTITY CASCADE;

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

-- Insert 3NF Features
INSERT INTO features (id, module_id, key, display_name, route_path, sort_order) VALUES
(1, 1, 'overview', 'Dashboard Overview', '/dashboard', 1),
(2, 2, 'employee-list', 'Employee List', '/employees', 1),
(3, 2, 'employee-details', 'Employee Details', '/employees/:id', 2),
(4, 3, 'department-list', 'Department List', '/departments', 1),
(5, 4, 'attendance-list', 'Attendance List', '/attendance', 1),
(6, 5, 'leave-list', 'Leave List', '/leave', 1),
(7, 6, 'payroll-list', 'Payroll List', '/payroll', 1),
(8, 7, 'login', 'Login Page', '/login', 1);
SELECT setval('features_id_seq', (SELECT MAX(id) FROM features));

-- Insert 3NF Feature Specifications
INSERT INTO feature_specifications (id, feature_id, key, display_name) VALUES
(1, 2, 'manage-employees', 'Manage Employees'),
(2, 3, 'add-document', 'Add Employee Document'),
(3, 4, 'manage-departments', 'Manage Departments'),
(4, 5, 'manage-attendance', 'Manage Attendance'),
(5, 6, 'manage-leave', 'Manage Leave Requests'),
(6, 7, 'manage-payroll', 'Manage Payroll'),
(7, 8, 'login-form', 'User Login Form'),
(8, 4, 'add-department', 'Add Department'),
(9, 2, 'add-employee', 'Add Employee'),
(10, 5, 'manual-entry-correction', 'Manual Entry / Correction'),
(11, 6, 'request-leave', 'Request Leave'),
(12, 7, 'process-payroll', 'Process Payroll');
SELECT setval('feature_specifications_id_seq', (SELECT MAX(id) FROM feature_specifications));

-- Insert Help Headers (Module, Feature, or Feature Specification levels)
-- 1. Dashboard Context -> Module-level help (module_id = 1)
INSERT INTO help_headers (id, module_id, title) VALUES
(1, 1, 'Quick steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(1, 1, 'Use the sidebar to open the module you need.'),
(1, 2, 'Review the dashboard overview and current system information.'),
(1, 3, 'Open Employees, Departments, Attendance, Leave, or Payroll as needed.'),
(1, 4, 'Use the ⓘ Need help? beside a function whenever you need guidance.');

-- 2. Employees Context -> Feature-level help (feature_id = 2 [employee-list])
INSERT INTO help_headers (id, feature_id, title) VALUES
(2, 2, 'Quick steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(2, 1, 'Open Employees from the sidebar.'),
(2, 2, 'Click Add Employee to open the employee registration form.'),
(2, 3, 'Enter the required personal and employment information.'),
(2, 4, 'Select the employee’s department and position.'),
(2, 5, 'Click Save Employee to complete registration.');

-- 3. Employee Details / Add Document Context -> Feature-specification-level help (feature_specification_id = 2 [add-document])
INSERT INTO help_headers (id, feature_specification_id, title) VALUES
(3, 2, 'Document upload steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(3, 1, 'Navigate to the target Employee Profile page.'),
(3, 2, 'Click the Documents tab or Add Document button.'),
(3, 3, 'Select the document type and file from your computer.'),
(3, 4, 'Click Upload Document to attach it to the employee profile.');

-- 4. Departments Context -> Feature-level help (feature_id = 4 [department-list])
INSERT INTO help_headers (id, feature_id, title) VALUES
(4, 4, 'Quick steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(4, 1, 'Open Departments from the sidebar.'),
(4, 2, 'Click Add Department.'),
(4, 3, 'Enter the department name and required information.'),
(4, 4, 'Review the department details.'),
(4, 5, 'Save the department.');

-- 5. Attendance Context -> Feature-level help (feature_id = 5 [attendance-list])
INSERT INTO help_headers (id, feature_id, title) VALUES
(5, 5, 'Quick steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(5, 1, 'Open Attendance from the sidebar.'),
(5, 2, 'Select the employee whose attendance you want to record.'),
(5, 3, 'Select the correct attendance status.'),
(5, 4, 'Check the attendance date and details.'),
(5, 5, 'Save the attendance record.');

-- 6. Leave Context -> Feature-level help (feature_id = 6 [leave-list])
INSERT INTO help_headers (id, feature_id, title) VALUES
(6, 6, 'Quick steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(6, 1, 'Open Leave Management from the sidebar.'),
(6, 2, 'Click New Request.'),
(6, 3, 'Select the employee and leave type.'),
(6, 4, 'Select the start and end dates.'),
(6, 5, 'Submit the leave request.');

-- 7. Payroll Context -> Feature-level help (feature_id = 7 [payroll-list])
INSERT INTO help_headers (id, feature_id, title) VALUES
(7, 7, 'Quick steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(7, 1, 'Open Payroll from the sidebar.'),
(7, 2, 'Review the employee salary information.'),
(7, 3, 'Verify the payroll details before processing.'),
(7, 4, 'Check the calculated payroll information.'),
(7, 5, 'Process payroll according to your organization workflow.');

-- 8. Login Context -> Feature-specification-level help (feature_specification_id = 7 [login-form])
INSERT INTO help_headers (id, feature_specification_id, title) VALUES
(8, 7, 'Quick steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(8, 1, 'Enter your username or email.'),
(8, 2, 'Enter your password.'),
(8, 3, 'Click Login.'),
(8, 4, 'The system validates your credentials.'),
(8, 5, 'If successful, you are redirected to the Dashboard.');

-- 9. Add Department Form -> Feature-specification-level help (feature_specification_id = 8 [add-department])
INSERT INTO help_headers (id, feature_specification_id, title) VALUES
(9, 8, 'Add Department steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(9, 1, 'Click the ➕ Add Department button on the Departments Management page.'),
(9, 2, 'Enter the required Department Name (e.g. Information Technology) and Department Code (e.g. IT).'),
(9, 3, 'Optionally provide a short Description detailing unit functions and responsibilities.'),
(9, 4, 'Click Create Department to save the new department record.');

-- 10. Add Employee Form -> Feature-specification-level help (feature_specification_id = 9 [add-employee])
INSERT INTO help_headers (id, feature_specification_id, title) VALUES
(10, 9, 'Add Employee steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(10, 1, 'Click the ➕ Add Employee button on the Employee Directory page.'),
(10, 2, 'Enter the employee’s First Name, Last Name, and mandatory Email Address.'),
(10, 3, 'Select the assigned Department and enter their Position / Job Title.'),
(10, 4, 'Set the account Status to Active or Inactive.'),
(10, 5, 'Click Create Employee to finalize registration.');

-- 11. Manual Entry / Correction Form -> Feature-specification-level help (feature_specification_id = 10 [manual-entry-correction])
INSERT INTO help_headers (id, feature_specification_id, title) VALUES
(11, 10, 'Attendance log steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(11, 1, 'Click the ➕ Manual Entry / Correction button on the Attendance page.'),
(11, 2, 'Select the target Employee from the dropdown menu.'),
(11, 3, 'Enter or adjust the Check In Time and Check Out Time fields.'),
(11, 4, 'Choose the appropriate attendance Status (Present, Late, On Leave, or Absent).'),
(11, 5, 'Click Save Record to update the employee attendance entry.');

-- 12. Request Leave Form -> Feature-specification-level help (feature_specification_id = 11 [request-leave])
INSERT INTO help_headers (id, feature_specification_id, title) VALUES
(12, 11, 'Leave request steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(12, 1, 'Click the ➕ Request Leave button on the Leave Management page.'),
(12, 2, 'Select the applying Employee and choose the appropriate Leave Type.'),
(12, 3, 'Specify the Start Date and End Date for the requested leave duration.'),
(12, 4, 'Enter a detailed Reason for Leave explaining the request justification.'),
(12, 5, 'Click Submit Request to send for manager approval.');

-- 13. Process Payroll Form -> Feature-specification-level help (feature_specification_id = 12 [process-payroll])
INSERT INTO help_headers (id, feature_specification_id, title) VALUES
(13, 12, 'Payroll process steps');

INSERT INTO help_details (help_header_id, step_number, step_text) VALUES
(13, 1, 'Click the 💰 Process Payroll button on the Payroll & Salary Processing page.'),
(13, 2, 'Select the target Employee and confirm the Pay Period (e.g. 2026-09).'),
(13, 3, 'Enter the Base Salary ($) along with any applicable Allowances ($).'),
(13, 4, 'Enter total Tax & Pension Deductions ($) to preview the Net Salary calculation.'),
(13, 5, 'Click Process Salary to generate and record the monthly payslip statement.');

SELECT setval('help_headers_id_seq', (SELECT MAX(id) FROM help_headers));




