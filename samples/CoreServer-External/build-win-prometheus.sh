daemon=tcp://127.0.0.1:2375
docker -H $daemon rmi infragravity/sonar-samples-webapi-prom --force
#dotnet publish -c Release -o out -r win10-x64
cp ./Sonard-prometheus.config ./out/Sonard.config 
cp ./Sonard-prometheus.dll.config ./out/Sonard.dll.config
docker -H $daemon build -f Dockerfile -t infragravity/sonar-samples-webapi-prom .
docker -H $daemon run -it -d --name sonar-samples-webapi-prom -p:8556:8000 -p:8050:5000 infragravity/sonar-samples-webapi-prom powershell