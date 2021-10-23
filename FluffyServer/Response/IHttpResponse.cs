using System.Collections.Immutable;

namespace FluffyServer.Response
{
    public interface IHttpResponse
    {
        public int StatusCode { get; }

        public IImmutableDictionary<string, string> Headers { get; }

        public string Body { get; }
    }
}
