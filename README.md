# Purchasely

A purchase order management system. Handles the flow from requisition to approval to purchase order to receiving, with suppliers, products, and users along the way.

## Stack

**Backend** — .NET, organized as Clean Architecture (API / Application / Domain / Infrastructure), EF Core with PostgreSQL, SignalR for real-time notifications, RabbitMQ for async messaging, Redis for caching, Azure Blob Storage (Azurite locally) for file storage.

**Frontend** — React + TypeScript + Vite, TanStack Query for data fetching, React Hook Form + Zod for forms, Tailwind for styling.

## Project structure

```
backend/
  Purchasely.API/            entry point, controllers, hubs
  Purchasely.Application/    use cases, DTOs, interfaces
  Purchasely.Domain/         entities, enums
  Purchasely.Infrastructure/ EF Core, repositories, migrations, external services
  Purchasely.UnitTests/

frontend/
  src/
    api/          axios calls per resource
    components/   UI components, grouped by feature
    hooks/        TanStack Query hooks
    pages/        routed pages
    schemas/      zod validation schemas
    types/
```

Core domain areas: requisitions, purchase orders, suppliers, products, users, and notifications (with a dashboard tying stats together).

## Running locally

### 1. Infrastructure

Spin up Postgres, Redis, RabbitMQ, Azurite, and MailHog:

```bash
docker compose up -d
```

Create a `.env` at the repo root with the Postgres credentials used by docker-compose:

```
POSTGRES_USER=purchasely_user
POSTGRES_PASSWORD=purchasely_password
POSTGRES_DB=purchasely_db
```

### 2. Backend

Add `backend/Purchasely.API/appsettings.Development.json` (gitignored) with your connection strings and secrets — connection string for Postgres, a JWT secret/issuer/audience, RabbitMQ credentials, the Azurite connection string, and Redis's connection string. Then:

```bash
cd backend
dotnet run --project Purchasely.API
```

Runs at `http://localhost:5206`. Migrations run automatically on startup, and in Development the database gets seeded and the OpenAPI/Scalar docs are exposed.

### 3. Frontend

```bash
cd frontend
npm install
npm run dev
```

The frontend expects the API at the URL set in `.env.development` (`VITE_API_URL`), defaults to `http://localhost:5206`.

## Tests

```bash
cd backend
dotnet test
```
