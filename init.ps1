[CmdletBinding(DefaultParameterSetName = "no-arguments")]
Param (
    [Parameter(HelpMessage = "Enables initialization of values in the .env file, which may be placed in source control.",
        ParameterSetName = "env-init")]
    [switch]$InitEnv,

    [Parameter(Mandatory = $true,
        HelpMessage = "The path to a valid Sitecore license.xml file.",
        ParameterSetName = "env-init")]
    [string]$LicenseXmlPath,

    # We do not need to use [SecureString] here since the value will be stored unencrypted in .env,
    # and used only for transient local development environments.
    [Parameter(Mandatory = $true,
        HelpMessage = "Sets the sitecore\\admin password for this environment via environment variable.",
        ParameterSetName = "env-init")]
    [string]$AdminPassword,

    [Parameter(Mandatory = $true,
    HelpMessage = "The Client ID used to authenticate with Auth0.",
        ParameterSetName = "env-init")]
    [string]$Auth0_ClientId,

    [Parameter(Mandatory = $true,
    HelpMessage = "The Client Secret used to authenticate with Auth0.",
        ParameterSetName = "env-init")]
    [string]$Auth0_ClientSecret,

    [Parameter(HelpMessage = "The hostname used for the local CM instance.",
        ParameterSetName = "env-init")]
    [string]$Host_Suffix = "xmcloudcm.localhost",

    [Parameter(HelpMessage = "The hostname used for the local CM instance.",
        ParameterSetName = "env-init")]
    [string]$Edge_Url,

    [Parameter(HelpMessage = "The hostname used for the local CM instance.",
        ParameterSetName = "env-init")]
    [string]$Edge_Token
)

$ErrorActionPreference = "Stop";

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

##################
# Create .env file
##################
Write-Host "Creating Environment File..." -ForegroundColor Green
Copy-Item ".\.env.template" ".\.env" -Force

################################################
# Retrieve and import SitecoreDockerTools module
################################################
# Check for Sitecore Gallery
Import-Module PowerShellGet
$SitecoreGallery = Get-PSRepository | Where-Object { $_.SourceLocation -eq "https://sitecore.myget.org/F/sc-powershell/api/v2" }
if (-not $SitecoreGallery) {
    Write-Host "Adding Sitecore PowerShell Gallery..." -ForegroundColor Green
    Register-PSRepository -Name SitecoreGallery -SourceLocation https://sitecore.myget.org/F/sc-powershell/api/v2 -InstallationPolicy Trusted
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
$CM_Host = $Host_Suffix
$MVP_Host = "mvp.$Host_Suffix"

##################################
# Configure TLS/HTTPS certificates
##################################
Push-Location docker\traefik\certs
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
    Write-Host "Generating Traefik TLS certificate..." -ForegroundColor Green
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
Write-Host "Adding Windows hosts file entries..." -ForegroundColor Green
Add-HostsEntry $CM_Host
Add-HostsEntry $MVP_Host

###############################
# Populate the environment file
###############################
Write-Host "Populating Environment File..." -ForegroundColor Green
if ($InitEnv) {

    Write-Host "Populating required .env file values..." -ForegroundColor Green
    Set-EnvFileVariable "HOST_LICENSE_FOLDER" -Value $LicenseXmlPath
    Set-EnvFileVariable "CM_HOST" -Value $CM_Host
    Set-EnvFileVariable "MVP_RENDERING_HOST" -Value $MVP_Host
    Set-EnvFileVariable "REPORTING_API_KEY" -Value (Get-SitecoreRandomString 128 -DisallowSpecial)
    Set-EnvFileVariable "TELERIK_ENCRYPTION_KEY" -Value (Get-SitecoreRandomString 128)
    Set-EnvFileVariable "MEDIA_REQUEST_PROTECTION_SHARED_SECRET" -Value (Get-SitecoreRandomString 64)
    Set-EnvFileVariable "SQL_SA_PASSWORD" -Value (Get-SitecoreRandomString 19 -DisallowSpecial -EnforceComplexity)
    Set-EnvFileVariable "SITECORE_ADMIN_PASSWORD" -Value $AdminPassword
    Set-EnvFileVariable "SITECORE_FedAuth_dot_Auth0_dot_ClientId" -Value $Auth0_ClientId
    Set-EnvFileVariable "SITECORE_FedAuth_dot_Auth0_dot_ClientSecret" -Value $Auth0_ClientSecret
    Set-EnvFileVariable "SITECORE_FedAuth_dot_Auth0_dot_RedirectBaseUrl" -Value "https://$CM_Host/"
    Set-EnvFileVariable "EXPERIENCE_EDGE_URL" -Value $Edge_Url
    Set-EnvFileVariable "EXPERIENCE_EDGE_TOKEN" -Value $Edge_Token
}

Push-Location docker\traefik\certs
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