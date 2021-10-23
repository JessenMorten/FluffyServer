using System.Text;

namespace FluffyServer.Response
{
    public class HttpResponseWriter : IHttpResponseWriter
    {
        public byte[] Write(IHttpResponse httpResponse)
        {
            // Create string builder
            var stringBuilder = new StringBuilder();

            // Add status line
            stringBuilder.AppendLine($"HTTP/1.1 {httpResponse.StatusCode} {GetStatusDescription(httpResponse)}");

            // Add headers
            foreach (var header in httpResponse.Headers)
            {
                stringBuilder.AppendLine($"{header.Key}: {header.Value}");
            }

            // Add payload
            if (httpResponse.Body is not null)
            {
                stringBuilder
                    .AppendLine(string.Empty)
                    .AppendLine(httpResponse.Body);
            }

            var raw = stringBuilder.ToString();

            return Encoding.UTF8.GetBytes(raw);
        }

        private static string GetStatusDescription(IHttpResponse httpResponse)
        {
            return httpResponse.StatusCode switch
            {
                200 => "OK",
                201 => "Created",
                204 => "No Content",
                _ => "Unknown"
            };
        }
    }
}
