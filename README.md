#  ⚠️ Pre-release software ⚠️
This project is being built against a pre-release version of XM Cloud. There will most likely be breaking changes introduced that will require (possibly extensive) refactoring of the code seen here. 

During this time it is not possible for people external to Sitecore's Technical Marketing team to run this project locally. We will also not be accepting community contributions during this time.

You can `watch` this repository to follow along with our development progress as we prepare for the full public release of XM Cloud. Once we reach that point, we will then define a process for community contributions.

# Build Status

[![XM Cloud Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/deploy_XM_Cloud.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/deploy_XM_Cloud.yml)
[![MVP Site Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/deploy_MVP.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/deploy_MVP.yml)
[![SUGCON ANZ Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/deploy_SUGCON_ANZ.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/deploy_SUGCON_ANZ.yml)
[![SUGCON EU Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/deploy_SUGCON_EU.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/deploy_SUGCON_EU.yml)

# Introduction

This repository contains the codebase for a series of sites managed by the Technical Marketing Team at Sitecore. You will find the following sites in this repository:
- [Sitecore MVP Site](https://mvp.sitecore.com)
- [SUGCON EU Site](https://europe.sugcon.events)
- [SUGCON ANZ Site](https://anz.sugcon.events)

# Prerequisites

Prerequisites for this repository are:
- [DotNet 6.0](https://dotnet.microsoft.com/en-us/download)
- [NodeJS 16 LTS](https://nodejs.org/en/download/) (or greater)

# Initializing the repository
You first need to initialize your .env file which will configure how the different application elements will run. There are a series of parameters you can pass in to override the default behaviour of the application, you can find the full list of parameters with their purpose here.

Examples of how to use them can be seen in the different setup guides for both Local Mode and Edge Mode below.

| Parameter          | Required? | Purpose                                                                                                      |
|--------------------|-----------|--------------------------------------------------------------------------------------------------------------|
| LicenseXmlPath     | Yes       | Used to specify the path to the license file                                                                 |
| AdminPassword      | Yes       | Used to specify the password for the Sitecore admin user                                                     |
| InitEnv            | No        | Used to force a full initialisation of the repository                                                        |
| Edge_Token         | No        | Used to authenticate with XM Cloud, when running in 'Edge Mode'                                              |
| OKTA_Domain        | No        | Okta domain used by the MVP Rendering host                                                                   |
| OKTA_Client_Id     | No        | Okta Client Id used by the MVP Rendering host                                                                |
| OKTA_Client_Secret | No        | Okta Client Secret used by the MVP Rendering host                                                            |

# Running in Local Mode

Running in Local Mode will run all of the application elemenets required on your local machine using Docker

## Initialize your repository for Local Mode

First initialize your repo using the `.init/ps1` script, you can see an example of how to init for 'Local Mode' below

```ps1
.\init.ps1 -InitEnv -LicenseXmlPath "C:\path\to\license.xml" -AdminPassword "DesiredAdminPassword"
```

## Bring up all the application elememnts for Local Mode

Next, use the `up.ps1` script to bring up all of the containers required for Local Mode.

```ps1
.\up.ps1
```

# Running in Edge Mode

Running in Edge Mode will run only run the Host applications and Traefik used to access them. The hosts will pull their data directly from XM Cloud

## Create a new environment and deploy the codebase

- Create an environment that you're going to deploy to:
  - `dotnet sitecore cloud environment create --project-id <<YOUR_PROJECT_ID>> -n <<ENVIRONMENT_NAME>>`
  - _Record the returned Environment ID._
- Create a deployment and push the local codebase into XM Cloud
  - `dotnet sitecore cloud deployment create -id <<YOUR_ENVIRONMENT_ID>> --upload`
- Access the CM instance that has been created and perform a "Site Publish" to ensure all content items have been pushed to the Edge.
- Generate an Edge Token used to allow your Rendering hosts to authenticate with XM Cloud
  - `.\New-EdgeToken.ps1 -EnvironmentId <<YOUR_ENVIRONMENT_ID>>`
  - _Record the returned Edge Token, and Edge URL._

## Initialize your repository for Edge Mode

Next initialize your repo using the `.init/ps1` script, you can see an example of how to init for 'Edge Mode' below

```ps1
.\init.ps1 -InitEnv -LicenseXmlPath "C:\path\to\license.xml" -AdminPassword "DesiredAdminPassword" -Edge_Token "<<Edge_Token>>"
```

## Bring up all the application elememnts for Edge Mode

Next, use the `up.ps1` script to bring up all of the containers required for Edge Mode.

```ps1
.\up.ps1 -UseEdge
```