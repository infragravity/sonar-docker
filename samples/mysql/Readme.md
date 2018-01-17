# Monitoring MySQL

## Prerequisutes

* Kubernetes v1.7+
* Prometheus v1.8+

## Building Sonar container with MySql Adapter

1.Modify name of Docker container image in build.sh file.
2.Build container using the following command:
`bash
./build.sh
`
3.Deploy Sonar container image to Docker hub using your account. 

## Deplying Sonar container to Kubernetes

1.Install MySql chart using the following command:

`bash
helm install stable/mysql --name mysql01 --set mysqlRootPassword=Pass@word1
`

2.Clone Sonar charts [repository](http://github.com/infragravity/charts). Install Sonar daemon chart using the following command:

`bash
cd <chart-repository-path>
helm install stable/sonar-ds --name sonar-mysql --set=image.repo=infragravity/sample-mysql,image.tag=latest,config.name=Sonar-mysql.config
`

3. At this point, Prometeus will discover Sonar agent endpont and start reporting metrics using data from MySQL queries.