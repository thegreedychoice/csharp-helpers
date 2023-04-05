#!/bin/bash

/opt/mssql/bin/sqlservr &

for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i init-db.sql
    if [ $? -eq 0 ]
    then
        echo "Database initialized"
        break
    else
        echo "Waiting for SQL Server: $i..."
        sleep 1
    fi
done