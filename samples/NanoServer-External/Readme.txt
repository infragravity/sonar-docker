This sample shows how to monitor Windows Containers on Nano Server 2016 (build 14393).
## Goal
Use Prometheus deployed on Kubernetes to scrape metrics from Sonar metric agent endpoint. In this example, Windows container is external to Kubernetes cluster and Prometheus deployed on it.
## Setup
    1. Modify IP address and port to reference Sonar endpoint for Prometheus ( including annotations) 
    2. Run below commands to create service with endpoint in K8S
```
kubectl create -f ExternalDockerSvc.yml 
kubectl create -f ExternalDockerEndpoint.yml
```

## Cleanup
Run below command to remove service and endpoing from K8S:
```
kubectl delete service nano-metrics 
```