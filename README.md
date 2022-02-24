
# Fashion Like API

Backend for the collaborative project **Fashion Like**, as a result of the initiative **[IDForIdeas](https://idforideas.com/)** and the **#idea1**.

The goal of the project is to create a **social network** about clothes where the users need to sign up in the platform (security) so that they will see posts about clothes designs grouped by tags, besides they can react to the posts.

## Run Locally
It's necessary to install the [.NET Core SDK](https://dotnet.microsoft.com/en-us/download/dotnet/5.0) for be able to run the project.  


Clone the project

```bash
  git clone https://github.com/DanielOy/FashionLike.git
```

Go to the project directory

```bash
  cd FashionLike
```

Install dependencies

```bash
  dotnet restore
```

Start the server

```bash
  dotnet run --project API 
```
### Docker
```bash
  docker run --rm -it -p 5000:80 danny247/fashion-like-backend
```

The API will listening at http://localhost:5000
also you'll can check the swagger documentation at http://localhost:5000/swagger/index.html

## Lessons

This project helps me to practice 
- .NET Core Web api
- EF Core
- Repository pattern
- Specification pattern
- Error handling
- Swagger documentation  
- Docker
- Git


## Contributing

Pull requests, improve ideas and opinions are welcome. So, please share any suggestion about the project.


## License

[MIT](https://choosealicense.com/licenses/mit/)

