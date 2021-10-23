using FluffyServer.Response;
using System.Collections.Generic;
using Xunit;

namespace FluffyServer.Test
{
    public class HttpResponseTest
    {
        [Fact]
        public void AddsHeader()
        {
            // Arrange
            var response = new HttpResponse();
            var newHeader = new KeyValuePair<string, string>("User-Agent", "Unit test");

            // Act
            response.AddHeader(newHeader.Key, newHeader.Value);

            // Assert
            Assert.True(response.Headers.Contains(newHeader));
        }

        [Fact]
        public void PropertiesWork()
        {
            // Arrange
            var statusCode = 501;
            var body = "<h1>Hello World</h1>";

            // Act
            var response = new HttpResponse
            {
                StatusCode = statusCode,
                Body = body
            };

            // Assert
            Assert.Equal(statusCode, response.StatusCode);
            Assert.Equal(body, response.Body);
        }
    }
}
