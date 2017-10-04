Sonar is metric collection agent wrtten in .NET Core for gathering data from WMI locally or remotely(using WS-Management protocol) integrated with time series databases. This repository contains configuration files for deploying Sonar using Docker. Helm chart for Kubernetes is coming soon:) 

# Background
There are few choices for collecting performance metrics from Windows. While there are choices to collect performance counters or generate code for specific WMI metrics available, Sonar is offers configuratin-only approach with supporting multiple types of TSDBs at the same time.   

# Overview
Sonar collects metrics from WMI queries and sends them to time series databases. It can simultaneously be integrated following TDSDBs:
  * InfluxDb - using UDP protocol.
  * Prometeus - as HTTP exporter scrape endpoint. 
See WebAPi sample that shows how to use both of the above scenarios.

## Deployment Scenarios
Sonar can be deployed as:
  * Windows service
  * Docker container
  * Kubernetes pod 
Metric collection from WMI has been tested with multiple Windows versions, including Win7(SP1) and Nano Server(TP5).
## Documentation
Docs are available at [knowledge base](http://www.infragravity.com/knowledge-base/sonar-overview/) 

# Installation
The below steps describe how to use Docker to create and deploy simple environment with Sonar, InfluxDb. The purpose of Telegraf in this case is to monitor performance for Docker containers.
## Docker  
1. Clone the repository.
2. Review Sonar WebAPI sample configuration files and WMI queries in WQL. 
3. Deploy containers.
## Windows Service
The following steps are required to run sonar (sonard stands for sonar daemon) locally:
1. Download sonar bits and modify configuration files as shown in the examples. 
2. Use the following command to configure Windows service (named sonard):
```
  sc.exe create sonard binpath= <path>\Sonard.exe start= auto obj= LocalSystem depend= "WinRM"
```
3. Start sonar daemon as windows service.
## Configuration
Open InfluxDb administration web interface and create new database for storing collected metrics:
```
  create database sonar
```
The configuration in the samples uses UDP configuration to listen on port 8092 for receiving events from Sonar container and storing them in database named 'sonar'. 
## Configuring WinRM
WinRM can be configured to use Basic authentication using the following commands(run as Adminstrator):
```
  winrm quickconfig
  winrm set winrm/config/service/auth '@{Basic="true"}'
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
The level of diagnostic logging can be configured for Sonar. This setting can be changed to 'Information' or 'Debug' for displaying low level diagnostics.
