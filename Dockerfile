FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /Drunkenpolls.Zapfenstreich

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /Drunkenpolls.Zapfenstreich
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Drunkenpolls.Zapfenstreich.dll"]