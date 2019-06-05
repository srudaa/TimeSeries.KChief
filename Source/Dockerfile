FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

VOLUME [ "/app/data" ]

COPY Build/MSBuild/* ./Build/MSBuild/
COPY Source/*.csproj ./Source/

WORKDIR /app/Source

RUN dotnet restore

COPY Source/ ./
RUN dotnet publish -c Debug -o out

FROM microsoft/dotnet:2.1-runtime-stretch-slim

#RUN apt-get update && \
#    apt-get install -y --no-install-recommends unzip procps && \
#    rm -rf /var/lib/apt/lists/*

#RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg

WORKDIR /app
COPY --from=build-env /app/Source/out ./

#RUN useradd -ms /bin/bash moduleuser
#USER moduleuser

ENTRYPOINT ["dotnet", "Dolittle.TimeSeries.KChief.dll"]