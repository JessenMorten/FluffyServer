using FluffyServer.Request;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace FluffyServer.Test
{
    public class HttpRequestTest
    {
        [Fact]
        public void HeaderKeysAreNormalizedWhenGettingHeaderValue()
        {
            // Arrange
            var headers = new Dictionary<string, string>
            {
                { "User-Agent", "Unit test" }
            };
            var request = new HttpRequest(headers.ToImmutableDictionary());

            // Act
            var userAgent = request.GetHeaderValueOrDefault("useragent");

            // Assert
            Assert.Equal("Unit test", userAgent);
        }

        [Fact]
        public void HeaderKeysAreNormalizedWhenCheckingIfHeaderValueExist()
        {
            // Arrange
            var headers = new Dictionary<string, string>
            {
                { "User-Agent", "Unit test" }
            };
            var request = new HttpRequest(headers.ToImmutableDictionary());

            // Act
            var hasHeader = request.HasHeader("useragent");

            // Assert
            Assert.True(hasHeader);
        }

        [Fact]
        public void ClassHandlesNullValues()
        {
            // Arrange
            IImmutableDictionary<string, string> headers = null;
            string headerKey = null;

            // Act
            var request = new HttpRequest(headers);
            var hasHeader = request.HasHeader(headerKey);
            var headerValue = request.GetHeaderValueOrDefault(headerKey);

            // Assert
            Assert.Null(headerValue);
            Assert.False(hasHeader);
        }

        [Fact]
        public void PropertiesWork()
        {
            // Arrange
            IImmutableDictionary<string, string> headers = null;
            var route = Guid.NewGuid().ToString();
            var hash = Guid.NewGuid().ToString();
            var method = Guid.NewGuid().ToString();
            var protocol = Guid.NewGuid().ToString();
            var query = Guid.NewGuid().ToString();

            // Act
            var request = new HttpRequest(headers)
            {
                Route = route,
                Hash = hash,
                Method = method,
                Protocol = protocol,
                Query = query
            };

            // Assert
            Assert.Equal(route, request.Route);
            Assert.Equal(hash, request.Hash);
            Assert.Equal(method, request.Method);
            Assert.Equal(protocol, request.Protocol);
            Assert.Equal(query, request.Query);
        }
    }
}
