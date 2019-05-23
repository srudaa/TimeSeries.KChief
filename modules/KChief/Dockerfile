FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

VOLUME [ "/app/data" ]

COPY Build/MSBuild/* ./Build/MSBuild/
COPY modules/KChief/*.csproj ./modules/KChief/

WORKDIR /app/modules/KChief

RUN dotnet restore

COPY modules/KChief/ ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-runtime-stretch-slim
WORKDIR /app
COPY --from=build-env /app/modules/KChief/out ./

#RUN useradd -ms /bin/bash moduleuser
#USER moduleuser

ENTRYPOINT ["dotnet", "Dolittle.TimeSeries.KChief.dll"]