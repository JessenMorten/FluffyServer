using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyServer.Request
{
    public class HttpRequestParser : IHttpRequestParser
    {
        private readonly Encoding _encoding = Encoding.UTF8;

        public IHttpRequest Parse(byte[] buffer)
        {
            var raw = _encoding.GetString(buffer);

            var lines = raw.Split("\r\n");

            var headers = lines
                .Skip(1)
                .Where(line => line.Split(":").Count() == 2)
                .Select(line =>
                {
                    var parts = line.Split(":");
                    return (Key: parts.First(), Value: parts.Last());
                });

            var dic = new Dictionary<string, string>();
            foreach (var h in headers)
            {
                dic.Add(h.Key.Trim(), h.Value.Trim());
            }

            var route = lines.First().Split(" ").ElementAt(1);

            var hash = string.Empty;
            var hashIndex = route.IndexOf("#");
            if (hashIndex > 0)
            {
                hash = route.Substring(hashIndex);
                route = route.Replace(hash, string.Empty);
            }

            var query = string.Empty;
            var queryIndex = route.IndexOf("?");
            if (queryIndex > 0)
            {
                query = route.Substring(queryIndex);
                route = route.Replace(query, string.Empty);
            }

            return new HttpRequest(dic.ToImmutableDictionary())
            {
                Method = lines.First().Split(" ").First(),
                Route = route,
                Protocol = lines.First().Split(" ").Last(),
                Hash = hash,
                Query = query
            };
        }
    }
}
