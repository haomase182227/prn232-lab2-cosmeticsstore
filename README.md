# Cosmetics Store API

REST API for cosmetics store management with .NET 9.0, Entity Framework Core, and JWT authentication.

## Tech Stack

- .NET 9.0 Web API
- Entity Framework Core (SQL Server)
- JWT Bearer Authentication
- Docker & Docker Compose
- Swagger/OpenAPI

## Architecture

- **3-Layer Architecture**: API → Services → Repositories
- **Unit of Work Pattern**: Transaction management
- **Repository Pattern**: Data access abstraction
- **Model Types**: Entity (DB) → Business (Service) → Request/Response (API)

## Quick Start

### With Docker (Recommended)

```bash
docker-compose up -d
```

API: http://localhost:5282/swagger

### Local Development

1. Update connection string in `appsettings.json`
2. Run migrations:
```bash
cd CosmeticsStore.API
dotnet ef database update --project ../CosmeticsStore.Repositories
```
3. Run API:
```bash
dotnet run
```

## API Endpoints

### Authentication
- `POST /api/auth/login` - Login with email & password

### Cosmetics
- `GET /api/cosmetics` - Get all cosmetics (with filtering, sorting, paging)
- `GET /api/cosmetics/{id}` - Get cosmetic by ID
- `POST /api/cosmetics` - Create new cosmetic (requires auth)
- `PUT /api/cosmetics/{id}` - Update cosmetic (requires auth)
- `DELETE /api/cosmetics/{id}` - Delete cosmetic (requires auth)

### Categories
- `GET /api/categories` - Get all categories

## Default Credentials

```json
{
  "email": "admin@CosmeticsDB.info",
  "password": "@1"
}
```

## Environment Variables

```env
ConnectionStrings__DefaultConnectionString=Server=localhost;Database=CosmeticsDB;...
JWT__SecretKey=your-secret-key
JWT__Issuer=your-issuer
JWT__Audience=your-audience
```

## Docker Configuration

- **SQL Server**: Port 1434
- **API**: Port 5282

## REST API Standards

- URLs: lowercase with hyphens (`/api/cosmetics`)
- Query params: kebab-case (`?search-term=value`)
- JSON: camelCase (`cosmeticName`, `skinType`)
- Authentication: JWT Bearer token required for POST/PUT/DELETE
