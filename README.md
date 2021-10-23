# FluffyServer

A Lightweight HTTP server, written in C# for fun.

[![master_build](https://github.com/JessenMorten/FluffyServer/actions/workflows/master_build.yml/badge.svg)](https://github.com/JessenMorten/FluffyServer/actions/workflows/master_build.yml)

## Usage example

```csharp
// Create server
var requestParser = new HttpRequestParser();
var responseWriter = new HttpResponseWriter();
var router = new HttpRouter();
var socketFactory = new FluffySocketFactory();
var server = new FluffyServer.FluffyServer(requestParser, responseWriter, router, socketFactory);

// Add handlers
server.Router.On("GET", "/", request =>
{
    var userAgent = request.GetHeaderValueOrDefault("useragent");

    return Task.FromResult(new HttpResponse
    {
        StatusCode = 200,
        Body = $"Hello, your user agent is {userAgent ?? "Unknown"}!"
    } as IHttpResponse);
});

server.Router.On<string, string>("POST", "/greet/{firstname}/{lastname}", (request, firstname, lastname) =>
{
    return Task.FromResult(new HttpResponse
    {
        StatusCode = 200,
        Body = $"Hello {firstname} {lastname}!"
    } as IHttpResponse);
});

// Start server
await server.Listen(IPEndPoint.Parse("127.0.0.1:80"));
```
