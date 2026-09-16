# APPLE WORLD SHOPPING MALL — Employee Management System

A starter/full working ASP.NET Core MVC application for Visual Studio.

## Technology
- .NET 8
- ASP.NET Core MVC
- Entity Framework Core
- SQLite (zero SQL Server setup required)
- Cookie authentication
- Local employee image uploads

## Run in Visual Studio
1. Install Visual Studio 2022 with **ASP.NET and web development**.
2. Open `AppleWorldShoppingMallEMS.csproj`.
3. Build the solution.
4. Press **F5** or Ctrl+F5.
5. The database `appleworld.db` is created automatically.

## Default administrator
Username: `admin`
Password: `Admin@123`

Change the password before real deployment.

## Employee accounts
When an employee is registered, a unique Employee ID is created, e.g. `AWM-4F2A91`.
If no initial password is supplied, the default is `Welcome@123`.

## Main features
- Employee registration/edit/deactivation
- Employee photo upload
- Departments
- Blockers restricted to male employees
- 7-day random work schedule
- Exactly 3 work days per active employee
- Employee login
- Employee schedule/profile/assessment pages
- Monthly payment due dates based on registration date
- Payment alerts and mark-paid action
- Responsive professional login/dashboard interface

## Important scheduling note
The requested department numbers are 5 Cleaners + 5 Blockers + 5 Organizers + 5 Cashiers + 10 Security = 30, while the stated total is 50. The application therefore allows more than those department targets; the targets are displayed as configurable staffing requirements. You should decide the final distribution of all 50 workers before production deployment.

## Moving to SQL Server
The application currently uses SQLite so it can run immediately without installing/configuring SQL Server. If your course requires SQL Server, replace the SQLite EF Core package/provider and connection string with Microsoft SQL Server, then use EF Core migrations.
