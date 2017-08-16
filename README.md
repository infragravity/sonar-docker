# Overview
Sonar is metric collection agent wrtten in .NET Core 2 for gathering data from WMI locally or remotely using WS-Management protocol. Sonar sends collected metrics to InfluxDb time series database using UDP protocol. The purpose of Sonar is to complement Telegraf from InfluxData TICK stack that does not support metric collection from Windows Management Instrumentation(WMI). Metrics are collected from remote host when deployed as container or pod. This repository contains configuration files for deploying Sonar and its dependencies using Docker. Helm chart for Kubernetes is coming soon:) 
## Deployment Scenarios
Sonar can be deployed as Docker container, Kubernetes pod of locally on host. It has been tested to collect metrics from WMI using multiple Windows versions, including Win7(SP1) and Nano Server(TP5). The docker-compose.yml and config files are included in this repository.   
## Documentation
The docs are available at [knowledge base](http://www.infragravity.com/knowledge-base/sonar-overview/)
# Installation
The below steps describe how to use Docker to create and deploy simple environment with Sonar, InfluxDb and Telegraf. The purpose of Telegraf in this case is to monitor performance for Docker containers.
## Docker  
1. Clone the repository.
2. Review Sonar configuration files with sample WMI queries in WQL. Change Windows host name and user credentials for accessing WinRM on remote host. Note that domain name is not required for the basic authentication.
3. Configure WinRM to allow remote connections. Only Basic authentication is supported via HTTP or HTTPS.
4. Customize docker-compose.yml file if you need to point to  (optional)
5. Create and start containers:
``` 
  docker-compose up -d
```
## Windows Host
The following steps are required to run sonar locally:
1. Download nssm.
2. Copy sonar bits from docker container using "docker cp".
3. Use the following command to configure Windows service (named sonard):
```
  nssm install sonard
```
4. Select sonar.exe file in nssm with working directory set to path where it is located.
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
  winrm set winrm/config/service/auth '@{Basic="true"}'
  winrm set winrm/config/client/auth '@{Basic="true"}'
```
For non-production scenarios, WinRm can be configured to use HTTP:
```
  winrm set winrm/config/service '@{AllowUnencrypted="true"}'
```
The WinRM HTTPS listener can be configured to list certificates and set thumbprint for the HTTPS listener:
```
  Get-ChildItem -path cert:\LocalMachine\My\        
  winrm create winrm/config/Listener?Address=*+Transport=HTTPS @{Hostname="<hostname>";CertificateThumbprint="<thumbprint>"}
```
## Troubleshooting
The level of diagnostic logging (LogLevel) is configurable in docker-compose file. This setting can be changed to 'Information' or 'Debug' for displaying low level diagnostics.
