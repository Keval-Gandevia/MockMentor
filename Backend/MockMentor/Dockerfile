FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY MockMentorRESTAPI/MockMentorRESTAPI.csproj /app/MockMentorRESTAPI/
COPY MockMentorConsumer/MockMentorConsumer.csproj /app/MockMentorConsumer/

RUN dotnet restore MockMentorConsumer/MockMentorConsumer.csproj

COPY . .

RUN dotnet build MockMentorConsumer/MockMentorConsumer.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish MockMentorConsumer/MockMentorConsumer.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish ./

EXPOSE 80

ENTRYPOINT ["dotnet", "MockMentorConsumer.dll"]