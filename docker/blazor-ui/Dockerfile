# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/BlazorCRUD/BlazorCRUD.csproj", "BlazorCRUD/"]
COPY ["src/BlazorCRUD.Client/BlazorCRUD.Client.csproj", "BlazorCRUD.Client/"]
RUN dotnet restore "BlazorCRUD/BlazorCRUD.csproj"

# Copy source code
COPY src/BlazorCRUD/ BlazorCRUD/
COPY src/BlazorCRUD.Client/ BlazorCRUD.Client/

# Build and publish
WORKDIR "/src/BlazorCRUD"
RUN dotnet publish "BlazorCRUD.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "BlazorCRUD.dll"]
