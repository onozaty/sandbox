#!/bin/bash

cd "$(dirname "$0")"
java -jar schemaspy-6.2.4.jar -configFile schemaspy-enum.properties
