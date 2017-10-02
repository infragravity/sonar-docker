#build for using InfluxDb
docker -H tcp://10.0.0.21:2375 rmi infragravity/sonar-samples-webapi-influx --force
rm -r out
dotnet publish -c Release -o out -r win10-x64
cp ./Sonard-influxdb.config ./out/Sonard.config 
cp ./Sonard-influxdb.dll.config ./out/Sonard.dll.config
docker -H tcp://10.0.0.21:2375 build -f Dockerfile-win -t infragravity/sonar-samples-webapi-influx .
docker -H tcp://10.0.0.21:2375 run -d --name sonar-samples-webapi-influx -p:8555:8000 --ip 172.24.224.98 infragravity/sonar-samples-webapi-influx
rm -r out
