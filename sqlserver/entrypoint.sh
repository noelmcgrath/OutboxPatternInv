#!/bin/bash

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to become available
echo "Waiting for SQL Server to start..."
for i in {1..30}; do
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'YourStron$$88gPassword' -Q "SELECT 1" &>/dev/null && break
  echo -n "."
  sleep 1
done

# Run the seed script
echo "Running seed-data.sql..."
/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U SA -P 'YourStron$$88gPassword' -d master -i /opt/scripts/seed-data.sql

# Bring SQL Server to foreground
wait
