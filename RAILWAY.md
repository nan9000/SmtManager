# Railway Deployment Configuration

This file helps Railway auto-detect and configure the deployment.

## Services

### Database (SQL Server)
- Service: SQL Server 2022
- Port: 1433

### API
- Docker image: Built from Dockerfile.api
- Port: 80 (internal)
- External port: Auto-assigned

### Web
- Docker image: Built from Dockerfile.web
- Port: 80 (internal)
- External port: Auto-assigned

## Environment Variables

Set these in Railway dashboard for each service:

### API Service
```
ConnectionStrings__DefaultConnection=Server=smt-db;Database=SmtDb;User Id=sa;Password=<DB_PASSWORD>;TrustServerCertificate=True;
JwtSettings__Key=<JWT_SECRET_KEY>
JwtSettings__Issuer=SmtManagerAPI
JwtSettings__Audience=SmtManagerClient
ASPNETCORE_URLS=http://+:80
```

### Database Service
```
SA_PASSWORD=<STRONG_PASSWORD>
ACCEPT_EULA=Y
```
