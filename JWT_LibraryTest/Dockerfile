#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JWT_LibraryTest/JWT_LibraryTest.csproj", "JWT_LibraryTest/"]
RUN dotnet restore "JWT_LibraryTest/JWT_LibraryTest.csproj"
COPY . .
WORKDIR "/src/JWT_LibraryTest"
RUN dotnet build "JWT_LibraryTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JWT_LibraryTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JWT_LibraryTest.dll"]