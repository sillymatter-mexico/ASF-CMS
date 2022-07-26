FROM grugnog/mysql-5.1:latest

ENV MYSQL_DATABASE "asf_cms"
ENV MYSQL_ROOT_PASSWORD "toor"
ENV MYSQL_USER: "asf_cms_usr"
ENV MYSQL_PASSWORD: "asf-pass-10"

COPY src/App_Data/asf_cms20200410.sql /docker-entrypoint-initdb.d/