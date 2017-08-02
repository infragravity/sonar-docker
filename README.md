# Overview
Sonar is metric collection agent wrtten in .NET Core 2 for gathering data from WMI using WS-Management protocol. It can collect data remotely when deployed as Docker container. Sonar sends collected metrics to InfluxDb time series database using UDP transport. The purpose of Sonar is to complement Telegraf from InfluxData TICK stack that does not support metric collection from WMI.
This repository contains configuration files for deploying Sonar and its dependencies using Docker. 

# Installation
The below steps describe how to use Docker to create and deploy example environment with Sonar, InfluxDb and Telegraf. The purpose of Telegraf in this case is to monitor performance for Docker containers.  
## Deployment
1. Clone the repository.
2. Review Sonar configuration files with sample WMI queries in WQL. Change Windows host name and user credentials for accessing WinRM on remote host. Note that domain name is not required for the basic authentication.
3. Configure WinRM to allow remote connections. Only Basic authentication is supported via HTTP or HTTPS.
4. Customize docker-compose.yml file if you need to point to  (optional)
5. Create and start containers:
``` 
  docker-compose up -d
```
## Configuration
Open InfluxDb administration web interface and create new database for storing collected metrics:
```
  create database sonar
```
The configuration in influxdb.conf includes UDP configuration to listen on port 8092 for receiving events from Sonar container and storing them in database named 'sonar'. 
## Configuring WinRM
WinRM can be configured to use Basic authentication using the following commands(run as Adminstrator):
```
  winrm quickconfig
  winrm set winrm/config/client/auth '@{Basic="true"}'
  winrm set winrm/config/service/auth '@{Basic="true"}'
  winrm set winrm/config/service '@{AllowUnencrypted="true"}'
```
## Troubleshooting
The level of diagnostic logging (LogLevel) is configurable in docker-compose file. This setting can be changed to 'Debug' for displaying low level diagnostics.
