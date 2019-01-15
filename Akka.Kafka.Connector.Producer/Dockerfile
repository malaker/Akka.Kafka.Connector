FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Akka.Kafka.Connector.Producer/Akka.Kafka.Connector.Producer.csproj Akka.Kafka.Connector.Producer/
RUN dotnet restore Akka.Kafka.Connector.Producer/Akka.Kafka.Connector.Producer.csproj
COPY . .
WORKDIR /src/Akka.Kafka.Connector.Producer
RUN dotnet build Akka.Kafka.Connector.Producer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Akka.Kafka.Connector.Producer.csproj -c Release -o /app
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Akka.Kafka.Connector.Producer.dll"]
