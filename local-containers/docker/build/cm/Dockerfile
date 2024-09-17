# escape=`

ARG PARENT_IMAGE
ARG TOOLS_IMAGE

FROM ${TOOLS_IMAGE} as tools
FROM ${PARENT_IMAGE}

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

WORKDIR C:\inetpub\wwwroot

# Copy developer tools and entrypoint
COPY --from=tools C:\tools C:\tools

# Copy authoring config files
COPY /authoring/platform/Mvp.Project.MvpSite.Platform/App_Config ./App_Config
COPY /authoring/platform/Mvp.Environment.Platform/App_Config ./App_Config