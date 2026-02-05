# Kubernetes Deployment

## Prerequisites
- Docker Desktop with Kubernetes enabled
- kubectl installed

## Building Docker Images

From the project root, build the images:

```bash
# Auth API
docker build -t auth-api:local -f docker/auth-api/Dockerfile .

# Student API (when ready)
# docker build -t student-api:local -f docker/student-api/Dockerfile .

# Gateway (when ready)
# docker build -t gateway:local -f docker/gateway/Dockerfile .

# Blazor App (when ready)
# docker build -t blazorcrud:local -f docker/blazorcrud/Dockerfile .
```

## Deploying to Kubernetes

### Auth API
```bash
kubectl apply -f k8s/auth-api/auth-deployment.yaml
kubectl apply -f k8s/auth-api/auth-service.yaml
```

### Check Status
```bash
kubectl get pods
kubectl get svc
```

### Access Services
- Auth API: http://localhost:30001
- Auth API Swagger: http://localhost:30001/swagger

### View Logs
```bash
kubectl logs -f deployment/auth-api
```

### Delete Resources
```bash
kubectl delete -f k8s/auth-api/
```

## Folder Structure
- `auth-api/` - Auth API K8s manifests
- `student-api/` - Student API K8s manifests
- `gateway/` - API Gateway K8s manifests
