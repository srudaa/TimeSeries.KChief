#!/bin/bash
export VERSION=$(git tag --sort=-version:refname | head -1)
docker build --no-cache -f ./Source/Dockerfile -t dolittle/timeseries-kchief:$VERSION .
docker push dolittle/timeseries-kchief:$VERSION