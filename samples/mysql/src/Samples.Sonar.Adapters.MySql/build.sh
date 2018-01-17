docker rmi infragravity/sample-mysql:latest --force
rm -r out
#dotnet build /t:Clean,Build
#dotnet publish -c Debug -o out
dotnet publish -c Release -o out
docker build -t infragravity/sample-mysql .
rm -r out
#docker run -it infragravity/sample-mysql --name sonar-samples-mysql