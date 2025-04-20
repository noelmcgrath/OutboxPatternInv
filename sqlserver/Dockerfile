FROM mcr.microsoft.com/mssql/server:2019-latest

EXPOSE 1433

ENV MSSQL_SA_PASSWORD='Pa$$word1'
ENV ACCEPT_EULA=true
ENV MSSQL_PID=Express

RUN mkdir -p /opt/scripts
COPY seed-data.sql /opt/scripts
USER mssql

RUN /opt/mssql/bin/sqlservr & sleep 10; /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U SA -P 'Pa$$word1' -d master -i /opt/scripts/seed-data.sql
