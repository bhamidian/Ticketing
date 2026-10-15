# Ticketing

A support ticketing application with an ASP.NET Core Web API and a React client. Customers can create tickets, exchange messages, and optionally attach images. Support agents can browse unassigned tickets, claim tickets, and manage tickets assigned to them.

## Technology stack

- .NET 10 and ASP.NET Core Web API
- Entity Framework Core 10 with SQL Server
- ASP.NET Core Identity and JWT access tokens with refresh-token cookies
- React 19, TypeScript, and Vite 8
- Serilog console and Seq sinks

## Repository layout

```text
src/
├── 00-Framework/                 Shared framework utilities
├── 01-Domain/                    Entities and domain rules
├── 02-Application/               Use cases, commands, queries, and DTOs
├── 03-Infrastructue/             EF Core, JWT, and file-service implementations
├── 04-Endpoint/                  ASP.NET Core Web API
└── 05-Client/                    React and TypeScript application
```

## Prerequisites

- .NET 10 SDK
- Node.js and npm
- SQL Server
- `dotnet-ef` for applying database migrations

Install the EF Core CLI tool if it is not already available:

```powershell
dotnet tool install --global dotnet-ef
```

## Configuration

The API reads its database connection from `ConnectionStrings:DefaultConnection` and JWT settings from the `Jwt` section (`Issuer`, `Audience`, and `SigningKey`). Configure these values through user secrets or environment variables. For a PowerShell development session, for example:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Database=Ticketing;Trusted_Connection=True;TrustServerCertificate=True;"
$env:Jwt__Issuer = "Ticketing"
$env:Jwt__Audience = "TicketingClient"
$env:Jwt__SigningKey = "replace-with-a-long-random-development-key"
```

Use a private, sufficiently long signing key and never commit real credentials. Development account seeding is optional; it runs only in the Development environment when `SeedUsers:Enabled` is set to `true` and a database connection is configured.

## Database setup

From the repository root, apply the existing EF Core migrations:

```powershell
dotnet ef database update `
  --project src/03-Infrastructue/Ticketing.Infrastructure.Efcore `
  --startup-project src/04-Endpoint/Ticketing.Endpoint.Webapi
```

## Run the API

From the repository root:

```powershell
dotnet run --project src/04-Endpoint/Ticketing.Endpoint.Webapi --launch-profile https
```

The HTTPS API listens at `https://localhost:7180` (HTTP: `http://localhost:5115`). In Development, Swagger UI is available at `https://localhost:7180/swagger`.

## Run the client

In a second terminal:

```powershell
cd src/05-Client
npm install
npm run dev
```

The Vite development server proxies `/api` requests to the local HTTPS API at `https://localhost:7180`. Set `VITE_API_BASE_URL` in `.env.local` only when the client needs to call a different API origin directly.

## Main API routes

All routes are under `/api`.

| Route | Purpose |
| --- | --- |
| `POST /Auth/register` | Register a customer account |
| `POST /Auth/login` | Sign in and receive tokens |
| `POST /Auth/refresh` | Refresh the access token using the refresh-token cookie |
| `POST /Auth/logout` | Revoke the refresh token and clear the cookie |
| `GET /Ticket` | List tickets with pagination, status/priority filters, sort order, and agent assignment filters |
| `POST /Ticket` | Create a ticket; an optional image can be uploaded as multipart form data |
| `GET /Ticket/{ticketId}` | View ticket details and messages |
| `POST /Ticket/{ticketId}/claim` | Claim an available ticket as an agent |
| `POST /Ticket/{ticketId}/messages` | Add a message to a ticket |
| `POST /Ticket/{ticketId}/close` | Close a ticket |
| `PATCH /Ticket/{ticketId}/status` | Change a ticket's status |
| `GET /Ticket/{ticketId}/messages/{messageId}/image` | Retrieve an attached ticket image |

Ticket routes require authentication. Authorization and visibility depend on the caller's role and ticket assignment.

## File uploads

Ticket images are optional, limited to 5 MB, and stored under the API's `wwwroot/Teams/Tickets` directory. The API serves an image only to callers authorized to view its ticket.
