# BlazorCRUD

A microservices-based CRUD application built with Blazor, ASP.NET Core, and Kubernetes.

## Project Structure

```
BlazorCRUD/
├── src/                         # Source code
│   ├── BlazorCRUD/             # Main Blazor Web App
│   ├── BlazorCRUD.Client/      # Blazor WebAssembly Client
│   ├── BlazorCRUD.AuthApi/     # Authentication Microservice
│   ├── BlazorCRUD.StudentApi/  # Student CRUD Microservice
│   └── BlazorCRUD.ApiGateway/  # YARP API Gateway
├── k8s/                         # Kubernetes manifests
│   ├── auth-api/
│   ├── student-api/
│   └── gateway/
├── docker/                      # Dockerfiles
│   ├── auth-api/
│   │   └── Dockerfile
│   ├── student-api/
│   │   └── Dockerfile
│   ├── gateway/
│   │   └── Dockerfile
│   └── blazorcrud/
│       └── Dockerfile
├── BlazorCRUD.sln              # Solution file
├── azure-pipelines.yml         # Azure DevOps CI/CD
└── README.md
```

## Technologies

- **.NET 8** - Framework
- **Blazor** - Web UI with SSR, Server, and WebAssembly modes
- **Entity Framework Core** - ORM with SQLite
- **YARP** - Reverse proxy for API Gateway
- **JWT** - Authentication
- **Docker** - Containerization
- **Kubernetes** - Orchestration

## Getting Started

### Local Development

1. Run Auth API:
   ```bash
   cd src/BlazorCRUD.AuthApi
   dotnet run
   ```

2. Run Student API:
   ```bash
   cd src/BlazorCRUD.StudentApi
   dotnet run
   ```

3. Run API Gateway:
   ```bash
   cd src/BlazorCRUD.ApiGateway
   dotnet run
   ```

4. Run Blazor App:
   ```bash
   cd src/BlazorCRUD
   dotnet run
   ```

### Kubernetes Deployment

See [k8s/README.md](k8s/README.md) for detailed instructions.

## API Endpoints

### Auth API (Port 5258 / K8s NodePort 30001)
- POST `/api/auth/register` - Register user
- POST `/api/auth/login` - Login user
- GET `/api/secure` - Protected endpoint

### Student API (Port 5248)
- GET `/api/students` - Get all students

### API Gateway (Port 5113)
- `/auth/*` - Routes to Auth API
- `/students/*` - Routes to Student API (requires JWT)
