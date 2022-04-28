# parse the sitecore.json file for the XM Cloud plugin version
[CmdletBinding(DefaultParameterSetName = 'FromArgs')]
param (
    [Parameter(Mandatory)]
    [string]$EnvironmentId
)

$ErrorActionPreference = "Stop"

$XmCloudVersion = (get-content sitecore.json | ConvertFrom-Json).plugins -match 'Sitecore.DevEx.Extensibility.XMCloud' 
if ($XmCloudVersion -eq '' -or $LASTEXITCODE -ne 0) {
    Write-Error "Unable to find version of XM Cloud Plugin"
}
$XmCloudVersion = ($XmCloudVersion -split '@')[1]
$pluginJsonFile = Get-Item -path "$PSScriptRoot\.sitecore\package-cache\nuget\Sitecore.DevEx.Extensibility.XMCloud.$($XmCloudVersion)\plugin\plugin.json"
$XmCloudDeployApi = (Get-Content $pluginJsonFile | ConvertFrom-Json).xmCloudDeployEndpoint
$XmCloudDeployAccessToken = (Get-Content "$PSScriptRoot\.sitecore\user.json" | ConvertFrom-Json).endpoints.xmCloud.accessToken

$Headers = @{"Authorization" = "Bearer $XmCloudDeployAccessToken" }
$URL = @(
    "$($XmCloudDeployApi)api/environments/v1"
    $EnvironmentId
    'obtain-edge-token'
)

$Response = Invoke-RestMethod ($URL -join '/') -Method 'GET' -Headers $Headers -Verbose
$AccessToken = $Response.apiKey
$EdgeUrl = "$($Response.edgeUrl)/api/graphql/ide"
Write-Host "Launching Edge GraphQL IDE"
Write-Host "Add { ""X-GQL-Token"" : ""$AccessToken"" } to the HTTP HEADERS tab at the bottom-left of the screen to write queries against your content"
Start-Process $EdgeUrl
