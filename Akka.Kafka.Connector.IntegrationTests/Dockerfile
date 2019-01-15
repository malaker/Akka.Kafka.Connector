FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Akka.Kafka.Connector.IntegrationTests/Akka.Kafka.Connector.IntegrationTests.csproj Akka.Kafka.Connector.IntegrationTests/
COPY Akka.Kafka.Connector.Producer/Akka.Kafka.Connector.Producer.csproj Akka.Kafka.Connector.Producer/
RUN dotnet restore Akka.Kafka.Connector.IntegrationTests/Akka.Kafka.Connector.IntegrationTests.csproj
RUN dotnet restore Akka.Kafka.Connector.Producer/Akka.Kafka.Connector.Producer.csproj
COPY . .
WORKDIR /src/Akka.Kafka.Connector.IntegrationTests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

