# College Management System

A full-stack college management web application built with ASP.NET Core MVC and .NET 8.

The system provides role-based portals for administrators, faculty members, and students, supporting the management of courses, enrolments, attendance, assignments, exams, and academic results.

## Features

### Administrator

- Manage branches and courses
- Manage student and faculty profiles
- Manage course enrolments
- Manage academic records

### Faculty

- Access assigned courses
- Record student attendance
- Manage assignments
- Record assignment results
- Manage exams and exam results

### Students

- Access a dedicated student portal
- View course information
- View attendance records
- View assignments and results
- View released exam results

## Tech Stack

- **C#**
- **.NET 8**
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Core Identity**
- **Razor Views**
- **xUnit**
- **GitHub Actions**

## Architecture

The application follows the Model-View-Controller (MVC) pattern and separates the solution into:

- **Domain Layer** — core domain models and business entities
- **MVC Application** — controllers, views, services, authentication, and data access
- **Test Project** — automated tests using xUnit

The application also uses dependency injection and dedicated services for business logic such as enrolment management, faculty access, and result visibility.

## Authentication & Authorization

ASP.NET Core Identity is used for authentication and role-based authorization.

The application supports three primary roles:

- Administrator
- Faculty
- Student

Each role has access to its own functionality and portal.

## Database

Entity Framework Core is used as the ORM with SQL Server.

The application includes database migrations and seed data to initialise the system.

## Automated Testing & CI

Automated tests are implemented with **xUnit**.

A **GitHub Actions** workflow is included to support automated build and test execution.

## Running the Project Locally

### Requirements

- Visual Studio 2022
- .NET 8 SDK
- SQL Server LocalDB

### Setup

1. Clone the repository.
2. Open the solution in Visual Studio 2022.
3. Restore NuGet packages.
4. Open the Package Manager Console.
5. Run:

```powershell
Update-Database -Project CollegeManagement.Web -StartupProject CollegeManagement.Web
```

6. Build and run the application.

## Project Context

This project was developed as part of my Computer Science studies and expanded into a complete MVC-based college management application to demonstrate object-oriented programming, database integration, authentication, automated testing, and layered application design.

## Future Improvements

- Improve responsive UI
- Expand automated test coverage
- Add deployment configuration
- Add API endpoints
- Add reporting and analytics features

## Author

**Sebastian Lopez**

Computer Science Student — Dublin, Ireland
