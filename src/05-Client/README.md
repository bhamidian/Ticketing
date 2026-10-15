# Ticketing client

React, TypeScript, and Vite client for the Ticketing API.

## Run locally

1. The Vite development server proxies `/api` to `https://localhost:7180`; set `VITE_API_BASE_URL` in `.env.local` only when calling a different API origin directly.
2. Run `npm install`.
3. Run `npm run dev`.

The API's current HTTPS launch URL is `https://localhost:7180`.

## Source layout

- `src/app`: application entry and global providers.
- `src/features`: feature-owned pages, UI, hooks, API calls, and types (`auth`, `tickets`).
- `src/components/ui`: shared presentational components.
- `src/layouts`: page-level shells.
- `src/shared`: shared API client, config, hooks, types, and utilities.
- `src/assets`: imported static assets.
- `src/styles`: global styles and design tokens.
