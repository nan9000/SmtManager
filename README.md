# SMT Manager

## Overview
This repository contains the implementation of the SMT Manager application.

## Technical Stack

### Backend
- **Framework**: .NET Web API
- **Architecture**: Clean Architecture
- **Database**: SQL Server
- **Authentication**: JWT (JSON Web Tokens)
- **Testing**: NUnit, Moq

### Frontend
- **Framework**: Angular
- **Styling**: Bootstrap 5

## Getting Started

### Prerequisites
- Docker & Docker Compose

### Execution
To build and launch the application services, execute the following command:

```bash
docker compose up --build
```

### Access Points
- **Web Client**: http://localhost:4200
- **API Documentation (Swagger)**: http://localhost:5000/swagger

## Testing
To execute the backend test suite:

```bash
dotnet test
```

## Implementation Notes
- The solution enforces separation of concerns through Clean Architecture layers.
- Authentication is secured via JWT infrastructure.
- Default Credentials (for testing): `admin` / `admin123`

---
**Author**: Adnan
