using FluffyServer.Request;
using System.Text;
using Xunit;

namespace FluffyServer.Test
{
    public class HttpRequestParserTest
    {
        [Fact]
        public void ParsesHeaders()
        {
            // Arrange
            var httpRequestParser = new HttpRequestParser();
            var bytes = Encoding.UTF8.GetBytes(new StringBuilder()
                .AppendLine("GET / HTTP/1.1")
                .AppendLine("User-Agent: Unit test")
                .AppendLine("Language: en")
                .ToString());

            // Act
            var httpRequest = httpRequestParser.Parse(bytes);
            var userAgent = httpRequest.GetHeaderValueOrDefault("User-Agent");
            var language = httpRequest.GetHeaderValueOrDefault("Language");

            // Assert
            Assert.Equal("Unit test", userAgent);
            Assert.Equal("en", language);
        }

        [Fact]
        public void ParsesProtocol()
        {
            // Arrange
            var httpRequestParser = new HttpRequestParser();
            var bytes = Encoding.UTF8.GetBytes(new StringBuilder()
                .AppendLine("GET / HTTP/1.1")
                .ToString());

            // Act
            var httpRequest = httpRequestParser.Parse(bytes);
            var protocol = httpRequest.Protocol;

            // Assert
            Assert.Equal("HTTP/1.1", protocol);
        }

        [Fact]
        public void ParsesMethod()
        {
            // Arrange
            var httpRequestParser = new HttpRequestParser();
            var bytes = Encoding.UTF8.GetBytes(new StringBuilder()
                .AppendLine("GET / HTTP/1.1")
                .ToString());

            // Act
            var httpRequest = httpRequestParser.Parse(bytes);
            var method = httpRequest.Method;

            // Assert
            Assert.Equal("GET", method);
        }

        [Fact]
        public void ParsesRoute()
        {
            // Arrange
            var httpRequestParser = new HttpRequestParser();
            var bytes = Encoding.UTF8.GetBytes(new StringBuilder()
                .AppendLine("GET /home/search?q=samplequery#test HTTP/1.1")
                .ToString());

            // Act
            var httpRequest = httpRequestParser.Parse(bytes);
            var route = httpRequest.Route;

            // Assert
            Assert.Equal("/home/search", route);
        }

        [Fact]
        public void ParsesQuery()
        {
            // Arrange
            var httpRequestParser = new HttpRequestParser();
            var bytes = Encoding.UTF8.GetBytes(new StringBuilder()
                .AppendLine("GET /home/search?q=samplequery#test HTTP/1.1")
                .ToString());

            // Act
            var httpRequest = httpRequestParser.Parse(bytes);
            var query = httpRequest.Query;

            // Assert
            Assert.Equal("?q=samplequery", query);
        }

        [Fact]
        public void ParsesHash()
        {
            // Arrange
            var httpRequestParser = new HttpRequestParser();
            var bytes = Encoding.UTF8.GetBytes(new StringBuilder()
                .AppendLine("GET /home/search?q=samplequery#test HTTP/1.1")
                .ToString());

            // Act
            var httpRequest = httpRequestParser.Parse(bytes);
            var hash = httpRequest.Hash;

            // Assert
            Assert.Equal("#test", hash);
        }
    }
}
