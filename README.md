# Lection Server

Test server for a simple books library.

## Before you start

1. Install .NET 7 SDK from `https://dotnet.microsoft.com/en-us/download/dotnet/7.0`.
2. Install Docker from `https://www.docker.com/`.
3. Install Postman from `https://www.postman.com/downloads/`.

## Running server

1. Open terminal and navigate to the root folder.
2. Run `dotnet publish --os linux --arch x64` to create a Docker image.
3. Run `docker run -d --name lection-server -p 5010:80 codemasters-lection-server:1.1.0` to run a Docker container.
4. Your test server is running on `http://localhost:5010`.

## References

- Navigate to `http://localhost:5010/swagger` to see a documentation for API.
- Use `test@email.ru` and `test-password` as valid credentials.
