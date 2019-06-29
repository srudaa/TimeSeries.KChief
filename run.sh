#!/bin/bash
docker build -f ./Source/Dockerfile -t shipos/timeseries-kchief . --build-arg CONFIGURATION="Debug"
iotedgehubdev start -d deployment.json -v