-- PostgreSQL Database Setup Script for Films Application
-- This script creates the database and initial schema

-- Create database (run this as postgres superuser)
-- CREATE DATABASE films WITH ENCODING 'UTF8' LC_COLLATE='en_US.UTF-8' LC_CTYPE='en_US.UTF-8';

-- Connect to the films database
\c films;

-- Create schema (if not using default public schema)
-- CREATE SCHEMA IF NOT EXISTS public;

-- Set search path
SET search_path TO public;

-- Enable UUID extension (if needed in future)
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Create tables (these will be created by EF Core migrations, but included here for reference)

-- Sex table
CREATE TABLE IF NOT EXISTS sex (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(200)
);

-- Type User table
CREATE TABLE IF NOT EXISTS type_user (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(200)
);

-- Users table
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL,
    email VARCHAR(200) NOT NULL,
    password_hash VARCHAR(500) NOT NULL,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    type_user_id INTEGER REFERENCES type_user(id) ON DELETE SET NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modified_date TIMESTAMP WITHOUT TIME ZONE,
    last_login_date TIMESTAMP WITHOUT TIME ZONE
);

-- Actors table
CREATE TABLE IF NOT EXISTS actors (
    id SERIAL PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    sex_id INTEGER REFERENCES sex(id) ON DELETE SET NULL,
    birth_date TIMESTAMP WITHOUT TIME ZONE,
    biography VARCHAR(2000),
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modified_date TIMESTAMP WITHOUT TIME ZONE
);

-- Directors table
CREATE TABLE IF NOT EXISTS directors (
    id SERIAL PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    sex_id INTEGER REFERENCES sex(id) ON DELETE SET NULL,
    birth_date TIMESTAMP WITHOUT TIME ZONE,
    biography VARCHAR(2000),
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modified_date TIMESTAMP WITHOUT TIME ZONE
);

-- Films table
CREATE TABLE IF NOT EXISTS films (
    id SERIAL PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    description VARCHAR(2000),
    year INTEGER,
    genre VARCHAR(100),
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modified_date TIMESTAMP WITHOUT TIME ZONE
);

-- Rights table
CREATE TABLE IF NOT EXISTS rights (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(200),
    code VARCHAR(50)
);

-- User Rights junction table
CREATE TABLE IF NOT EXISTS user_rights (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    right_id INTEGER NOT NULL REFERENCES rights(id) ON DELETE CASCADE,
    granted_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Actor-Film junction table
CREATE TABLE IF NOT EXISTS ref_af (
    id SERIAL PRIMARY KEY,
    actor_id INTEGER NOT NULL REFERENCES actors(id) ON DELETE CASCADE,
    film_id INTEGER NOT NULL REFERENCES films(id) ON DELETE CASCADE,
    role VARCHAR(100)
);

-- Director-Film junction table
CREATE TABLE IF NOT EXISTS ref_daf (
    id SERIAL PRIMARY KEY,
    director_id INTEGER NOT NULL REFERENCES directors(id) ON DELETE CASCADE,
    film_id INTEGER NOT NULL REFERENCES films(id) ON DELETE CASCADE
);

-- Create indexes for better performance
CREATE INDEX IF NOT EXISTS idx_films_title ON films(title);
CREATE INDEX IF NOT EXISTS idx_films_year ON films(year);
CREATE INDEX IF NOT EXISTS idx_films_genre ON films(genre);

CREATE INDEX IF NOT EXISTS idx_actors_last_name ON actors(last_name);
CREATE INDEX IF NOT EXISTS idx_actors_first_name ON actors(first_name);

CREATE INDEX IF NOT EXISTS idx_directors_last_name ON directors(last_name);
CREATE INDEX IF NOT EXISTS idx_directors_first_name ON directors(first_name);

CREATE INDEX IF NOT EXISTS idx_users_username ON users(username);
CREATE INDEX IF NOT EXISTS idx_users_email ON users(email);
CREATE INDEX IF NOT EXISTS idx_users_type_user_id ON users(type_user_id);

CREATE INDEX IF NOT EXISTS idx_user_rights_user_id ON user_rights(user_id);
CREATE INDEX IF NOT EXISTS idx_user_rights_right_id ON user_rights(right_id);

CREATE INDEX IF NOT EXISTS idx_ref_af_actor_id ON ref_af(actor_id);
CREATE INDEX IF NOT EXISTS idx_ref_af_film_id ON ref_af(film_id);

CREATE INDEX IF NOT EXISTS idx_ref_daf_director_id ON ref_daf(director_id);
CREATE INDEX IF NOT EXISTS idx_ref_daf_film_id ON ref_daf(film_id);

-- Insert initial data for Sex
INSERT INTO sex (name, description) VALUES 
    ('Male', 'Male gender'),
    ('Female', 'Female gender'),
    ('Other', 'Other gender')
ON CONFLICT DO NOTHING;

-- Insert initial data for Type User
INSERT INTO type_user (name, description) VALUES 
    ('Admin', 'Administrator user with full access'),
    ('User', 'Regular user with limited access'),
    ('Guest', 'Guest user with read-only access')
ON CONFLICT DO NOTHING;

-- Insert initial data for Rights
INSERT INTO rights (name, description, code) VALUES 
    ('View Films', 'Can view films', 'VIEW_FILMS'),
    ('Create Films', 'Can create new films', 'CREATE_FILMS'),
    ('Edit Films', 'Can edit existing films', 'EDIT_FILMS'),
    ('Delete Films', 'Can delete films', 'DELETE_FILMS'),
    ('Manage Users', 'Can manage users', 'MANAGE_USERS')
ON CONFLICT DO NOTHING;

-- Grant permissions
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO postgres;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO postgres;

-- Create a read-only user (optional)
-- CREATE USER films_readonly WITH PASSWORD 'readonly_password';
-- GRANT CONNECT ON DATABASE films TO films_readonly;
-- GRANT USAGE ON SCHEMA public TO films_readonly;
-- GRANT SELECT ON ALL TABLES IN SCHEMA public TO films_readonly;

-- Create application user (optional)
-- CREATE USER films_app WITH PASSWORD 'app_password';
-- GRANT CONNECT ON DATABASE films TO films_app;
-- GRANT USAGE ON SCHEMA public TO films_app;
-- GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO films_app;
-- GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO films_app;

-- Verify tables
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' 
ORDER BY table_name;

-- Verify indexes
SELECT indexname, tablename 
FROM pg_indexes 
WHERE schemaname = 'public' 
ORDER BY tablename, indexname;
