# Overview
This directory contains set of samples for using Sonar WMI exporter for monitoring on Docker and Kubernets:
  * The WebAPI - shows how to monitor .NET core web application deployed on Windows Nano Server container that runs IIS.

## Deployment Scenarios
Sonar can be deployed as Windows service on container or host. The WebAPI examples how it can be accomplished using Windows Nano Server container image. 
## Documentation
The docs are available at [knowledge base](http://www.infragravity.com/knowledge-base/sonar-overview/)
# Installation
The below steps describe how to use Docker to create and deploy simple Web API with Sonar to monitor Nano Server container metrics, including IIS and send them via UDP to InfluxDb time series database.
## Docker  
1. Clone the repository.
2. Review Sonard.config files with sample WMI queries in WQL. Change InfluxDb host and port if necessary. Note that domain name is not required for the basic authentication.
3. Install InfluxDB for storing time series on a host or as Docker container.
4. Review .NET Core project files and run build-win.sh or equivalent steps on Windows. Alternatively, you can use copy Sonar files and Dockerfile to another project.
5. Visualize metrics stored in InfluxDb using Grafana. The sample dashboard "sonar-influxdb-mswin-iis.json" is included in dashboards folder of this project.

4. Select sonar.exe file in nssm with working directory set to path where it is located.
## Configuration
Open InfluxDb administration web interface and create new database for storing collected metrics:
```
  create database sonar
```
The WebApi sample assumes that InfluxDb is configured with UDP listener. 
``` 
  [[udp]]
  enabled = true
  bind-address = ":8092"
  database = "sonar"
  retention-policy = ""
  batch-size = 5000
  batch-pending = 10
  read-buffer = 0
  batch-timeout = "1s"
  precision = ""
```
Thee above configuration snippet from influxdb.conf includes UDP configuration to listen on port 8092 for receiving events from Sonar container and storing them in database named 'sonar'. 
## Troubleshooting
The level of diagnostic logging (LogLevel) is configurable in Sonard.dll.config file. This setting can be changed to for displaying low level diagnostics.
