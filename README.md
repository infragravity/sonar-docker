# sonar-docker
This repository contains configuration files for deploying Sonar and its dependencies using Docker. 
Sonar is metric collection agent for gathering data from WMI remotely and can be deployed using Docker.
It sends collected metrics to InfluxDb time series database using UDP transport.

Installation
1. Clone the repository.
2. Review Sonar.cofig file with sample WMI queries in WQL. Change setting to specify Windows host name and user credentials.
3. Configure WinRM to allow remote connections. Only Basic authentication is supported via HTTP or HTTPS.
4. Customize docker-compose.yml file (optional)
5. Create and start containers: 
  >docker-compose up -d

WinRM can be configured to use Basic authentication using the following commands(run as Adminstrator):
  >winrm quickconfig
  >winrm set winrm/config/client/auth '@{Basic="true"}'
  >winrm set winrm/config/service/auth '@{Basic="true"}'
  >winrm set winrm/config/service '@{AllowUnencrypted="true"}'
