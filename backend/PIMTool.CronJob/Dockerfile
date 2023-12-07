FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PIMTool.CronJob/PIMTool.CronJob.csproj", "PIMTool.CronJob/"]
RUN dotnet restore "PIMTool.CronJob/PIMTool.CronJob.csproj"
COPY . .
WORKDIR "/src/PIMTool.CronJob"
RUN dotnet build "PIMTool.CronJob.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PIMTool.CronJob.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PIMTool.CronJob.dll"]
