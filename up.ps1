[CmdletBinding(DefaultParameterSetName = "no-arguments")]
Param (
    [Parameter(HelpMessage = "Switch to enable running against edge, this will only run Traefik and the host containers.",
        ParameterSetName = "env-init")]
    [switch]$UseEdge
)

$ErrorActionPreference = "Stop";

$envContent = Get-Content .env -Encoding UTF8
$xmCloudHost = $envContent | Where-Object { $_ -imatch "^CM_HOST=.+" }
$mvpHost = $envContent | Where-Object { $_ -imatch "^MVP_RENDERING_HOST=.+" }
$sugconeuHost = $envContent | Where-Object { $_ -imatch "^SUGCON_EU_HOST=.+" }
$sugconanzHost = $envContent | Where-Object { $_ -imatch "^SUGCON_ANZ_HOST=.+" }
$sitecoreDockerRegistry = $envContent | Where-Object { $_ -imatch "^SITECORE_DOCKER_REGISTRY=.+" }
$sitecoreVersion = $envContent | Where-Object { $_ -imatch "^SITECORE_VERSION=.+" }

$xmCloudHost = $xmCloudHost.Split("=")[1]
$mvpHost = $mvpHost.Split("=")[1]
$sugconeuHost = $sugconeuHost.Split("=")[1]
$sugconanzHost = $sugconanzHost.Split("=")[1]
$sitecoreDockerRegistry = $sitecoreDockerRegistry.Split("=")[1]
$sitecoreVersion = $sitecoreVersion.Split("=")[1]

# Double check whether init has been run
$envCheckVariable = "HOST_LICENSE_FOLDER"
$envCheck = $envContent | Where-Object { $_ -imatch "^$envCheckVariable=.+" }
if (-not $envCheck) {
    throw "$envCheckVariable does not have a value. Did you run 'init.ps1 -InitEnv'?"
}

Write-Host "Keeping XM Cloud base image up to date" -ForegroundColor Green
docker pull "$($sitecoreDockerRegistry)sitecore-xmcloud-cm:$($sitecoreVersion)"

# Build all containers in the Sitecore instance, forcing a pull of latest base containers
Write-Host "Building containers..." -ForegroundColor Green
docker-compose build
if ($LASTEXITCODE -ne 0) {
    Write-Error "Container build failed, see errors above."
}

if ($UseEdge) {
    # Start the container using the edge override to only run traefik & host containers
    Write-Host "Starting Sitecore hosts running againt edge..." -ForegroundColor Green
    docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml -f .\docker-compose.edge.yml up -d

    # Wait for Traefik to expose Rendering Host route
    Write-Host "Waiting for MVP Rendering Host to become available..." -ForegroundColor Green
    $startTime = Get-Date
    do {
        Start-Sleep -Milliseconds 100
        try {
            $status = Invoke-RestMethod "http://localhost:8079/api/http/routers/mvp-secure@docker"
        }
        catch {
            if ($_.Exception.Response.StatusCode.value__ -ne "404") {
                throw
            }
        }
    } while ($status.status -ne "enabled" -and $startTime.AddSeconds(60) -gt (Get-Date))
    if (-not $status.status -eq "enabled") {
        $status
        Write-Error "Timeout waiting for MVP Rendering Host to become available via Traefik proxy. Check mvp-rendering container logs."
    }

    Write-Host "Opening site..." -ForegroundColor Green
    Start-Process https://$mvpHost
    Start-Process https://$sugconeuHost
    Start-Process https://$sugconanzHost
}
else {
    # Start the Sitecore instance
    Write-Host "Starting Sitecore environment, all roles running locally..." -ForegroundColor Green
    docker-compose up -d

    # Wait for Traefik to expose CM route
    Write-Host "Waiting for CM to become available..." -ForegroundColor Green
    $startTime = Get-Date
    do {
        Start-Sleep -Milliseconds 100
        try {
            $status = Invoke-RestMethod "http://localhost:8079/api/http/routers/cm-secure@docker" -TimeoutSec 600
        }
        catch {
            if ($_.Exception.Response.StatusCode.value__ -ne "404") {
                throw
            }
        }
    } while ($status.status -ne "enabled" -and $startTime.AddSeconds(600) -gt (Get-Date))
    if (-not $status.status -eq "enabled") {
        $status
        Write-Error "Timeout waiting for Sitecore CM to become available via Traefik proxy. Check CM container logs."
    }

    Write-Host "Restoring Sitecore CLI..." -ForegroundColor Green
    dotnet tool restore
    Write-Host "Installing Sitecore CLI Plugins..."
    dotnet sitecore --help | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Unexpected error installing Sitecore CLI Plugins"
    }

    Write-Host "Logging into Sitecore..." -ForegroundColor Green
    dotnet sitecore cloud login
    dotnet sitecore connect -r xmcloud --cm https://$xmCloudHost --allow-write true --environment-name default

    if ($LASTEXITCODE -ne 0) {
        Write-Error "Unable to log into Sitecore, did the Sitecore environment start correctly? See logs above."
    }

    # Populate Solr managed schemas to avoid errors during item deploy
    Write-Host "Populating Solr managed schema..." -ForegroundColor Green
    dotnet sitecore index schema-populate
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Populating Solr managed schema failed, see errors above."
    }

    # Rebuild indexes
    Write-Host "Rebuilding indexes ..." -ForegroundColor Green
    dotnet sitecore index rebuild

    # Deploy the serialised content items
    Write-Host "Pushing items to Sitecore..." -ForegroundColor Green
    dotnet sitecore ser push
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Serialization push failed, see errors above."
    }

    Write-Host "Opening site..." -ForegroundColor Green
    Start-Process https://$xmCloudHost/sitecore/
    Start-Process https://$mvpHost
    Start-Process https://$sugconeuHost
    Start-Process https://$sugconanzHost
}