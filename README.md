# 📝 Introduction
**This repository is not a Starter Kit or Template Solution**, you should not clone this repository for the purposes of starting a new Sitecore project. This is intended as a reference example of a Sitecore XM Cloud implementation.

This repository contains the codebase for a series of sites managed by the Technical Marketing Team at Sitecore. You will find the following sites in this repository:
- [Sitecore MVP Site](https://mvp.sitecore.com)
- [SUGCON EU Site](https://europe.sugcon.events)
- [SUGCON ANZ Site](https://anz.sugcon.events)
- [SUGCON Events Site](https://www.sugcon.events)

# ✅ Build Status
[![XM Cloud Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_XM_Cloud.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_XM_Cloud.yml)
[![MVP Site Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_MVP.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_MVP.yml)
[![SUGCON ANZ Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_SUGCON_ANZ.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_SUGCON_ANZ.yml)
[![SUGCON EU Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_SUGCON_EU.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_SUGCON_EU.yml)

# ✋ Prerequisites
- [DotNet 6.0](https://dotnet.microsoft.com/en-us/download)
- [NodeJS 16 LTS](https://nodejs.org/en/download/) (or greater)
- [Docker](https://www.docker.com/)

This repositories maintainers intend to keep this repository running against the latest Docker Windows Container versions as possible to reduce image size and increase performance. You may need to downgrade to an earlier Base OS version if you are running an older Host OS. You can read more about this on the [Wiki](../../wiki/Changing-local-Docker-Image-Base-OS-Versions)

## Okta Account
If you wish to run the MVP Site you will need to provide Okta configuration details. You can generate these values for yourself by [Signing up for an Okta Developer Account](https://developer.okta.com/signup/)

## Auth0 Account
When you interact with XM Cloud you will be asked to authenticate with our Auth0 tenant. If you don't have an account already you can follow the registration flow provided by the Auth0 to create one.

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
| MVP_Selections_API | **Yes**               | No                        | URL for the MVP Selections API                                  |

# 🏃 Running the Repository
After you have initialized the repository, you can run it using the `up.ps1` script. There are 3 different ways you can run this repository
- [Full local development Mode](#-running-in-full-local-development-mode)
- [Edge development with Docker](#-running-in-edge-development-mode-with-docker)
- [Edge development without Docker](#-running-in-edge-development-mode-without-docker)

The different parameters on the `up.ps1` script control which mode you want to select, along with some other options.

| Parameter     | Required  | Purpose                                                                         |
|---------------|-----------|---------------------------------------------------------------------------------|
| UseEdge       | No        | Used to run the head applications against Edge, not against a local CM instance |
| SkipBuild     | No        | Used to skip running the `docker build` command                                 |
| SkipIndexing  | No        | Used to skip indexing the content                                               |
| SkipPush      | No        | Used to skip pushing the local content into the CM                              |
| SkipOpen      | No        | Okta to skip automatically opening the sites                                    |


## 🐋 Running in Full Local Development Mode
Running in Local Mode will run all of the application elements required on your local machine using Docker

### Initialize your repository for Local Mode
First initialize your repo using the `.init/ps1` script, you can see an example of how to init for 'Local Mode' below

```ps1
.\init.ps1 -InitEnv -LicenseXmlPath "C:\path\to\license.xml" -AdminPassword "DesiredAdminPassword"
```

### Bring up all the application elements for Local Mode
Next, use the `up.ps1` script to bring up all of the containers required for Local Mode.

```ps1
.\up.ps1
```

## 🐋+🌏 Running in Edge Development Mode with Docker
Running in Edge Mode with Docker will run only run the Head applications and Traefik. To be able to run the sites in this repository locally against Edge, you will need to have access to an active XM Cloud Subscription with enough entitlements to be able to create a new Project & Environment to run against.

## Create a new environment and deploy the codebase
You can follow these steps to create a new XM Cloud Project & Environment, then generate an Edge Token used to authenticate your Head applications with your Edge Tenant.
- Authenticate with the XM Cloud Deploy Application
  - `dotnet sitecore cloud login`
- Create a new XM Cloud Project
  - `dotnet sitecore cloud project create -n <<YOUR_PROJECT_NAME>>`
  - _Record the returned Project ID._
- Create an environment that you're going to deploy to:
  - `dotnet sitecore cloud environment create --project-id <<YOUR_PROJECT_ID>> -n <<ENVIRONMENT_NAME>>`
  - _Record the returned Environment ID._
- Create a deployment and push the local codebase into XM Cloud
  - `dotnet sitecore cloud deployment create -id <<YOUR_ENVIRONMENT_ID>> --upload`
- Access the CM instance that has been created and perform a "Site Publish" to ensure all content items have been pushed to the Edge.
- Generate an Edge Token used to allow your Rendering hosts to authenticate with XM Cloud
  - `.\New-EdgeToken.ps1 -EnvironmentId <<YOUR_ENVIRONMENT_ID>>`
  - _Record the returned Edge Token, and Edge URL._

If you want more information about the Cloud plugin for the CLI then you access it on the [documentation site](https://doc.sitecore.com/xmc/en/developers/xm-cloud/the-cloud-deployment-command.html).

### Initialize your repository for Edge Development Mode with Docker
First initialize your repo using the `.init/ps1` script, you will need to pass in the `-Edge_Token` parameter set to the value you generated in the previous step.

```ps1
.\init.ps1 -InitEnv -LicenseXmlPath "C:\path\to\license.xml" -AdminPassword "DesiredAdminPassword" --Edge_Token "<<YOUR_EDGE_TOKEN>>"
```

## Bring up all the application elements for Edge Mode with Docker
Once you have initialised the repository with your Edge Token, use the `up.ps1` script, ensuring to pass the `-UseEdge` parameter to bring up all of the containers required for Edge Mode.

```ps1
.\up.ps1 -UseEdge
```

## 💻+🌏 Running in Edge Development Mode without Docker
Coming soon...