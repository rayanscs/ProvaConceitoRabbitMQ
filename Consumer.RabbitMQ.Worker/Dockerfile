#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Consumer.RabbitMQ.Worker/Consumer.RabbitMQ.Worker.csproj", "Consumer.RabbitMQ.Worker/"]
RUN dotnet restore "Consumer.RabbitMQ.Worker/Consumer.RabbitMQ.Worker.csproj"
COPY . .
WORKDIR "/src/Consumer.RabbitMQ.Worker"
RUN dotnet build "Consumer.RabbitMQ.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consumer.RabbitMQ.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consumer.RabbitMQ.Worker.dll"]