FROM mcr.microsoft.com/mssql/server:2019-latest

ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD='YourStron$$88gPassword'
ENV MSSQL_PID=Express

EXPOSE 1433

# Create directory and copy seed script
RUN mkdir -p /opt/scripts
COPY seed-data.sql /opt/scripts/

# Copy and fix permissions on entrypoint.sh
COPY entrypoint.sh /usr/src/app/entrypoint.sh

# Switch to root to make it executable
USER root
RUN chmod +x /usr/src/app/entrypoint.sh

# Switch back to mssql (the default SQL Server user)
USER mssql

CMD ["/usr/src/app/entrypoint.sh"]
