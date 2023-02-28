# escape=`

ARG DEBUG_BASE_IMAGE
ARG RELEASE_BASE_IMAGE
ARG SOLUTION_IMAGE

FROM ${SOLUTION_IMAGE} as solution

# Debug image (sdk image)
FROM ${DEBUG_BASE_IMAGE} as debug
ARG ARTIFACTS_FOLDER=/artifacts/mvp-rendering/

USER ContainerAdministrator
WORKDIR /app/src/project/MvpSite/rendering
USER ContainerUser
ENV ASPNETCORE_URLS "http://+:80"
EXPOSE 80

ENTRYPOINT ["dotnet", "watch"]