Sonar is metric collection agent implemented using .NET Core for gathering data from WMI locally or remotely(using WS-Management protocol) integrated with time series databases. This repository contains configuration files for deploying Sonar using Docker. Helm chart for Kubernetes is coming soon:) 

# Background
While there are few choices avalable for collecting performance counters or generating exporter code for specific WMI metrics, Sonar offers configuration only approach with support for multiple types of TSDBs used at the same time. Sonar supports sending metrics to Prometheus and InfluxDb. Using InfluxDb is ideal for storing events collected by Sonar from Windows OS event logs.  

# Overview
Sonar collects metrics from WMI queries and sends them to time series databases. Metric collection from WMI has been tested with multiple Windows versions, including Win7(SP1) and Nano Server(TP5). It can simultaneously be integrated following TDSDBs:
  * InfluxDb - using UDP protocol.
  * Prometeus - as HTTP exporter scrape endpoint. 
## Deployment Scenarios
Sonar can be deployed as agent or sidecar(for monitoring multiple Windows containers on the same host):
  * Windows service
  * Docker container
  * Kubernetes pod 
## Documentation
Docs are available at [knowledge base](http://www.infragravity.com/knowledge-base/sonar-overview/)
## Samples
The WebAPi sample shows how to use Prometheus or InfluxDb to monitor Windows container (Nano Server) with IIS.
# Installation
The below steps describe how to use Docker to create and deploy simple environment with Sonar, InfluxDb. Telegraf is optional to monitor performance metrics privided by Docker.
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
# Configuration
## Metrics
### Queries
Sonar can be configured to run WMI query for any metric without code changes. Below example shows query for Windows processes:
```
   <Queries>
     <add name="Win32_Process"
             filter="select * from Win32_Process"
             resource="http://schemas.microsoft.com/wbem/wsman/1/wmi/root/cimv2/*" namespace="root\cimv2">
            <Tags>
                <add name="Name" value = "Name"/>
            </Tags>
            <Values>
                 <add name="ThreadCount" value="CimType.UInt32"/>
                 <add name="HandleCount" value="CimType.UInt32"/>
                 <add name="VirtualSize" value="CimType.UInt64"/>
                 <add name="WorkingSetSize" value="CimType.UInt64"/>
            </Values>
        </add>
   </Queries>
```
For event logs, you can use additional expressions. In the below example, WMI query scans Windows event log entries made in past 30 seconds:
```
   <Queries>
     <add name="EventLog_System"
            filter="select TimeGenerated,Message,EventCode,ComputerName,SourceName,EventType from Win32_NTLogEvent where TimeGenerated > timeshift(30s) and LogFile='System' and EventType!=0"
            resource="http://schemas.microsoft.com/wbem/wsman/1/wmi/root/cimv2/*" namespace="root\cimv2" 
            timestamp="TimeGenerated"> 
            <Tags>
                <add name="ComputerName" value = "ComputerName"/>
                <add name="SourceName" value = "SourceName"/>
            </Tags>
            <Values>
                <add name="EventCode" value="CimType.UInt16" />
                <add name="EventType" value="CimType.UInt8" />
            </Values>
        </add>
   </Queries>
```
### Servers
Sonar allows configuring one or more endpoints for running queries. Below is simple example:
```
   <Servers>
    <add name="webapi-prom" url="http://127.0.0.1:5985/wsman" username="" password="" timeoutMilliseconds="1000" authType="Negotiate"/>
   </Servers>  
```
### Schedules
Each schedule maps query to a server and specifies polling interval:
```
   <Schedules>
    <add name="localProcesses" query="Win32_Process" server="webapi-prom"  intervalSeconds="10" />
   </Schedules>
```
For more information about configuring schedules and servers, see samples in this repository and our [knowledge base](http://www.infragravity.com/knowledge-base/sonar-overview/).
## Integration
### InfluxDb
To store metrics in InfluxDb, open CLI and create new database for storing collected metrics:
```
  create database sonar
```
Review configuration in the WebAPI sample. It uses uses UDP configuration to listen on port 8092 for receiving events from Sonar container and storing them in database named 'sonar'.
### Prometheus
Create or update Prometheus job by adding new scrape endpoint hosted by Sonar. By default, Sonar uses configuration to determine port for http://host:port/metrics
## WinRM
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
