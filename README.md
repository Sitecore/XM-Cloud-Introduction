# Introduction

This repository contains the codebase for a series of sites managed by the Technical Marketing Team at Sitecore. You will find the following sites in this repository:
- [Sitecore MVP Site](https://mvp.sitecore.com)
- [SUGCON EU Site](https://europe.sugcon.events)
- [SUGCON ANZ Site](https://anz.sugcon.events)

# Prerequisites

Prerequisites for this repository are...

# Local Development

## Intialise the repository
Run the following PowerShell command from an elevated prompt.

```ps1
.\init.ps1 -InitEnv -LicenseXmlPath "C:\path\to\license.xml" -AdminPassword "DesiredAdminPassword" -Auth0_ClientId "<<Auth0_Client_Id>>" -Auth0_ClientSecret "<<Auth0_Client_Secret>>"
```

## Run the repository
Run the following PowerShell command to bring up the application locally.

```ps1
.\up.ps1
```

# Deploying to XM Cloud

## Create a new environment and deploy the codebase

Create an environment that you're going to deploy to:
* `dotnet sitecore cloud environment create --project-id <<YOUR_PROJECT_ID>> -n mvp-site-definition`

Record the returned Environment ID.

Create a deployment and push the local codebase into XM Cloud
* `dotnet sitecore cloud deployment create -id <<YOUR_ENVIRONMENT_ID>> --upload`

## Deserialise the content items into XM Cloud

The inital deploy performed above will move all of the items into the instance automatically. To perform a further manual sync you can use the following commands:

* Connect your local CLI instance using `dotnet sitecore cloud environment connect -id <<YOUR_ENVIRONMENT_ID>>`
* Push the content to your XM Cloud instance using `dotnet sitecore ser push`

## Running the MVP Site Rendering Host
Currently the MVP Site Rendering Host isn't running inside of a container, work needs to be completed to set this up. 

It will run successfully against the local CM instance, however, none of the features have been migrated yet - so the homepage has no renderings and just a success message, work needs to be completed to migrate the features.

You can run it manually with the following commands:

```ps1
cd src/project/MvpSite/rendering
dotnet build
dotnet watch
```

# Testing Edge GraphQL Endpoint

Ensure you have performed a publish of the content first, or you will have data available in Edge.

## Create an Edge Token
Running the following script with the environment id from the previous steps will create an Edge access token and launch the GraphQL Playground so that you can query content.

```ps1
.\New-EdgeToken.ps1 -EnvironmentId {ENVIRONMENT_ID}
```

Add the following to HTTPHeaders

```
{ "X-GQL-Token" : "<<TOKEN_RETURNED_BY_EDGE_TOKEN_SCRIPT_ABOVE>>" }
```

Run the following GraphQL Query to test getting the MVP Site Information, along with the layout data for the MVP Homepage

```
{
  site {
    siteInfo(site:"mvp-site") {
      name
    }
  }, 
  layout(site:"mvp-site", routePath:"/", language:"en") {
    item {
      rendered
    }
  }
}
```