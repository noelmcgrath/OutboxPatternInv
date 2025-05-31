#!/bin/sh
echo "Waiting for MySQL..."
sleep 5  # or use wait-for-it here
echo "Starting app..."
exec dotnet OutboxProcessor.dll
