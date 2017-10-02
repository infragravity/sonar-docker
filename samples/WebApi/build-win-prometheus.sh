docker -H tcp://10.0.0.21:2375 rmi infragravity/sonar-samples-webapi-prom --force
rm -r out
dotnet publish -c Release -o out -r win10-x64
cp ./Sonard-prometheus.config ./out/Sonard.config 
cp ./Sonard-prometheus.dll.config ./out/Sonard.dll.config
docker -H tcp://10.0.0.21:2375 build -f Dockerfile-win -t infragravity/sonar-samples-webapi-prom .
docker -H tcp://10.0.0.21:2375 run -d --name sonar-samples-webapi-prom -p:8556:8000 -p:8050:5000 --ip 172.24.224.99 infragravity/sonar-samples-webapi-prom
rm -r out
