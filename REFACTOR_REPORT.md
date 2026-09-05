# Refactor Report

## Main problems found in the supplied project

1. Broken and inconsistent project/folder names (`Class1.cs.csproj`, `Graduatesclup`, `DT0s`).
2. Unused template/backup code and unrelated files.
3. Event service/controller dependency mismatch in the original source.
4. Event service methods were not implemented in the original source.
5. Database configuration was incomplete in the original source.
6. Dependency injection responsibilities were scattered.
7. EF Core entity mapping was all inside `AppDbContext` instead of focused configurations.
8. No consistent global error response behavior.
9. Foreign-key failures could surface as opaque server/database errors.
10. Duplicate department names and alumni emails were not protected.
11. Cascade delete could remove related data unexpectedly.
12. Repository save semantics used a boolean result, which made update behavior less clear.
13. Unused repository methods and mixed read/update tracking concerns made the code harder to reason about.
14. Public API methods did not pass cancellation tokens through to EF Core.

## Refactoring completed

- Normalized all project and folder names.
- Kept four clear layers: Domain, Application, Infrastructure, API.
- Centralized service DI in Application and repository/database DI in Infrastructure.
- Added focused EF Core configurations for Department, Alumni, and ClubEvent.
- Kept the database table name `Events` and the public `/api/Event` endpoint.
- Added LocalDB configuration for `GraduatesClubPlatformDb`.
- Added automatic first-run database creation with `EnsureCreatedAsync()`.
- Added global exception middleware using RFC-style `ProblemDetails` responses.
- Added DataAnnotations validation and service-level normalization.
- Added foreign-key validation before inserts/updates.
- Added duplicate checks and unique database indexes for department name and alumni email.
- Replaced cascade deletes with restricted deletes.
- Separated detailed read queries from tracked update queries.
- Added cancellation token support end-to-end.
- Added Swagger response metadata.
- Added a Swagger test checklist and PowerShell CRUD smoke-test script.

## Static QA performed in the working environment

- All JSON files parsed successfully.
- All `.csproj` files parsed successfully as XML.
- All project references resolve to existing files.
- All solution project paths resolve correctly.
- No old template/broken source artifacts remain in code/config.
- Basic C# delimiter checks passed for all C# files.
- DI registrations were checked for all controllers/services/repositories.
- LocalDB connection string and startup database initialization were checked.
- ZIP CRC/integrity test passed.

## Runtime limitation

The execution sandbox does not expose the process information required by the .NET CLI and does not provide SQL Server LocalDB, so a live build and Swagger/database run could not be completed here. JSON/XML validation and static source checks were completed. The included `scripts/swagger-smoke-test.ps1` performs a real end-to-end CRUD smoke test on Windows after both projects start.

## API and MVC integration fixes

- Enabled database migrations during API and MVC startup.
- Removed invalid `object` properties from `AppDbContext` that EF Core attempted to map.
- Registered a named `HttpClient` in MVC and moved the API base URL to configuration.
- Corrected the MVC endpoint from `/api/Events` to `/api/Event`.
- Completed the event form payload with description and alumni id so API validation succeeds.
- Added visible API connection and validation errors instead of silently swallowing exceptions.
- Loaded departments for the alumni form and included departments in the alumni list query.
- Added working Department details, edit, and delete actions with conflict handling.
- Restored the missing Job controller and separated it from the Login controller.
- Added antiforgery validation and initialized nullable-reference properties.
- Added persistent SQL Server entities and MVC workflows for jobs, announcements, profile settings, and contact messages.
