$result = dotnet sitecore cloud deployment create --environment-id 7FcX57sQPthjvguD4WHGdW --upload --json | ConvertFrom-Json
#$result = '{   "IsTimedOut": false,   "IsCompleted": true,   "StatusByTask": {     "Provisioning": "Complete",     "Build": "Complete",     "Deployment": "Complete",     "PostAction": "N/A",     "Calculated": "Complete"   },   "DurationByTask": {     "Provisioning": "00:03:05.8660827",     "Build": "00:00:00",     "Deployment": "00:00:00",     "PostAction": "00:00:00",     "Calculated": "00:05:57.8140322"   },   "ErrorByTask": {     "Provisioning": null,     "Build": null,     "Deployment": null,     "PostAction": null,     "Calculated": null   },   "Instance": "https://xmc-sitecoresaac8f3-xmcloudintrea43-staging.sitecorecloud.io/" }' 
Write-Host $result
if($result.IsTimedOut) {
    Write-Error -Message "Operation Timed Out." -ErrorAction Stop
    exit -1
}
if(-not $result.IsCompleted) {
    Write-Error -Message "Operation Failed." -ErrorAction Stop
    exit -1
}
Write-Host "Deployment  Completed"