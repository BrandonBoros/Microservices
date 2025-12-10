FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only MicroClinic CSPROJ (correct relative path from Dockerfile location)
COPY ../../Microservices/MicroClinic/*.csproj MicroClinic/

# If Clinic project is referenced:
COPY ../../Microservices/Clinic/*.csproj Clinic/

# If Microservices project is referenced:
COPY ../../Microservices/Microservices/*.csproj Microservices/

RUN dotnet restore "MicroClinic/MicroClinic.csproj"

# Copy all source code
COPY ../../Microservices ./ 

WORKDIR "/src/Microservices/MicroClinic"
RUN dotnet build "MicroClinic.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MicroClinic.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroClinic.dll"]
