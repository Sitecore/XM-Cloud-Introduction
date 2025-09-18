# 🚫 Not a Starter Kit or Template Solution
**This repository is not a Starter Kit or Template Solution**, you should not clone this repository for the purposes of starting a new Sitecore project. This is intended as a reference example of a Sitecore XM Cloud implementation. If you want to learn XM Cloud, this repository is not a good place to start. You should begin by reading the [XM CLoud Getting Started Guide](https://doc.sitecore.com/xmc/en/developers/xm-cloud/getting-started-with-xm-cloud.html).

# 📝 Introduction
This repository contains the codebase for a series of sites managed by Sitecore. You will find the following sites in this repository:
- [Sitecore MVP Site](https://mvp.sitecore.com)
- [SUGCON Europe Site](https://europe.sugcon.events)
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

## Okta Account
If you wish to run the MVP Site you will need to provide Okta configuration details. You can generate these values for yourself by [Signing up for an Okta Developer Account](https://developer.okta.com/signup/)

# 🏃‍♀️‍➡️ Running the repository
To run this you will need to deploy this project to an XM Cloud environment. 

## Setting up an XM Cloud Environment
This can be achieved using the CLI by following the steps below from within PowerShell:
- Install the Sitecore CLI
  - `dotnet tool install`
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

If you want more information about the Cloud plugin for the CLI then you access it on the [documentation site](https://doc.sitecore.com/xmc/en/developers/xm-cloud/the-cloud-deployment-command.html).

## Running the MVP Site
To run the MVP site you will need to create a Development instance of your settings.

Duplicate the `./headapps/MvpSite/Mvp.Project.MvpSite.Rendering/appsettings.json` and name it `appsettings.Development.json`. 

Populate the Sitecore, and Okta sections. You can find the `EdgeContextId` & `EditingSecret` in the XMC Deploy application, in the Developer Settings section for your environment. The Okta settings are available after completing the Okta Developer sign-up linked above. 

When completed it will look something like:

```json
  "Sitecore": {
    "EdgeContextId": "XXX",
    "EditingSecret": "XXX",
    ...
  },
  ...
  "Okta" : {
    "OktaDomain": "https://your-okta-domain.com",
    "ClientId": "YOUR_OKTA_CLIENT_ID",
    "ClientSecret": "YOUR_OKTA_CLIENT_ID",
    "AuthorizationServerId": "YOUR_OKTA_CLIENT_ID"
  },
```
You will be able to run the MVP Site either directly from within Visual Studio, or by using the DotNet CLI.

- To run from within Visual Studio, open the `./headapps/MvpSite/XMC-Introduction-MVP.sln`, ensure that the `Mvp.Project.MvpSite.Rendering` project is set as your StartUp Project, then hit F5.
  - The site should then be started loaded in the browser automatically
- To run from the DotNet CLI, open a new terminal window and navigate to the `./headapps/MvpSite/Mvp.Project.MvpSite.Rendering` folder, then run `dotnet restore && dotnet run`
  - You can then access the site at `https://localhost:5001` or `http://localhost:5000`

## Running the SUGCON Sites

After completing the init setup above you will be able to run the SUGCON Sites directly using the NPM CLI, they are all built using SXA Headless so the process is the same for each of them.

- Log into the [XM Cloud Deploy Application](https://deploy.sitecorecloud.io/)
- Locate the Project and Environment you created earlier
- Open the `Developer settings` tab
- Choose the Site you wish to load from the dropdown, e.g. EU for the SUGCON Europe Site.
- Create a new `.env` file in the root of the `./headapps/Sugcon2024` folder
- Populate the newly created `.env` file with the values from the `Developer settings` tab, it should look something like:

  ```env
  SITECORE_EDGE_CONTEXT_ID=XXXX
  SITECORE_SITE_NAME=ANZ
  JSS_EDITING_SECRET=XXXX
  ```
- Open a new terminal window and navigate to the `./headapps/Sugcon2024` folder.
- Run the following command to install dependencies and start the site:
  ```bash
  npm i && npm run start:connected
  ```
- You can then access the default site at [http://localhost:3000](http://localhost:3000).

### Switching Between SUGCON Sites
   - To switch to another SUGCON site, update the `JSS_APP_NAME` variable in the `.env` file.
   - Available site options include:
     - `ANZ`
     - `EU`
     - `India`
     - `NA`
     - `Events`

## Disconnected offline development
It is possible to mock a small subset of the XM Cloud Application elements to enable offline development. This can allow for a disconnected development experience, however it is recommend to work in the default connected mode.

You can find more information about how setup the offline development experience [here](./local-containers/README.md)
