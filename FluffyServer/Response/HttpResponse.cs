using System.Collections.Generic;
using System.Collections.Immutable;

namespace FluffyServer.Response
{
    public class HttpResponse : IHttpResponse
    {
        private readonly IDictionary<string, string> _headers = new Dictionary<string, string>();

        public int StatusCode { get; init; }

        public IImmutableDictionary<string, string> Headers => _headers.ToImmutableDictionary();

        public string Body { get; init; }

        public void AddHeader(string key, string value)
        {
            _headers.Add(key, value);
        }
    }
}
