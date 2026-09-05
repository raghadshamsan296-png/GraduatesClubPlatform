# Swagger Test Checklist

Run **GraduatesClub.API** in the **Development** environment. Swagger opens at `/swagger`.

The project uses SQL Server LocalDB automatically:

- Server: `(localdb)\MSSQLLocalDB`
- Database: `GraduatesClubPlatformDb`

The database and tables are created automatically the first time the API runs.

## 1. Department

### POST `/api/Department`

```json
{
  "name": "Computer Science"
}
```

Expected: **201 Created**.

### GET `/api/Department`

Expected: **200 OK** and the created department.

## 2. Alumni

### POST `/api/Alumni`

Use the department id created above.

```json
{
  "fullName": "Test Graduate",
  "email": "graduate@example.com",
  "phone": "777000000",
  "graduationYear": 2025,
  "departmentId": 1
}
```

Expected: **201 Created**.

Invalid `departmentId`: **400 Bad Request**.
Duplicate email: **409 Conflict**.

## 3. Event

### POST `/api/Event`

Use the alumni id created above.

```json
{
  "title": "Graduates Meeting",
  "description": "Swagger integration test",
  "eventDate": "2026-08-21T12:00:00",
  "alumniId": 1
}
```

Expected: **201 Created**.

Invalid `alumniId`: **400 Bad Request**.

## 4. Update tests

PUT requires the route id and body id to match.

Example: `PUT /api/Department/1`

```json
{
  "id": 1,
  "name": "Information Technology"
}
```

Expected: **204 No Content**.

A route/body id mismatch returns **400 Bad Request**.
A missing entity returns **404 Not Found**.

## 5. Delete tests

Delete an event first, then its alumni, then its department.

The relationships use `Restrict`, so deleting a parent that still has related records returns **409 Conflict** instead of silently deleting dependent data.

## Expected status codes

| Scenario | Status |
|---|---:|
| Valid GET | 200 |
| Valid POST | 201 |
| Valid PUT | 204 |
| Valid DELETE | 204 |
| Invalid model / foreign key | 400 |
| Missing record | 404 |
| Duplicate / related-data conflict | 409 |
| Unexpected server failure | 500 |
