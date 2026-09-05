# Graduates Club Platform - ASP.NET Core 6 Web API + MVC

This solution contains a complete ASP.NET Core 6 MVC application and a separate REST Web API.

## Architecture

- `GraduatesClub.Domain` - entities and repository contracts only.
- `GraduatesClub.Application` - DTOs, service contracts, business services, and application DI.
- `GraduatesClub.Infrastructure` - EF Core, SQL Server, entity configuration, repositories, infrastructure DI.
- `GraduatesClub.API` - controllers, Swagger, global exception middleware, and startup.

## Database

The API is configured for Visual Studio SQL Server LocalDB:

`Server=(localdb)\MSSQLLocalDB;Database=GraduatesClubPlatformDb;...`

`EnsureCreatedAsync()` creates the database and current schema automatically on first run.

You can inspect it in Visual Studio:

`View -> SQL Server Object Explorer -> SQL Server -> (localdb)\MSSQLLocalDB -> Databases -> GraduatesClubPlatformDb`

## Run

1. Open `GraduatesClub.sln` in Visual Studio 2022.
2. Right-click `GraduatesClub.Web`, select **Set as Startup Project**, and choose the `GraduatesClub.Web` launch profile.
3. Restore NuGet packages if Visual Studio asks.
4. Build the solution.
5. Run `GraduatesClub.Web` with its HTTPS profile (7064). The public MVC home page opens automatically.
6. Use **دخول الإدارة** to sign in. With the combined launch profile, the API runs silently on port 7158 for Postman while only the MVC website opens in the browser. Swagger remains available manually at `https://localhost:7158/swagger`.
7. Follow `SWAGGER_TEST.md`.
8. Optional: with the API running, execute `scripts\swagger-smoke-test.ps1` to run an end-to-end CRUD smoke test.

## Important behavior

- Validation errors return 400.
- Unknown foreign keys return 400.
- Duplicate department names and alumni emails return 409.
- Attempts to delete a department with alumni, or alumni with events, return 409.
- Missing ids return 404.
- Unhandled exceptions return a safe 500 response and are logged server-side.
- MVC and API use the same SQL Server LocalDB database and can be started independently.
- MVC jobs, announcements, profile settings, and contact messages are stored permanently in SQL Server.
- MVC is self-contained and can run by itself; it applies migrations and uses the database directly.
- This version uses `GraduatesClubCompleteDb` to avoid conflicts with databases created by older builds.

## Initial login

- Email: `admin@graduates.local`
- Password: `Admin@123`
- Change the password from the profile settings after the first login.

## Complete modules

- Departments, alumni, events, jobs, announcements, contact messages, and users: Index, Details, Create, Edit, and Delete pages.
- Alumni images: validated upload (JPG/PNG/WEBP, maximum 2 MB), persistent path, and display in the list/details pages.
- Public home page: displays live database statistics, upcoming events, jobs, and announcements.
- DataTables: search, pagination, and Arabic labels on all administration lists.
- Bootstrap Modal, Bootstrap Icons, Cairo font, and CSS variables are enabled in the MVC interface.
- Jobs and announcements: full MVC CRUD plus REST API endpoints.
- Contact messages: public submission, protected management page, and API endpoints.
- User profiles: persistent profile API and password-protected MVC login.
- Dashboard: live counts from the database.

## Refactoring performed

- Removed the old `Class1.cs.csproj` project naming and the `Graduatesclup` typo.
- Renamed `DT0s` to `DTOs`.
- Removed unused/backup/template code from the supplied project.
- Centralized application and infrastructure dependency injection.
- Moved exception handling out of `Program.cs` into middleware.
- Split EF Core entity configuration into focused configuration classes.
- Changed the domain event entity to `ClubEvent` while preserving the `Events` database table and `/api/Event` endpoint.
- Added cancellation token support through controllers, services, repositories, and EF calls.
- Added duplicate checks and database uniqueness constraints.
- Changed cascade deletes to restricted deletes to avoid accidental data loss.
- Kept the existing public Swagger endpoint names and request shapes to avoid unnecessary breaking changes.

## Scope note

This refactoring intentionally fixes and organizes the backend that was actually supplied: Departments, Alumni, and Events. It does not invent missing modules from the written report such as authentication, jobs, notifications, admin roles, or attendance.
