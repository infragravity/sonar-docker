# Monitoring Windows Container 

This sample shows how to monitor Windows Containers (Core)on Windows Server (build 17046).

## Goal

Use Prometheus deployed on Kubernetes to scrape metrics from Sonar metric agent endpoint. In this example, Windows container is external to Kubernetes cluster and Prometheus deployed on it.

## Prerequesites

None.

## Setup

1. Modify IP address and port to reference Sonar endpoint on container for Prometheus ( including annotations).
1. Run build script or equivalent commands in PowerShell

```bash
./build-win.sh
```

1. If you deployed Windows container outside of Kubernetes, use below commands to create service with endpoint in K8S to expose metrics:

```bash
kubectl create -f ExternalDockerSvc.yml
kubectl create -f ExternalDockerEndpoint.yml
```

## Cleanup

Run below command to remove service and endpoing from K8S:

```bash
kubectl delete service core-metrics
```