# Calculator Microservices

Simple calculator built with microservices to demonstrate REST and gRPC communication.

## Architecture
```
Client → API Gateway (Node.js) → Calculator (Go) → History (.NET) → PostgreSQL
                ↓                       ↓ (gRPC)        ↑
            (REST)                  ──────────────────┘
```

## Services

- **API Gateway** (Node.js :3000) - Entry point for clients
- **Calculator** (Go :4000) - Does the math, sends results via gRPC
- **History** (.NET :5000, :5001) - Saves results to PostgreSQL

## Tech Stack

- Node.js + Express
- Go + net/http
- .NET 10 + EF Core
- PostgreSQL
- gRPC

## Run It

**Terminal 1:**
```bash
cd HistoryService
dotnet run
```

**Terminal 2:**
```bash
cd calculator-service
go run main.go
```

**Terminal 3:**
```bash
cd api-gateway
node server.js
```

## Test It
```bash
# Calculate
curl -X POST http://localhost:3000/calculate \
-H "Content-Type: application/json" \
-d '{"a": 10, "b": 5}'

# Get last result
curl http://localhost:3000/history
```

## What It Does

1. Send numbers to gateway
2. Calculator adds them
3. Result saved to database via gRPC
4. Can retrieve last calculation

## Setup Requirements

- Node.js, Go, .NET 10, PostgreSQL installed
- Create database: `createdb historydb`
- Update connection string in `appsettings.json`
