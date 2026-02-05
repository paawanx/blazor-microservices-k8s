# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project files first (for layer caching)
COPY ["src/BlazorCRUD.AuthApi/BlazorCRUD.AuthApi.csproj", "BlazorCRUD.AuthApi/"]
RUN dotnet restore "BlazorCRUD.AuthApi/BlazorCRUD.AuthApi.csproj"

# Copy the rest of the source code
COPY src/BlazorCRUD.AuthApi/ BlazorCRUD.AuthApi/
WORKDIR "/src/BlazorCRUD.AuthApi"
RUN dotnet publish "BlazorCRUD.AuthApi.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "BlazorCRUD.AuthApi.dll"]
