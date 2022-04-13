# Introduction

This repository contains the codebase for a series of sites managed by the Technical Marketing Team at Sitecore. You will find the following sites in this repository:
- [Sitecore MVP Site](https://mvp.sitecore.com)
- [SUGCON EU Site](https://europe.sugcon.events)
- [SUGCON ANZ Site](https://anz.sugcon.events)

# Prerequisites

Prerequisites for this repository are...

# Local Development

Setup steps for local development are...

# Deserialise content into XM Cloud instance

* Connect your local CLI instance using `dotnet sitecore cloud environment connect -id <<ENV_ID>>`
* Push the content to your XM Cloud instance using `dotnet sitecore ser push`

## Troubleshooting Deserialisation

* Environment Default was not defined. Use the Login command to define it.
    - You need to edit your `/.sitecore/user.json` file, duplicate the XM Cloud node created after your call the `connect` command, and call the new instance `default`.

* Unexpected HttpResponseMessage with code: RequestEntityTooLarge.
    - This is probably due to media library items being included in your serialised items and the data size is larger than the NGINX ingress limit.
