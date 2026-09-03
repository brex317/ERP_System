-- PostgreSQL Seed Data Script for RARAS Employee Management System (EMS)

-- Clear existing data
TRUNCATE TABLE leave_requests, attendance, employees, departments RESTART IDENTITY CASCADE;

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
