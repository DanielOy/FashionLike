rmdir /s /q out
dotnet publish -c Release -o out
docker build -t danny247/fashion-like-backend .
docker push danny247/fashion-like-backend
pause