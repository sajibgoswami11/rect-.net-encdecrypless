FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["BizWebAPI.csproj", "./"]
RUN dotnet restore "BizWebAPI.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "BizWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BizWebAPI.csproj" -c Release -o /app/publish
COPY app.db /app/publish/

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BizWebAPI.dll"]

# # Set the ASPNETCORE_URLS environment variable to include both HTTP and HTTPS URLs
# ENV ASPNETCORE_URLS=http://+:80;https://+:443