rm -r out
dotnet publish -c Release -o out -r win10-x64
docker -H tcp://10.0.0.21:2375 build -f Dockerfile-win -t infragravity/sonar-samples-webapi .
docker -H tcp://10.0.0.21:2375 run -d -it --name sonar-samples-webapi -p:8555:8000  -v C:/Users/Share/config:C:/Config --ip 172.24.224.99 infragravity/sonar-samples-webapi
rm -r out
