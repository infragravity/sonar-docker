## Setup
    1. Modify IP address and port to reference Sonar endpoint for Prometheus ( including annotations) 
    2. Run below commands to create service with endpoint in K8S
```
kubectl create -f ExternalBtsSvc.yml 
kubectl create -f ExternalBtsEndpoint.yml
```

## Cleanup
Run below command to remove service and endpoing from K8S:
```
kubectl delete service nano-metrics 
```