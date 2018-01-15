daemon=tcp://127.0.0.1:2375
image=infragravity/sample-coreserver
container=sample-coreserver
monitoring=prometheus
docker -H $daemon rmi $image --force
#dotnet publish -c Release -o out -r win10-x64
cp ./Sonard-$monitoring.config ./out/Sonard.config 
cp ./Sonard-$monitoring.dll.config ./out/Sonard.dll.config
docker -H $daemon build -f Dockerfile -t $image .
docker -H $daemon run -it -d --name $container -p:8556:8000 -p:8050:5000 $image powershell