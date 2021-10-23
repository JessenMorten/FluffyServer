using FluffyServer.Request;
using FluffyServer.Response;
using FluffyServer.Router;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluffyServer.Test
{
    public class HttpRouterTest
    {
        [Fact]
        public void AddsAndRetreivesHandler()
        {
            // Arrange
            var router = new HttpRouter();
            IHttpResponse response = new HttpResponse();
            Task<IHttpResponse> RequestHandler(IHttpRequest request) => Task.FromResult(response);
            var request = new HttpRequest()
            {
                Method = "GET",
                Route = "/index"
            };

            // Act
            router.On("GET", "/index", RequestHandler);
            var handler = router.GetHandlerOrDefault(request);

            // Assert
            Assert.NotNull(handler);
            Assert.Equal(RequestHandler, handler);
        }

        [Fact]
        public async Task MapsRequestToTemplateWithOneParameter()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var router = new HttpRouter();
            var request = new HttpRequest()
            {
                Method = "GET",
                Route = $"/user/{userGuid}"
            };
            var mappedGuid = Guid.Empty;

            // Act
            router.On<Guid>("GET", "/user/{id}", (request, id) =>
            {
                mappedGuid = id;
                return Task.FromResult(new HttpResponse() as IHttpResponse);
            });
            _ = await router.GetHandlerOrDefault(request).Invoke(request);

            // Assert
            Assert.Equal(userGuid, mappedGuid);
        }

        [Fact]
        public async Task MapsRequestToTemplateWithTwoParameters()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var userAge = 42;
            var router = new HttpRouter();
            var request = new HttpRequest()
            {
                Method = "POST",
                Route = $"/user/{userGuid}/age/{userAge}"
            };
            var mappedGuid = Guid.Empty;
            var mappedAge = -1;

            // Act
            router.On<Guid, int>("POST", "/user/{id}/age/{age}", (request, id, age) =>
            {
                mappedGuid = id;
                mappedAge = age;
                return Task.FromResult(new HttpResponse() as IHttpResponse);
            });
            _ = await router.GetHandlerOrDefault(request).Invoke(request);

            // Assert
            Assert.Equal(userGuid, mappedGuid);
            Assert.Equal(userAge, mappedAge);
        }

        [Fact]
        public async Task MapsRequestToTemplateWithThreeParameters()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var userAge = 42;
            var userName = "John";
            var router = new HttpRouter();
            var request = new HttpRequest()
            {
                Method = "PUT",
                Route = $"/user/{userGuid}/age/{userAge}/name/{userName}"
            };
            var mappedGuid = Guid.Empty;
            var mappedAge = -1;
            var mappedName = string.Empty;

            // Act
            router.On<Guid, int, string>("PUT", "/user/{id}/age/{age}/name/{name}", (request, id, age, name) =>
            {
                mappedGuid = id;
                mappedAge = age;
                mappedName = name;
                return Task.FromResult(new HttpResponse() as IHttpResponse);
            });
            _ = await router.GetHandlerOrDefault(request).Invoke(request);

            // Assert
            Assert.Equal(userGuid, mappedGuid);
            Assert.Equal(userAge, mappedAge);
            Assert.Equal(userName, mappedName);
        }
    }
}
