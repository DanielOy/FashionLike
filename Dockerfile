FROM mcr.microsoft.com/dotnet/aspnet:5.0

COPY out/ App/

WORKDIR /App

EXPOSE 80

ENTRYPOINT ["dotnet", "API.dll"]

#dotnet publish -c Release -o out
#docker build -t danny247/fashion-like-backend .