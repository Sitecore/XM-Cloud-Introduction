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

    # We do not need to use [SecureString] here since the value will be stored unencrypted in .env,
    # and used only for transient local development environments.
    [Parameter(Mandatory = $true,
        HelpMessage = "Sets the sitecore\\admin password for this environment via environment variable.",
        ParameterSetName = "env-init")]
    [String]$AdminPassword,

    [Parameter(HelpMessage = "The Edge token used when running in Edge Mode.",
        ParameterSetName = "env-init")]
    [string]$Edge_Token,

    [Parameter(HelpMessage = "The Okta domain used by the MVP Rendering Host.",
        ParameterSetName = "env-init")]
    [string]$OKTA_Domain,

    [Parameter(HelpMessage = "The Okta Client Id used by the MVP Rendering Host.",
        ParameterSetName = "env-init")]
    [string]$OKTA_Client_Id,

    [Parameter(HelpMessage = "The Okta Client Secret used by the MVP Rendering Host.",
        ParameterSetName = "env-init")]
    [string]$OKTA_Client_Secret
)

$ErrorActionPreference = "Stop";

###############
# License Check
###############
Write-Host "Checking for existence of License." -ForegroundColor Green
if (-not $LicenseXmlPath.EndsWith("license.xml")) {
    Write-Error "Sitecore license file must be named 'license.xml'."
}
if (-not (Test-Path $LicenseXmlPath)) {
    Write-Error "Could not find Sitecore license file at path '$LicenseXmlPath'."
}
# We actually want the folder that it's in for mounting
$LicenseXmlPath = (Get-Item $LicenseXmlPath).Directory.FullName

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
	Unregister-PSRepository -Name SitecoreGallery -ErrorAction SilentlyContinue
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
Write-Host "Setting Hosts Values..." -ForegroundColor Green
$Host_Suffix = "xmcloudcm.localhost"
$CM_Host = $Host_Suffix
$MVP_Host = "mvp.$Host_Suffix"
$SUGCON_EU_HOST = "sugconeu.$Host_Suffix"
$SUGCON_ANZ_HOST = "sugconanz.$Host_Suffix"

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
Add-HostsEntry $SUGCON_EU_HOST
Add-HostsEntry $SUGCON_ANZ_HOST

###############################
# Generate scjssconfig
###############################
$xmCloudBuild = Get-Content "xmcloud.build.json" | ConvertFrom-Json
$scjssconfig = @{
    sitecore= @{
        deploySecret = $xmCloudBuild.renderingHosts.'sugcon-anz'.jssDeploymentSecret
        deployUrl = "https://xmcloudcm.localhost/sitecore/api/jss/import"
      }
    }

ConvertTo-Json -InputObject $scjssconfig | Out-File -FilePath "src\project\Sugcon\SugconAnzSxa\scjssconfig.json"
ConvertTo-Json -InputObject $scjssconfig | Out-File -FilePath "src\project\Sugcon\SugconEuSxa\scjssconfig.json"
Set-EnvFileVariable "JSS_DEPLOYMENT_SECRET_xmcloudpreview" -Value $xmCloudBuild.renderingHosts.sugconanz.jssDeploymentSecret

################################
# Generate Sitecore Api Key
################################
$sitecoreApiKey = (New-Guid).Guid
Set-EnvFileVariable "SITECORE_API_KEY_xmcloudpreview" -Value $sitecoreApiKey

################################
# Generate JSS_EDITING_SECRET
################################
$jssEditingSecret = Get-SitecoreRandomString 64 -DisallowSpecial
Set-EnvFileVariable "JSS_EDITING_SECRET" -Value $jssEditingSecret

###############################
# Populate the environment file
###############################
if ($InitEnv) {
	Write-Host "Populating Environment File..." -ForegroundColor Green
	
	# HOST_LICENSE_FOLDER
	Set-EnvFileVariable "HOST_LICENSE_FOLDER" -Value $LicenseXmlPath
	
	# CM_HOST
	Set-EnvFileVariable "CM_HOST" -Value $CM_Host
	
	# MVP_RENDERING_HOST
	Set-EnvFileVariable "MVP_RENDERING_HOST" -Value $MVP_Host
	
	# REPORTING_API_KEY
	Set-EnvFileVariable "REPORTING_API_KEY" -Value (Get-SitecoreRandomString 128 -DisallowSpecial)
	
	# TELERIK_ENCRYPTION_KEY
	Set-EnvFileVariable "TELERIK_ENCRYPTION_KEY" -Value (Get-SitecoreRandomString 128)
	
	# MEDIA_REQUEST_PROTECTION_SHARED_SECRET
	Set-EnvFileVariable "MEDIA_REQUEST_PROTECTION_SHARED_SECRET" -Value (Get-SitecoreRandomString 64)
	
	# SQL_SA_PASSWORD
	# Need to ensure it meets SQL complexity requirements
	Set-EnvFileVariable "SQL_SA_PASSWORD" -Value (Get-SitecoreRandomString 19 -DisallowSpecial -EnforceComplexity)
	
	# SQL_SERVER
    Set-EnvFileVariable "SQL_SERVER" -Value "mssql"

    # SQL_SA_LOGIN
    Set-EnvFileVariable "SQL_SA_LOGIN" -Value "sa"
	
	# SITECORE_ADMIN_PASSWORD
	Set-EnvFileVariable "SITECORE_ADMIN_PASSWORD" -Value $AdminPassword
	
	# EXPERIENCE_EDGE_TOKEN
	Set-EnvFileVariable "EXPERIENCE_EDGE_TOKEN" -Value $Edge_Token
	
	# JSS_EDITING_SECRET
	Set-EnvFileVariable "JSS_EDITING_SECRET" -Value (Get-SitecoreRandomString 64 -DisallowSpecial)

    # OKTA_DOMAIN
	Set-EnvFileVariable "OKTA_DOMAIN" -Value $OKTA_Domain

    # OKTA_CLIENT_ID
	Set-EnvFileVariable "OKTA_CLIENT_ID" -Value $OKTA_Client_Id

    # OKTA_CLIENT_SECRET
	Set-EnvFileVariable "OKTA_CLIENT_SECRET" -Value $OKTA_Client_Secret
}

Write-Host "Done!" -ForegroundColor Green

##########################
# Show Certificate Details
##########################
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