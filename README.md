# 📝 Introduction
**This repository is not a Starter Kit or Template Solution**, you should not clone this repository for the purposes of starting a new Sitecore project. This is intended as a reference example of a Sitecore XM Cloud implementation.

This repository contains the codebase for a series of sites managed by the Technical Marketing Team at Sitecore. You will find the following sites in this repository:
- [Sitecore MVP Site](https://mvp.sitecore.com)
- [SUGCON EU Site](https://europe.sugcon.events)
- [SUGCON ANZ Site](https://anz.sugcon.events)
- [SUGCON India Site](https://india.sugcon.events)
- [SUGCON NA Site](https://na.sugcon.events)
- [SUGCON Events Site](https://www.sugcon.events)

# ✅ Build Status
[![XM Cloud Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_XM_Cloud.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_XM_Cloud.yml)
[![MVP Site Deploy Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_MVP.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_MVP.yml)
[![SUGCON 2024 Status](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_SUGCON_24.yml/badge.svg?branch=main)](https://github.com/Sitecore/XM-Cloud-Introduction/actions/workflows/CI-CD_SUGCON_24.yml)

# ✋ Prerequisites
- [DotNet 8.0](https://dotnet.microsoft.com/en-us/download)
- [NodeJS 16 LTS](https://nodejs.org/en/download/) (or greater)
- [Docker](https://www.docker.com/) (Not required when running in Edge Development Mode without Docker)

## Docker Compose v2
This repository uses Docker Compose v2. As of June 2023 v1 is no longer supported by Docker. You can read more about the differences between the two versions [here](https://docs.docker.com/compose/compose-v2/).

## Base Image Versions
This repositories maintainers intend to keep this repository running against the latest Docker Windows Container versions as possible to reduce image size and increase performance. You may need to downgrade to an earlier Base OS version if you are running an older Host OS. You can read more about this on the [Wiki](../../wiki/Changing-local-Docker-Image-Base-OS-Versions)

## Okta Account
If you wish to run the MVP Site you will need to provide Okta configuration details. You can generate these values for yourself by [Signing up for an Okta Developer Account](https://developer.okta.com/signup/)

## Auth0 Account
When you interact with XM Cloud you will be asked to authenticate with our Auth0 tenant. If you don't have an account already you can follow the registration flow provided by the Auth0 to create one.

# ▶️ Initializing the repository

You first need to initialize the repository, which will configure how the different application elements will run. There are a series of parameters you can pass into the `init.ps1` script to achieve this, you can find the full list of parameters with their purpose here.

You must be in "administrator mode" to run the `init.ps1` script.

Examples of how to use them can be seen in the setup guides for the different running modes below .

| Parameter                     | Required for MVP Site | Required for SUGCON Sites | Required for full local | Required for Edge Mode | Purpose                                                         |
|-------------------------------|-----------------------|---------------------------|-------------------------|------------------------|-----------------------------------------------------------------|
| LicenseXmlPath                | No                    | No                        | **Yes**                 | No                     | Used to specify the path to the license file                    |
| AdminPassword                 | No                    | No                        | **Yes**                 | No                     | Used to specify the password for the Sitecore admin user        |
| InitEnv                       | No                    | No                        | No                      | No                     | Used to force a full initialization of the repository           |
| Edge_Token                    | No                    | No                        | No                      | **Yes**                | Used to authenticate with XM Cloud, when running in 'Edge Mode' |
| OKTA_Domain                   | **Yes**               | No                        | No                      | No                     | Okta domain used by the MVP Rendering host                      |
| OKTA_Client_Id                | **Yes**               | No                        | No                      | No                     | Okta Client Id used by the MVP Rendering host                   |
| OKTA_Client_Secret            | **Yes**               | No                        | No                      | No                     | Okta Client Secret used by the MVP Rendering host               |
| MVP_Selections_API            | No                    | No                        | No                      | No                     | URL for the MVP Selections API                                  |
| SUCGON_ANZ_CDP_CLIENT_KEY     | No                    | No                        | No                      | No                     | CDP Client key for SUGCON ANZ Site                              |
| SUCGON_ANZ_CDP_TARGET_URL     | No                    | No                        | No                      | No                     | CDP Target URL for SUGCON ANZ Site                              |
| SUCGON_ANZ_CDP_POINTOFSALE    | No                    | No                        | No                      | No                     | CDP POS for SUGCON ANZ Site                                     |
| SUCGON_EU_CDP_CLIENT_KEY      | No                    | No                        | No                      | No                     | CDP Client key for SUGCON EU Site                               |
| SUCGON_EU_CDP_TARGET_URL      | No                    | No                        | No                      | No                     | CDP Target URL for SUGCON EU Site                               |
| SUCGON_EU_CDP_POINTOFSALE     | No                    | No                        | No                      | No                     | CDP POS for SUGCON EU Site                                      |
| SUCGON_INDIA_CDP_CLIENT_KEY   | No                    | No                        | No                      | No                     | CDP Client key for SUGCON India Site                            |
| SUCGON_INDIA_CDP_TARGET_URL   | No                    | No                        | No                      | No                     | CDP Target URL for SUGCON India Site                            |
| SUCGON_INDIA_CDP_POINTOFSALE  | No                    | No                        | No                      | No                     | CDP POS for SUGCON India Site                                   |
| SUCGON2024_EU_CDP_CLIENT_KEY  | No                    | No                        | No                      | No                     | CDP Client key for SUGCON2024 EU Site                           |
| SUCGON2024_EU_CDP_TARGET_URL  | No                    | No                        | No                      | No                     | CDP Target URL for SUGCON2024 EU Site                           |
| SUCGON2024_EU_CDP_POINTOFSALE | No                    | No                        | No                      | No                     | CDP POS for SUGCON2024 EU Site                                  |
| Edge_NoDocker                 | No                    | No                        | No                      | No                     | Used to initialize the repo for Edge Developer without Docker   |

## Embedded Personalization
Embedded Personalization is disabled locally by default. If you wish to enable it you will need to populate the different `xx_CDP_CLIENT_KEY`, `xx_CDP_TARGET_URL` & `xx_CDP_POINTOFSALE` parameters with valid values and change the `NODE_ENV` parameter from `development` to another value.

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

### Create a new environment and deploy the codebase
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
.\init.ps1 -InitEnv -LicenseXmlPath "C:\path\to\license.xml" -AdminPassword "DesiredAdminPassword" -Edge_Token "<<YOUR_EDGE_TOKEN>>"
```

### Bring up all the application elements for Edge Mode with Docker
Once you have initialized the repository with your Edge Token, use the `up.ps1` script, ensuring to pass the `-UseEdge` parameter to bring up all of the containers required for Edge Mode.

```ps1
.\up.ps1 -UseEdge
```

## 💻+🌏 Running in Edge Development Mode without Docker

### Initialize your repository for Edge Development Mode without Docker
First initialize your repo using the `.init/ps1` script, you will need to pass in the `-Edge_Token` & `-EdgeNoDocker` parameters to ensure that the repository is correct initialized.

```ps1
.\init.ps1 -InitEnv -Edge_Token "<<YOUR_EDGE_TOKEN>>" -Edge_NoDocker
```

Running with the `-EdgeNoDocker` switch, will setup configuration files for each of the heads included in this repository. You can then manually run each of them using either NPM or DotNet.

### Running the MVP Site

After completing the init setup above you will be able to run the MVP Site either directly from within Visual Studio, or by using the DotNet CLI.

- To run from within Visual Studio, open the `src\XmCloudIntroduction.sln`, ensure that the `Mvp.Project.MvpSite.Rendering` project is set as your StartUp Project, then hit F5.
  - The site should then be started loaded in the browser automatically
- To run from the DotNet CLI, open a new terminal window and navigate to the `src\Project\MvpSite\rendering` folder, then run `dotnet restore && dotnet run`
  - You can then access the site at `https://localhost:5001` or `http://localhost:5000`

### Running the SUGCON Sites

After completing the init setup above you will be able to run the SUGCON Sites directly using the NPM CLI, they are all built using SXA Headless so the process is the same for each of them.

- Open a new terminal window and navigate to the `src\Project\Sugcon\<<SUGCON_SITE>>` folder, then run `npm i && npm run start:connected`
  - You can then access the site at `http://localhost:3000`