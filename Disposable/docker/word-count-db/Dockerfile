FROM psdispose/sql-server:2019

WORKDIR /init-db
COPY init-db/ .

USER root
RUN chmod +x init-db.sh 

USER mssql
RUN ./init-db.sh