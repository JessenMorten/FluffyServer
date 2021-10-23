using FluffyServer.Request;
using FluffyServer.Response;
using FluffyServer.Router;
using Xunit;

namespace FluffyServer.Test
{
    public class FluffyServerTest
    {
        [Fact]
        public void ExposesHttpRouter()
        {
            // Arrange
            var requestParser = new HttpRequestParser();
            var responseWriter = new HttpResponseWriter();
            var router = new HttpRouter();
            var server = new FluffyServer(requestParser, responseWriter, router, null);

            // Act
            var httpRouter = server.Router;

            // Assert
            Assert.Equal(router, httpRouter);
        }
    }
}
