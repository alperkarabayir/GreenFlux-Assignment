FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GreenFluxAssignment.Api/GreenFluxAssignment.Api.csproj", "GreenFluxAssignment.Api/"]
COPY ["GreenFluxAssignment.Core/GreenFluxAssignment.Core.csproj", "GreenFluxAssignment.Core/"]
COPY ["GreenFluxAssignment.Data/GreenFluxAssignment.Data.csproj", "GreenFluxAssignment.Data/"]
COPY ["GreenFluxAssignment.Services/GreenFluxAssignment.Services.csproj", "GreenFluxAssignment.Services/"]
RUN dotnet restore "GreenFluxAssignment.Api/GreenFluxAssignment.Api.csproj"
COPY . .
WORKDIR "/src/GreenFluxAssignment.Api"
RUN dotnet build "GreenFluxAssignment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GreenFluxAssignment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "GreenFluxAssignment.Api.dll"]