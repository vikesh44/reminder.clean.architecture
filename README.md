
# Reminder App (Clean Architecture) — React + .NET 8 + SQL Server Express

This repository contains a minimal, runnable example implementing Clean Architecture:
- **Front end**: React (Vite)
- **Backend API**: ASP.NET Core 8 (Controllers)
- **Database**: SQL Server Express / SQL Server

## How to Run

### 1) Database setup

```bash
Run the SQL script: `server\Database\CreateSchema.sql`
```

### 2) Backend API
```bash
cd server
# edit appsettings.json: set Jwt secret and connection string as needed
dotnet restore
# apply migration and create DB (first time)
dotnet tool install --global dotnet-ef
dotnet ef database update
# run
dotnet run
```
The API listens on `http://localhost:5080` and exposes Swagger at `/swagger`.

### 3) Frontend
```bash
cd ../client
npm install
npm run dev
```
Open `http://localhost:5173`.

Log in / Register, then add and delete reminders.

## Notes
- Times are stored as UTC (datetimeoffset). The UI converts local input to ISO.
- CORS is configured to allow `http://localhost:5173`.
- JWT tokens expire in 7 days.

## Clean Architecture Mapping
- **Domain**: `server/Domain` (Entities & invariants)
- **Application**: `server/Application` (Use cases + ports)
- **Interface Adapters**: `server/Controllers` (API), `client` (React)
- **Infrastructure**: `server/Infrastructure` (EF Core, JWT, hashing, clock)
