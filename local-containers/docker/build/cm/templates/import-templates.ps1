param(
    [Parameter(Mandatory)]$RenderingSiteName,
    [Parameter(Mandatory)]$SitecoreApiKey
)

function ReplaceExistingModules {
    param(
        $srcConfig,
        $dstConfig
    )
    $sitecoreJson = Get-Content $srcConfig | ConvertFrom-Json
    $sitecoreJson.modules = @(".\modules\*.module.json")
    $sitecoreJson | ConvertTo-Json -Depth 32 | Out-File $dstConfig -Force
}

$modulesPath = (Join-Path $PSScriptRoot "modules")
if (Test-Path $modulesPath) {
    Remove-Item $modulesPath -Recurse -Force
}

Copy-Item -Path (Join-Path $PSScriptRoot "modules_template") -Destination $modulesPath -Recurse -Force
Copy-Item -Path ".\.sitecore"  -Destination $PSScriptRoot -Recurse -Force
$files = Get-ChildItem -Path $modulesPath -Recurse -File | ForEach-Object {$_.FullName}
foreach ($item in $files) {
    (Get-Content -Path $item -Encoding UTF8).Replace("<SITENAME>", $RenderingSiteName).Replace("<SITECORE-API-KEY>", $SitecoreApiKey) `
    | Set-Content -Path $item -Encoding UTF8
}

$localConfig = (Join-Path $PSScriptRoot "sitecore.json")
ReplaceExistingModules -srcConfig ".\sitecore.json" -dstConfig $localConfig
Write-Host "Pushing sitecore api key to Sitecore..." -ForegroundColor Green
dotnet sitecore ser push -s --config $PSScriptRoot

