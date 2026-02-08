# BlazorCRUD - Microservices Learning Project

A production-ready microservices application demonstrating modern .NET development practices, featuring Blazor UI, JWT authentication, API Gateway pattern, and Kubernetes orchestration.

## 🎯 Project Overview

This project showcases a complete microservices architecture built from scratch as a learning journey, evolving from a simple CRUD application to a full-featured, cloud-native solution deployed on Kubernetes.

### Key Learning Outcomes

- **Microservices Architecture**: Decomposed monolith into independent services (Auth, Student API, Gateway, UI)
- **API Gateway Pattern**: Implemented YARP reverse proxy with hot-reload configuration using Kubernetes ConfigMaps
- **Authentication & Authorization**: JWT-based authentication with custom `AuthenticationStateProvider` for Blazor
- **Container Orchestration**: Kubernetes deployment with Ingress, Services, ConfigMaps, and Secrets
- **Modern Blazor**: Server-side rendering with InteractiveServer mode, avoiding common prerendering pitfalls
- **DevOps**: Docker multi-stage builds, GitHub Container Registry, Kubernetes deployments

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     Nginx Ingress                           │
│              http://localhost (Port 80)                     │
└─────────────────┬───────────────────────┬───────────────────┘
                  │                       │
        / (UI)    │                       │ /api/* (APIs)
                  ▼                       ▼
         ┌─────────────┐         ┌──────────────┐
         │  Blazor UI  │────────>│ API Gateway  │
         │   (Server)  │  HTTP   │    (YARP)    │
         └─────────────┘         └──────┬───────┘
              Pod                       │
                               ┌────────┴────────┐
                               ▼                 ▼
                        ┌──────────┐      ┌─────────────┐
                        │ Auth API │      │ Student API │
                        │  (JWT)   │      │   (CRUD)    │
                        └──────────┘      └─────────────┘
                           Pod                  Pod
                            │                    │
                            ▼                    ▼
                        SQLite DB            SQLite DB
```

**Network Flow:**
- External → Ingress → UI Pod (Blazor Server)
- Blazor Server → Gateway Pod → Auth/Student API Pods (internal K8s DNS)
- Browser (WebAssembly) → Ingress → Gateway → APIs (optional client-side calls)

## 📁 Project Structure

```
BlazorCRUD/
├── src/
│   ├── BlazorCRUD/              # Blazor Server Web App
│   │   ├── Components/          # Razor components, pages, layouts
│   │   ├── Services/            # AuthApiService, StudentApiService
│   │   │   ├── CustomAuthStateProvider.cs
│   │   │   ├── AuthTokenStore.cs
│   │   │   └── ...
│   │   └── Models/              # Student, LoginResponse DTOs
│   ├── BlazorCRUD.Client/       # Blazor WebAssembly Client
│   ├── BlazorCRUD.AuthApi/      # Authentication Microservice
│   │   ├── Controllers/         # AuthController
│   │   ├── Services/            # JwtService
│   │   └── Data/                # ApplicationDbContext
│   ├── BlazorCRUD.StudentApi/   # Student CRUD Microservice
│   │   ├── Controllers/         # StudentsController
│   │   └── Data/                # StudentDbContext
│   └── BlazorCRUD.ApiGateway/   # YARP Reverse Proxy
│       └── Program.cs           # ConfigMap-based hot-reload config
├── k8s/                         # Kubernetes Manifests
│   ├── blazor-ui/
│   │   ├── blazor-ui-deployment.yaml
│   │   └── blazor-ui-service.yaml (ClusterIP)
│   ├── gateway/
│   │   ├── gateway-deployment.yaml
│   │   ├── gateway-service.yaml (ClusterIP)
│   │   └── yarp-config-configmap.yaml
│   ├── auth-api/
│   │   ├── auth-deployment.yaml
│   │   └── auth-service.yaml (NodePort 30001)
│   ├── student-api/
│   │   ├── student-deployment.yaml
│   │   └── student-service.yaml (NodePort 30002)
│   └── ingress.yaml             # Nginx Ingress Controller
├── docker/                      # Dockerfiles
│   ├── blazor-ui/Dockerfile     # Multi-stage .NET 8 build
│   ├── auth-api/Dockerfile
│   ├── student-api/Dockerfile
│   └── gateway/Dockerfile
└── README.md
```

## 🚀 Technologies & Patterns

### Core Stack
- **.NET 8** - Latest LTS framework
- **Blazor Server + WebAssembly** - Hybrid rendering with InteractiveServer mode
- **Entity Framework Core 8.0.11** - Code-first with SQLite
- **YARP (Yet Another Reverse Proxy)** - API Gateway with hot-reload configuration
- **JWT Bearer Authentication** - Stateless authentication with custom AuthenticationStateProvider

### Infrastructure
- **Docker** - Multi-stage builds for optimized images
- **Kubernetes** - Docker Desktop cluster with multiple deployments
- **Nginx Ingress Controller** - External traffic routing
- **ConfigMaps** - Externalized configuration with hot-reload
- **GitHub Container Registry (GHCR)** - Image hosting at `ghcr.io/paawanx`

### Design Patterns
- **Microservices Architecture** - Independently deployable services
- **API Gateway Pattern** - Single entry point for client requests
- **Repository Pattern** - Data access abstraction (DbContext)
- **Service Layer Pattern** - HttpClient-based API communication
- **Options Pattern** - IOptions<T> for configuration management

## 🎓 Key Technical Implementations

### 1. Blazor Authentication with JWT
- **Custom `AuthenticationStateProvider`** - Manages JWT tokens in `ProtectedLocalStorage`
- **`OnAfterRenderAsync` Pattern** - Avoids prerendering issues with JavaScript interop
- **`[Authorize]` Attribute** - Protects routes with automatic redirect to login
- **`AuthorizeView` Components** - Conditional UI rendering based on auth state

### 2. API Gateway Configuration
- **ConfigMap-based YARP Config** - Mounted at `/app/yarp/yarp-config.json`
- **Hot-reload Support** - `AddJsonFile(reloadOnChange: true)` for live updates
- **Route Matching** - Path-based routing (`/api/auth/*`, `/api/students/*`)

### 3. Kubernetes Networking
- **Internal Service Communication** - Pods use K8s DNS (`http://blazorcrud-gateway`)
- **NodePort vs ClusterIP** - NodePort for dev access, ClusterIP for internal routing
- **Ingress Routing** - Single external endpoint with path-based routing

### 4. Docker Optimization
- **Multi-stage Builds** - Separate build and runtime images
- **Layer Caching** - Optimized COPY order for faster rebuilds
- **Versioned Tags** - Semantic versioning for image tracking

## 📚 Getting Started

### Prerequisites
- .NET 8 SDK
- Docker Desktop with Kubernetes enabled
- kubectl CLI
- Nginx Ingress Controller installed

### Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/paawanx/BlazorCRUD.git
   cd BlazorCRUD
   ```

2. **Run services locally** (Optional - for development)
   ```bash
   # Terminal 1 - Auth API
   cd src/BlazorCRUD.AuthApi
   dotnet run

   # Terminal 2 - Student API
   cd src/BlazorCRUD.StudentApi
   dotnet run

   # Terminal 3 - API Gateway
   cd src/BlazorCRUD.ApiGateway
   dotnet run

   # Terminal 4 - Blazor UI
   cd src/BlazorCRUD
   dotnet run
   ```

### Kubernetes Deployment

1. **Install Nginx Ingress Controller**
   ```bash
   kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.1/deploy/static/provider/cloud/deploy.yaml
   ```

2. **Create GitHub Container Registry Secret** (if using private images)
   ```bash
   kubectl create secret docker-registry ghcr-secret \
     --docker-server=ghcr.io \
     --docker-username=YOUR_GITHUB_USERNAME \
     --docker-password=YOUR_GITHUB_PAT
   ```

3. **Deploy all services**
   ```bash
   # Deploy Auth API
   kubectl apply -f k8s/auth-api/

   # Deploy Student API
   kubectl apply -f k8s/student-api/

   # Deploy API Gateway with ConfigMap
   kubectl apply -f k8s/gateway/

   # Deploy Blazor UI
   kubectl apply -f k8s/blazor-ui/

   # Deploy Ingress
   kubectl apply -f k8s/ingress.yaml
   ```

4. **Verify deployment**
   ```bash
   kubectl get pods
   kubectl get svc
   kubectl get ingress
   ```

5. **Access the application**
   - UI: http://localhost
   - API via Gateway: http://localhost/api/*
   - Auth API direct: http://localhost:30001/swagger
   - Student API direct: http://localhost:30002/swagger

## 🔧 Configuration

### Environment Variables

**Blazor UI:**
- `GATEWAY_BASE_URL` - API Gateway endpoint (default: `http://blazorcrud-gateway`)
- `ASPNETCORE_ENVIRONMENT` - Environment (Development/Production)

**Auth API:**
- `JwtSettings__Secret` - JWT signing key
- `JwtSettings__Issuer` - Token issuer
- `JwtSettings__Audience` - Token audience
- `JwtSettings__ExpirationMinutes` - Token lifetime

### ConfigMap Hot-reload

Edit YARP configuration without redeploying:
```bash
kubectl edit configmap yarp-config
# Changes propagate within ~60 seconds
```

## 📖 API Documentation

### Authentication Endpoints

**POST** `/api/auth/register`
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**POST** `/api/auth/login`
```json
{
  "username": "user@example.com",
  "password": "SecurePassword123!"
}
```
**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

### Student CRUD Endpoints (Requires JWT)

**GET** `/api/students` - Get all students  
**GET** `/api/students/{id}` - Get student by ID  
**POST** `/api/students` - Create student  
**PUT** `/api/students/{id}` - Update student  
**DELETE** `/api/students/{id}` - Delete student  

## 🐛 Common Issues & Solutions

### Issue: Login fails with "Connection refused"
**Solution:** Ensure `GATEWAY_BASE_URL` uses internal K8s service name (`http://blazorcrud-gateway`), not localhost or NodePort.

### Issue: Ingress returns 404
**Solution:** Check ingress routes with `kubectl describe ingress blazorcrud-ingress` and verify path prefixes match YARP configuration.

### Issue: ProtectedLocalStorage error on login
**Solution:** Use `OnAfterRenderAsync` instead of `OnInitializedAsync` to avoid prerendering issues with JavaScript interop.

### Issue: ConfigMap changes not reflected
**Solution:** Wait ~60 seconds for kubelet to sync, or check mount: `kubectl exec <gateway-pod> -- cat /app/yarp/yarp-config.json`

## 🧪 Testing

### Test API Gateway routing
```bash
# Test auth endpoint
curl -X POST http://localhost/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"test123"}'

# Test students endpoint (with JWT)
curl http://localhost/api/students \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Check service health
```bash
# View logs
kubectl logs -l app=blazorcrud-gateway --tail=50
kubectl logs -l app=blazorcrud-ui --tail=50

# Check pod status
kubectl get pods -o wide

# Verify environment variables
kubectl exec <pod-name> -- printenv GATEWAY_BASE_URL
```

## 🎯 Learning Milestones

1. ✅ Built basic Blazor CRUD with QuickGrid and EF Core
2. ✅ Integrated WebAssembly client for interactive components
3. ✅ Migrated to microservices architecture (Auth API, Student API, Gateway)
4. ✅ Implemented JWT authentication with custom `AuthenticationStateProvider`
5. ✅ Containerized all services with Docker multi-stage builds
6. ✅ Deployed to Kubernetes with proper service mesh
7. ✅ Configured YARP API Gateway with ConfigMap hot-reload
8. ✅ Set up Nginx Ingress for external access
9. ✅ Mastered Kubernetes networking (ClusterIP, NodePort, Ingress)
10. ✅ Published images to GitHub Container Registry

## 🔮 Future Enhancements

- [ ] Add health checks and readiness probes
- [ ] Implement distributed tracing (OpenTelemetry)
- [ ] Add Redis for distributed caching
- [ ] Implement CQRS pattern with MediatR
- [ ] Add integration tests with Testcontainers
- [ ] Set up CI/CD with GitHub Actions
- [ ] Implement rate limiting in API Gateway
- [ ] Add Helm charts for easier deployment
- [ ] Migrate to PostgreSQL for production
- [ ] Implement Serilog structured logging

## 📝 License

This project is a learning exercise and is provided as-is for educational purposes.

## 🙏 Acknowledgments

Built as a hands-on learning project to master:
- Microservices architecture patterns
- Blazor Server and WebAssembly
- Kubernetes orchestration
- API Gateway with YARP
- JWT authentication
- Container deployment strategies

---

**Author:** Paawan Srivastava  
**Repository:** https://github.com/paawanx/BlazorCRUD  
**Container Registry:** ghcr.io/paawanx
