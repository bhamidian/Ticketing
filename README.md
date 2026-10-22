# Ticketing

A support ticketing application with an ASP.NET Core Web API and a React client. Customers create tickets and exchange messages with support. Agents work only on tickets assigned to them. Administrators assign tickets and review agent replies before they are sent to customers.

## Technology stack

- .NET 10 and ASP.NET Core Web API
- Entity Framework Core 10 with SQL Server
- ASP.NET Core Identity, JWT access tokens, and refresh-token cookies
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

The API reads `ConnectionStrings:DefaultConnection` and the `Jwt` settings (`Issuer`, `Audience`, and `SigningKey`) from ASP.NET Core configuration. The development connection string is intentionally kept out of `appsettings.Development.json`; use .NET User Secrets for local values:

```powershell
$apiProject = "src/04-Endpoint/Ticketing.Endpoint.Webapi/Ticketing.Endpoint.Webapi.csproj"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=Ticketing;User Id=sa;Password=<local-password>;TrustServerCertificate=True;" --project $apiProject
dotnet user-secrets set "Jwt:Issuer" "Ticketing.Api" --project $apiProject
dotnet user-secrets set "Jwt:Audience" "Ticketing.Client" --project $apiProject
dotnet user-secrets set "Jwt:SigningKey" "<a-long-random-development-key>" --project $apiProject
```

User Secrets are loaded automatically when the API runs in the Development environment. Environment variables can also be used; for example, `ConnectionStrings__DefaultConnection` overrides the connection string. Never commit real credentials.

### Development seed accounts

When `SeedUsers:Enabled` is `true` in Development, startup creates the configured development accounts. The seeder currently expects three accounts for each role: `User`, `Agent`, and `Admin`. Configure their emails and development-only passwords under `SeedUsers:Accounts` in `appsettings.Development.json` or through another configuration provider. The seeded normal users belong to the `پشتیبانی` department; newly registered normal users are assigned to that department as well.

## Database setup

From the repository root, apply the EF Core migrations:

```powershell
dotnet ef database update `
  --project src/03-Infrastructue/Ticketing.Infrastructure.Efcore `
  --startup-project src/04-Endpoint/Ticketing.Endpoint.Webapi
```

The database needs to be migrated before the development seeder or registration flow runs. The migration creates the admin and department tables, assigns departments to existing normal users, and adds the agent-message approval state.

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

## Roles and ticket workflow

- **User:** belongs to one department, can create tickets, and sees only their own tickets. Replies from agents are visible after an administrator approves them.
- **Agent:** sees and replies only to tickets assigned to that agent. New agent replies stay pending until an administrator reviews them.
- **Admin:** sees the ticket queue, assigns tickets to agents, views agent response counts, and can edit or approve pending agent replies. Approval sends the reply to the customer.
- A department can have multiple normal users. The ticket list and details show the requester's department to agents and administrators.

## Main API routes

All routes are under `/api`.

| Route | Purpose |
| --- | --- |
| `POST /Auth/register` | Register a normal user account |
| `POST /Auth/login` | Sign in and receive an access token and refresh-token cookie |
| `POST /Auth/refresh` | Rotate the refresh token and issue a new access token |
| `POST /Auth/logout` | Revoke the refresh-token family and clear the cookie |
| `GET /Profile/me` | Get the signed-in user's profile and department |
| `GET /Ticket` | List tickets with pagination, status/priority filters, and sorting |
| `POST /Ticket` | Create a ticket with an optional image attachment |
| `GET /Ticket/{ticketId}` | View an authorized ticket and its visible messages |
| `POST /Ticket/{ticketId}/messages` | Add a message; agent replies require admin approval |
| `POST /Ticket/{ticketId}/close` | Close a ticket |
| `PATCH /Ticket/{ticketId}/status` | Change the status of an assigned ticket |
| `GET /Ticket/{ticketId}/messages/{messageId}/image` | Retrieve an authorized ticket image |
| `GET /Admin/agents` | List agents and their responded-ticket counts |
| `POST /Admin/tickets/{ticketId}/assign` | Assign or reassign a ticket to an agent |
| `PATCH /Admin/tickets/{ticketId}/messages/{messageId}` | Edit a pending agent reply |
| `PATCH /Admin/messages/{messageId}/confirm` | Approve and publish a pending agent reply |

Ticket routes require authentication. Admin routes require the `Admin` role. Ticket and message visibility is also enforced by the API according to the caller's role and assignment.

## File uploads

Ticket images are optional, limited to 5 MB, and stored under the API's `wwwroot/Teams/Tickets` directory. The API serves an image only to callers authorized to view its ticket.
