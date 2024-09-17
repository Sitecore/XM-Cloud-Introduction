ARG BASE_IMAGE
FROM ${BASE_IMAGE}
ARG PROJECT_FILE
WORKDIR /app

COPY ${PROJECT_FILE} .
COPY ./headapps/MvpSite/Directory.Build.targets ./../
COPY ./headapps/MvpSite/Packages.props ./../
COPY ./nuget.config ./../

COPY ./headapps/MvpSite ./

CMD ["dotnet", "watch", "run", -v, "--urls", "http://*:80"]