#!/bin/bash

cd "$(dirname "$0")"
flyway -configFiles=conf/flyway.conf migrate
