ARG BASE_IMAGE
FROM ${BASE_IMAGE}
ARG PROJECT_FILE
WORKDIR /app

COPY ${PROJECT_FILE} .
COPY ./headapps/MvpSite/Directory.Packages.props ./../
COPY ./nuget.config ./../

COPY ./headapps/MvpSite ./

WORKDIR /app/MvpSite.Rendering
CMD ["dotnet", "watch", "run", -v, "--urls", "http://*:80"]