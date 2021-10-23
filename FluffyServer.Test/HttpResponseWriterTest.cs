using FluffyServer.Response;
using System.Linq;
using System.Text;
using Xunit;

namespace FluffyServer.Test
{
    public class HttpResponseWriterTest
    {
        [Fact]
        public void WritesHttpResponse()
        {
            // Arrange
            var httpResponseWriter = new HttpResponseWriter();
            var response = new HttpResponse
            {
                StatusCode = 204,
                Body = @"{""code"":204,""description"":""No Content""}"
            };
            response.AddHeader("Server", "FluffyServer");

            // Act
            var responseBytes = httpResponseWriter.Write(response);
            var rawResponseLines = Encoding.UTF8.GetString(responseBytes).Split("\r\n");

            // Assert
            Assert.Equal("HTTP/1.1 204 No Content", rawResponseLines[0]);
            Assert.Equal("Server: FluffyServer", rawResponseLines[1]);
            Assert.Equal("", rawResponseLines[2]);
            Assert.Equal(@"{""code"":204,""description"":""No Content""}", rawResponseLines[3]);
        }

        [Theory]
        [InlineData(200, "OK")]
        [InlineData(201, "Created")]
        [InlineData(204, "No Content")]
        public void MapsStatusCodeToStatusDescription(int statusCode, string expectedDescription)
        {
            // Arrange
            var httpResponseWriter = new HttpResponseWriter();
            var response = new HttpResponse
            {
                StatusCode = statusCode
            };

            // Act
            var responseBytes = httpResponseWriter.Write(response);
            var rawResponseLines = Encoding.UTF8.GetString(responseBytes).Split("\r\n");

            // Assert
            Assert.Equal($"HTTP/1.1 {statusCode} {expectedDescription}", rawResponseLines.First());
        }
        
    }
}
