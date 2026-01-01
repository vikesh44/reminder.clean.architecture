
# ReminderApp Server (.NET 8 Web API)

## Prerequisites
- .NET SDK 8.0+
- SQL Server Express (default instance `\SQLEXPRESS`) or any SQL Server reachable

## Quick Start
1. **Update secrets** in `appsettings.json`:
   - Set a strong `Jwt:Secret`
   - Optional: change `ConnectionStrings:DefaultConnection` if your SQL Server instance differs.
2. Open a terminal in this `server` folder and run:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```
   The API will listen on `http://localhost:5080`.

3. **Setup Database** (first time only):
> Since Entity Framework migrations are not used, you need to create the database schema manually:

    1. Ensure SQL Server is running
    2. Run the SQL script: `Database\CreateSchema.sql`
    3. This will create:
        - `ReminderAppDb` database
        - `Users` table with unique email index
        - `Reminders` table with foreign key to Users

## Swagger
Browse `http://localhost:5080/swagger` to try endpoints.

## Endpoints
- `POST /api/auth/register` { name, email, password }
- `POST /api/auth/login` { email, password } → { token }
- `GET /api/reminders` (Bearer token)
- `POST /api/reminders` { text, scheduledAtUtc } (Bearer token)
- `DELETE /api/reminders/{id}` (Bearer token)

## CORS
The API allows `http://localhost:5173` (Vite dev server) by default.
