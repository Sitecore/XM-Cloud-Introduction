#  ⚠️ Pre-release software ⚠️
This project is being built against a pre-release version of XM Cloud. There will most likely be breaking changes introduced that will require (possibly extensive) refactoring of the code seen here. You can clone this reponsitory and run Full Local Development Mode, however to test Edge Development Mode you will need to have your own XM Cloud tenant.  

We will not be accepting community contributions during this initial pre-release build phase. Once we pass this point, we will then define a process for community contributions.

# 📝 Introduction
This repository is not a Starter Kit or Template Solution, you should not clone this repository for the purposes of starting a new Sitecore project. This is intended as a reference example of a Sitecore XM Cloud implementation.

This repository contains the codebase for a series of sites managed by the Technical Marketing Team at Sitecore. You will find the following sites in this repository:
- [Sitecore MVP Site](https://mvp.sitecore.com)
- [SUGCON EU Site](https://europe.sugcon.events)
- [SUGCON ANZ Site](https://anz.sugcon.events)

# ✅ Build Status
[![XM Cloud Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_XM_Cloud.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_XM_Cloud.yml)
[![MVP Site Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_MVP.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_MVP.yml)
[![SUGCON ANZ Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_SUGCON_ANZ.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_SUGCON_ANZ.yml)
[![SUGCON EU Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_SUGCON_EU.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_SUGCON_EU.yml)

# ✋ Prerequisites
- [DotNet 6.0](https://dotnet.microsoft.com/en-us/download)
- [NodeJS 16 LTS](https://nodejs.org/en/download/) (or greater)
- [Docker](https://www.docker.com/)

# ▶️ Initializing the repository
You first need to initialize your .env file which will configure how the different application elements will run. There are a series of parameters you can pass in to override the default behaviour of the application, you can find the full list of parameters with their purpose here.

Examples of how to use them can be seen in the different setup guides for both Local Mode and Edge Mode below.

| Parameter          | Required for MVP Site | Required for SUGCON Sites | Purpose                                                         |
|--------------------|-----------------------|---------------------------|-----------------------------------------------------------------|
| LicenseXmlPath     | **Yes**               | **Yes**                   | Used to specify the path to the license file                    |
| AdminPassword      | **Yes**               | **Yes**                   | Used to specify the password for the Sitecore admin user        |
| InitEnv            | No                    | No                        | Used to force a full initialisation of the repository           |
| Edge_Token         | No                    | No                        | Used to authenticate with XM Cloud, when running in 'Edge Mode' |
| OKTA_Domain        | **Yes**               | No                        | Okta domain used by the MVP Rendering host                      |
| OKTA_Client_Id     | **Yes**               | No                        | Okta Client Id used by the MVP Rendering host                   |
| OKTA_Client_Secret | **Yes**               | No                        | Okta Client Secret used by the MVP Rendering host               |

## Okta Configuration
If you wish to run the MVP Site you will need to provide Okta configuration details. You can generate these values for yourself by [Signing up for an Okta Developer Account](https://developer.okta.com/signup/)

# 💻 Running in Full Local Development Mode
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

# 🌏 Running in Edge Development Mode
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