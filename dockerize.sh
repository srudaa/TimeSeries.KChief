#!/bin/bash
export VERSION=$(git tag --sort=-version:refname | head -1)
docker build --no-cache -f ./Source/Dockerfile -t dolittle/timeseries-kchief:$VERSION . --build-arg CONFIGURATION="Release"
docker push dolittle/timeseries-kchief:$VERSION