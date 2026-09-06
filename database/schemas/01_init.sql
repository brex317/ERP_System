-- PostgreSQL Initialization Script for RARAS Employee Management System (EMS)

-- Create extension for UUID generation if needed
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- 1. DEPARTMENTS TABLE
CREATE TABLE IF NOT EXISTS departments (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    code VARCHAR(20) NOT NULL UNIQUE,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

    
-- 2. EMPLOYEES TABLE
CREATE TABLE IF NOT EXISTS employees (
    id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    department_id INT REFERENCES departments(id) ON DELETE SET NULL,
    position VARCHAR(100) NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'Active',
    hire_date DATE NOT NULL DEFAULT CURRENT_DATE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 3. ATTENDANCE TABLE
CREATE TABLE IF NOT EXISTS attendance (
    id SERIAL PRIMARY KEY,
    employee_id INT NOT NULL REFERENCES employees(id) ON DELETE CASCADE,
    date DATE NOT NULL DEFAULT CURRENT_DATE,
    status VARCHAR(20) NOT NULL CHECK (status IN ('Present', 'Absent', 'Late', 'On Leave')),
    check_in TIME,
    check_out TIME,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT unique_employee_date UNIQUE(employee_id, date)
);

-- 4. LEAVE REQUESTS TABLE
CREATE TABLE IF NOT EXISTS leave_requests (
    id SERIAL PRIMARY KEY,
    employee_id INT NOT NULL REFERENCES employees(id) ON DELETE CASCADE,
    leave_type VARCHAR(50) NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK (status IN ('Pending', 'Approved', 'Rejected')),
    reason TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 5. MODULES TABLE
CREATE TABLE IF NOT EXISTS modules (
    id SERIAL PRIMARY KEY,
    key VARCHAR(100) NOT NULL UNIQUE,
    display_name VARCHAR(150) NOT NULL,
    icon VARCHAR(50),
    sort_order INT NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);

-- 6. PAGES TABLE
CREATE TABLE IF NOT EXISTS pages (
    id SERIAL PRIMARY KEY,
    module_id INT NOT NULL REFERENCES modules(id) ON DELETE CASCADE,
    key VARCHAR(100) NOT NULL,
    display_name VARCHAR(150) NOT NULL,
    route_path VARCHAR(255),
    sort_order INT NOT NULL DEFAULT 0,
    CONSTRAINT unique_module_page UNIQUE(module_id, key)
);

-- 7. FUNCTIONALITIES TABLE
CREATE TABLE IF NOT EXISTS functionalities (
    id SERIAL PRIMARY KEY,
    page_id INT NOT NULL REFERENCES pages(id) ON DELETE CASCADE,
    key VARCHAR(100) NOT NULL,
    display_name VARCHAR(150) NOT NULL,
    CONSTRAINT unique_page_functionality UNIQUE(page_id, key)
);

-- 8. HELP CONTEXTS TABLE
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

-- 9. HELP STEPS TABLE
CREATE TABLE IF NOT EXISTS help_steps (
    id SERIAL PRIMARY KEY,
    help_context_id INT NOT NULL REFERENCES help_contexts(id) ON DELETE CASCADE,
    step_number INT NOT NULL,
    step_text TEXT NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT unique_help_context_step UNIQUE(help_context_id, step_number)
);

-- 7. ROLES TABLE
CREATE TABLE IF NOT EXISTS roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 8. USERS TABLE
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


