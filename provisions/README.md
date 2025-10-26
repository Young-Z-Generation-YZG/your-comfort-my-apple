# Docker Setup for KLTN Monorepo

## Quick Start

```bash
# Build and start all services
docker-compose up --build

# Start in detached mode
docker-compose up -d

# Stop all services
docker-compose down

# Rebuild specific service
docker-compose build admin
docker-compose build client
```

## Services

- **Admin**: http://localhost:3001
- **Client**: http://localhost:3000

## Architecture

- Uses Turbo for efficient monorepo builds
- Multi-stage Dockerfiles for optimized image size
- Shared network for service communication
- Dependencies built before apps
