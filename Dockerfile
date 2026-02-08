# escape=`

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /app

# Copy project file and restore dependencies
COPY *.csproj .
COPY packages.config .
RUN nuget restore

# Copy everything else and build
COPY . .
RUN msbuild /p:Configuration=Release /p:OutputPath=C:\out /p:DeployOnBuild=true /p:PublishProfile=FolderProfile

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8
WORKDIR /inetpub/wwwroot

# Copy published application from build stage
COPY --from=build C:\out\_PublishedWebsites\neoris-pt-backend .

# Configure IIS
RUN powershell -Command `
    Import-Module WebAdministration; `
    New-WebAppPool -Name 'NeorisPTBackendPool' -Force; `
    Set-ItemProperty IIS:\AppPools\NeorisPTBackendPool -Name managedRuntimeVersion -Value 'v4.0'; `
    Set-ItemProperty IIS:\AppPools\NeorisPTBackendPool -Name enable32BitAppOnWin64 -Value $false; `
    Remove-Website -Name 'Default Web Site' -ErrorAction SilentlyContinue; `
    New-Website -Name 'NeorisPTBackend' `
        -Port 5000 `
        -PhysicalPath 'C:\inetpub\wwwroot' `
        -ApplicationPool 'NeorisPTBackendPool' `
        -Force

# Expose the port
EXPOSE 5000

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 `
    CMD powershell -Command `
    try { `
        $response = Invoke-WebRequest -Uri http://localhost:5000/api/productos -UseBasicParsing -TimeoutSec 5; `
        if ($response.StatusCode -eq 200) { exit 0 } else { exit 1 } `
    } catch { exit 1 }
