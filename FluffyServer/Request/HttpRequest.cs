using System.Collections.Generic;
using System.Collections.Immutable;

namespace FluffyServer.Request
{
    public class HttpRequest : IHttpRequest
    {
        private readonly IImmutableDictionary<string, string> _headers;

        public string Method { get; init; }

        public string Protocol { get; init; }

        public string Route { get; init; }

        public string Hash { get; set; }

        public string Query { get; set; }

        public HttpRequest(IImmutableDictionary<string, string> headers = default)
        {
            var normalizedHeaders = new Dictionary<string, string>();

            if (headers is not null)
            {
                foreach (var pair in headers)
                {
                    normalizedHeaders.Add(Normalize(pair.Key), pair.Value);
                }
            }

            _headers = normalizedHeaders.ToImmutableDictionary();
        }

        public string GetHeaderValueOrDefault(string key)
        {
            return _headers.GetValueOrDefault(Normalize(key));
        }

        public bool HasHeader(string key)
        {
            return _headers.ContainsKey(Normalize(key));
        }

        private static string Normalize(string str)
        {
            var withoutDashes = (str ?? string.Empty).Replace("-", string.Empty);
            return withoutDashes.ToUpperInvariant();
        }
    }
}
