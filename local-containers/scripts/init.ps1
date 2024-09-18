#Requires -RunAsAdministrator
[CmdletBinding(DefaultParameterSetName = "no-arguments")]
Param (
    [Parameter(HelpMessage = "Enables initialization of values in the .env file, which may be placed in source control.",
        ParameterSetName = "env-init")]
    [switch]$InitEnv,
    [Parameter(Mandatory = $true,
        HelpMessage = "The path to a valid Sitecore license.xml file.",
        ParameterSetName = "env-init")]
    [string]$LicenseXmlPath,
    [Parameter(Mandatory = $true,
        HelpMessage = "Sets the sitecore\\admin password for this environment via environment variable.",
        ParameterSetName = "env-init")]
    [String]$AdminPassword,
    [Parameter(HelpMessage = "The Edge token used when running in Edge Mode.")]
    [string]$Edge_Token,
    [Parameter(HelpMessage = "The Okta domain used by the MVP Rendering Host.")]
    [string]$OKTA_Domain,
    [Parameter(HelpMessage = "The Okta Client Id used by the MVP Rendering Host.")]
    [string]$OKTA_Client_Id,
    [Parameter(HelpMessage = "The Okta Client Secret used by the MVP Rendering Host.")]
    [string]$OKTA_Client_Secret,
    [Parameter(HelpMessage = "The URL for the MVP Selections API.")]
    [string]$MVP_Selections_API,
    [Parameter(HelpMessage = "SUGCON ANZ CPD Client Key.")]
    [string]$SUCGON_ANZ_CDP_CLIENT_KEY,
    [Parameter(HelpMessage = "SUGCON ANZ CPD Target URL.")]
    [string]$SUCGON_ANZ_CDP_TARGET_URL,
    [Parameter(HelpMessage = "SUGCON ANZ CPD Point of Sale.")]
    [string]$SUCGON_ANZ_CDP_POINTOFSALE,
    [Parameter(HelpMessage = "SUGCON EU CPD Client Key.")]
    [string]$SUCGON_EU_CDP_CLIENT_KEY,
    [Parameter(HelpMessage = "SUGCON EU CPD Target URL.")]
    [string]$SUCGON_EU_CDP_TARGET_URL,
    [Parameter(HelpMessage = "SUGCON EU CPD Point of Sale.")]
    [string]$SUCGON_EU_CDP_POINTOFSALE,
    [Parameter(HelpMessage = "SUGCON INDIA CPD Client Key.")]
    [string]$SUCGON_INDIA_CDP_CLIENT_KEY,
    [Parameter(HelpMessage = "SUGCON INDIA CPD Target URL.")]
    [string]$SUCGON_INDIA_CDP_TARGET_URL,
    [Parameter(HelpMessage = "SUGCON INDIA CPD Point of Sale.")]
    [string]$SUCGON_INDIA_CDP_POINTOFSALE,
    [Parameter(HelpMessage = "SUGCON NA CPD Client Key.")]
    [string]$SUCGON_NA_CDP_CLIENT_KEY,
    [Parameter(HelpMessage = "SUGCON NA CPD Target URL.")]
    [string]$SUCGON_NA_CDP_TARGET_URL,
    [Parameter(HelpMessage = "SUGCON NA CPD Point of Sale.")]
    [string]$SUCGON_NA_CDP_POINTOFSALE,
    [Parameter(Mandatory = $false, HelpMessage = "Specifies os version of the base image.")]
    [ValidateSet("ltsc2019", "ltsc2022")]
    [string]$baseOs = "ltsc2022"
)

$ErrorActionPreference = "Stop";

# Set the root of the repository
$RepoRoot = Resolve-Path "$PSScriptRoot\..\.."

if ($InitEnv) {
    if (-not $LicenseXmlPath.EndsWith("license.xml")) {
        Write-Error "Sitecore license file must be named 'license.xml'."
    }
    if (-not (Test-Path $LicenseXmlPath)) {
        Write-Error "Could not find Sitecore license file at path '$LicenseXmlPath'."
    }
    # We actually want the folder that it's in for mounting
    $LicenseXmlPath = (Get-Item $LicenseXmlPath).Directory.FullName
}

Write-Host "Preparing your Sitecore Containers environment!" -ForegroundColor Green

################################################
# Retrieve and import SitecoreDockerTools module
################################################

# Check for Sitecore Gallery
Import-Module PowerShellGet
$SitecoreGallery = Get-PSRepository | Where-Object { $_.SourceLocation -eq "https://nuget.sitecore.com/resources/v2" }
if (-not $SitecoreGallery) {
    Write-Host "Adding Sitecore PowerShell Gallery..." -ForegroundColor Green
    Unregister-PSRepository -Name SitecoreGallery -ErrorAction SilentlyContinue
    Register-PSRepository -Name SitecoreGallery -SourceLocation https://nuget.sitecore.com/resources/v2 -InstallationPolicy Trusted
    $SitecoreGallery = Get-PSRepository -Name SitecoreGallery
}

# Install and Import SitecoreDockerTools
$dockerToolsVersion = "10.2.7"
Remove-Module SitecoreDockerTools -ErrorAction SilentlyContinue
if (-not (Get-InstalledModule -Name SitecoreDockerTools -RequiredVersion $dockerToolsVersion -ErrorAction SilentlyContinue)) {
    Write-Host "Installing SitecoreDockerTools..." -ForegroundColor Green
    Install-Module SitecoreDockerTools -RequiredVersion $dockerToolsVersion -Scope CurrentUser -Repository $SitecoreGallery.Name
}
Write-Host "Importing SitecoreDockerTools..." -ForegroundColor Green
Import-Module SitecoreDockerTools -RequiredVersion $dockerToolsVersion
Write-SitecoreDockerWelcome

###########################
# Setup site host variables
###########################
Write-Host "Setting HOSTS values." -ForegroundColor Green
$Host_Suffix = "xmcloudcm.localhost"
$CM_Host = $Host_Suffix
$MVP_Host = "mvp.$Host_Suffix"
$SUGCON_EU_HOST = "sugconeu.$Host_Suffix"
$SUGCON_ANZ_HOST = "sugconanz.$Host_Suffix"
$SUGCON_INDIA_HOST = "sugconindia.$Host_Suffix"
$SUGCON_NA_HOST = "sugconna.$Host_Suffix"

##################################
# Configure TLS/HTTPS certificates
##################################
Push-Location $RepoRoot\local-containers\docker\traefik\certs
try {
    $mkcert = ".\mkcert.exe"
    if ($null -ne (Get-Command mkcert.exe -ErrorAction SilentlyContinue)) {
        # mkcert installed in PATH
        $mkcert = "mkcert"
    } elseif (-not (Test-Path $mkcert)) {
        Write-Host "Downloading and installing mkcert certificate tool..." -ForegroundColor Green
        Invoke-WebRequest "https://github.com/FiloSottile/mkcert/releases/download/v1.4.1/mkcert-v1.4.1-windows-amd64.exe" -UseBasicParsing -OutFile mkcert.exe
        if ((Get-FileHash mkcert.exe).Hash -ne "1BE92F598145F61CA67DD9F5C687DFEC17953548D013715FF54067B34D7C3246") {
            Remove-Item mkcert.exe -Force
            throw "Invalid mkcert.exe file"
        }
    }
    Write-Host "Generating Traefik TLS certificate." -ForegroundColor Green
    & $mkcert -install
    & $mkcert "*.$Host_Suffix"
    & $mkcert $Host_Suffix

    # stash CAROOT path for messaging at the end of the script
    $caRoot = "$(& $mkcert -CAROOT)\rootCA.pem"
}
catch {
    Write-Error "An error occurred while attempting to generate TLS certificate: $_"
}
finally {
    Pop-Location
}

################################
# Add Windows hosts file entries
################################
Write-Host "Adding Windows hosts file entries." -ForegroundColor Green
Add-HostsEntry $CM_Host
Add-HostsEntry $MVP_Host
Add-HostsEntry $SUGCON_EU_HOST
Add-HostsEntry $SUGCON_ANZ_HOST
Add-HostsEntry $SUGCON_INDIA_HOST
Add-HostsEntry $SUGCON_NA_HOST

##################
# Create .env file
##################
Write-Host "Creating .env file." -ForegroundColor Green
Copy-Item "$RepoRoot\local-containers\.env.template" "$RepoRoot\local-containers\.env" -Force
$envFileLocation = "$RepoRoot/local-containers/.env"

################################
# Add Windows hosts file entries
################################
Write-Host "Adding Windows hosts file entries..." -ForegroundColor Green
Add-HostsEntry "xmcloudcm.localhost"
Add-HostsEntry "www.nextjs-starter.localhost"

################################
# Generate JSS_EDITING_SECRET
################################
$jssEditingSecret = Get-SitecoreRandomString 64 -DisallowSpecial
Set-EnvFileVariable "JSS_EDITING_SECRET" -Value $jssEditingSecret -Path $envFileLocation

###############################
# Populate the environment file
###############################
if ($InitEnv) {
	Write-Host "Populating .env file." -ForegroundColor Green
    Set-EnvFileVariable "HOST_LICENSE_FOLDER" -Value $LicenseXmlPath -Path $envFileLocation
	Set-EnvFileVariable "REPORTING_API_KEY" -Value (Get-SitecoreRandomString 128 -DisallowSpecial) -Path $envFileLocation
	Set-EnvFileVariable "TELERIK_ENCRYPTION_KEY" -Value (Get-SitecoreRandomString 128 -DisallowSpecial) -Path $envFileLocation
	Set-EnvFileVariable "MEDIA_REQUEST_PROTECTION_SHARED_SECRET" -Value (Get-SitecoreRandomString 64 -DisallowSpecial) -Path $envFileLocation
	Set-EnvFileVariable "SQL_SA_PASSWORD" -Value (Get-SitecoreRandomString 19 -DisallowSpecial -EnforceComplexity) -Path $envFileLocation
    Set-EnvFileVariable "SQL_SERVER" -Value "mssql" -Path $envFileLocation
    Set-EnvFileVariable "SQL_SA_LOGIN" -Value "sa" -Path $envFileLocation
	Set-EnvFileVariable "SITECORE_ADMIN_PASSWORD" -Value $AdminPassword -Path $envFileLocation
	Set-EnvFileVariable "EXPERIENCE_EDGE_TOKEN" -Value $Edge_Token -Path $envFileLocation
	Set-EnvFileVariable "JSS_EDITING_SECRET" -Value (Get-SitecoreRandomString 64 -DisallowSpecial) -Path $envFileLocation
	Set-EnvFileVariable "OKTA_DOMAIN" -Value $OKTA_Domain -Path $envFileLocation
	Set-EnvFileVariable "OKTA_CLIENT_ID" -Value $OKTA_Client_Id -Path $envFileLocation
	Set-EnvFileVariable "OKTA_CLIENT_SECRET" -Value $OKTA_Client_Secret -Path $envFileLocation
    Set-EnvFileVariable "MVP_SELECTIONS_API" -Value $MVP_Selections_API -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_ANZ_CDP_CLIENT_KEY" -Value $SUCGON_ANZ_CDP_CLIENT_KEY -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_ANZ_CDP_TARGET_URL" -Value $SUCGON_ANZ_CDP_TARGET_URL -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_ANZ_CDP_POINTOFSALE" -Value $SUCGON_ANZ_CDP_POINTOFSALE -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_EU_CDP_CLIENT_KEY" -Value $SUCGON_EU_CDP_CLIENT_KEY -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_EU_CDP_TARGET_URL" -Value $SUCGON_EU_CDP_TARGET_URL -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_EU_CDP_POINTOFSALE" -Value $SUCGON_EU_CDP_POINTOFSALE -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_INDIA_CDP_CLIENT_KEY" -Value $SUCGON_INDIA_CDP_CLIENT_KEY -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_INDIA_CDP_TARGET_URL" -Value $SUCGON_INDIA_CDP_TARGET_URL -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_INDIA_CDP_POINTOFSALE" -Value $SUCGON_INDIA_CDP_POINTOFSALE -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_NA_CDP_CLIENT_KEY" -Value $SUCGON_NA_CDP_CLIENT_KEY -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_NA_CDP_TARGET_URL" -Value $SUCGON_NA_CDP_TARGET_URL -Path $envFileLocation
    Set-EnvFileVariable "SUCGON_NA_CDP_POINTOFSALE" -Value $SUCGON_NA_CDP_POINTOFSALE -Path $envFileLocation
}
Write-Host "Finished populating .env file." -ForegroundColor Green

##########################
# Show Certificate Details
##########################
Pop-Location
Push-Location $RepoRoot\local-containers\docker\traefik\certs
try
{
    Write-Host
    Write-Host ("#"*75) -ForegroundColor Cyan
    Write-Host "To avoid HTTPS errors, set the NODE_EXTRA_CA_CERTS environment variable" -ForegroundColor Cyan
    Write-Host "using the following commmand:" -ForegroundColor Cyan
    Write-Host "setx NODE_EXTRA_CA_CERTS $caRoot"
    Write-Host
    Write-Host "You will need to restart your terminal or VS Code for it to take effect." -ForegroundColor Cyan
    Write-Host ("#"*75) -ForegroundColor Cyan
}
catch {
    Write-Error "An error occurred while attempting to generate TLS certificate: $_"
}
finally {
    Pop-Location
}