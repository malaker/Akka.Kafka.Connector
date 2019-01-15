FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Akka.Kafka.Connector.IntegrationTests/Akka.Kafka.Connector.IntegrationTests.csproj Akka.Kafka.Connector.IntegrationTests/
RUN dotnet restore Akka.Kafka.Connector.IntegrationTests/Akka.Kafka.Connector.IntegrationTests.csproj
COPY . .
WORKDIR /src/Akka.Kafka.Connector.IntegrationTests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

