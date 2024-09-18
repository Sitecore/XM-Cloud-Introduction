# XM Cloud Introduction - Disconnected offline mode
Below are the instructions for how to mock a small subset of the XM Cloud Application elements in offline mode using Docker. This can allow for a disconnected development, however it is recommend to work in the default connected mode for the best experience.

## ✋ Prerequisites
- DotNet 8.0 (https://dotnet.microsoft.com/en-us/download)
- Docker (https://www.docker.com/products/docker-desktop)
- A windows based machine is required to run the local containers

### Docker Compose v2
This repository uses Docker Compose v2. As of June 2023 v1 is no longer supported by Docker. You can read more about the differences between the two versions [here](https://docs.docker.com/compose/compose-v2/).

### Base Image Versions  
This repositories maintainers intend to keep this repository running against the latest Docker Windows Container versions as possible to reduce image size and increase performance. You may need to downgrade to an earlier Base OS version if you are running an older Host OS. You can read more about this on the [Wiki](../../wiki/Changing-local-Docker-Image-Base-OS-Versions)

## ▶️ Initializing the repository

You first need to initialize the repository, which will configure how the different application elements will run. There are a series of parameters you can pass into the `./local-containers/scripts/init.ps1` script to achieve this, you can find the full list of parameters with their purpose here.

You must be in "administrator mode" to run the `./local-containers/scripts/init.ps1` script.

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

### Embedded Personalization
Embedded Personalization is disabled locally by default. If you wish to enable it you will need to populate the different `xx_CDP_CLIENT_KEY`, `xx_CDP_TARGET_URL` & `xx_CDP_POINTOFSALE` parameters with valid values and change the `NODE_ENV` parameter from `development` to another value.

You can see an example of how to use the init script below:

```ps1
.\init.ps1 -InitEnv -LicenseXmlPath "C:\path\to\license.xml" -AdminPassword "DesiredAdminPassword"
```

## 🏃 Running the Repository
The different parameters on the `./local-containers/scripts/up.ps1` script control which mode you want to select, along with some other options.

| Parameter     | Required  | Purpose                                                                         |
|---------------|-----------|---------------------------------------------------------------------------------|
| SkipBuild     | No        | Used to skip running the `docker build` command                                 |
| SkipIndexing  | No        | Used to skip indexing the content                                               |
| SkipPush      | No        | Used to skip pushing the local content into the CM                              |
| SkipOpen      | No        | Okta to skip automatically opening the sites                                    |

You can see an example of how to use the up script below:

```ps1
.\up.ps1
```

## ⛔ Stopping the Containers
Once you have finished you can use the `./local-containers/scripts/down.ps1` script to stop the containers:

```ps1
./local-containers/scripts/down.ps1
```

This will stop all containers, and tidy up any resources that were created.