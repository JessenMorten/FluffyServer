using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluffyServer.Request;
using FluffyServer.Response;
using FluffyServer.Router;
using FluffyServer.Socket;

namespace FluffyServer
{
    public class FluffyServer : IFluffyServer
    {
        private readonly IHttpRequestParser _requestParser;

        private readonly IHttpResponseWriter _responseWriter;

        private readonly IHttpRouter _router;

        private readonly IFluffySocketFactory _socketFactory;

        public IHttpRouter Router => _router;

        public FluffyServer(
            IHttpRequestParser requestParser,
            IHttpResponseWriter responseWriter,
            IHttpRouter router,
            IFluffySocketFactory socketFactory)
        {
            _requestParser = requestParser;
            _responseWriter = responseWriter;
            _router = router;
            _socketFactory = socketFactory;
        }

        public async Task Listen(IPEndPoint endPoint)
        {
            var listeningSocket = _socketFactory.Create();

            listeningSocket.Bind(endPoint);
            listeningSocket.Listen(100);

            while (true)
            {
                var newSocket = await listeningSocket.Accept();
                _ = HandleNewSocket(newSocket);
            }

            listeningSocket.Dispose();
        }

        private async Task<byte[]> HandleIncomingBytes(byte[] bytes)
        {
            byte[] result = null;

            try
            {
                var request = _requestParser.Parse(bytes);

                var handler = _router.GetHandlerOrDefault(request);

                if (handler is not null)
                {
                    var response = await handler.Invoke(request);

                    if (response is not null)
                    {
                        result = _responseWriter.Write(response);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result ?? Array.Empty<byte>();
        }

        private async Task HandleNewSocket(IFluffySocket socket)
        {
            try
            {
                var bytes = ReadBytes(socket);
                var responseBytes = await HandleIncomingBytes(bytes);
                if (responseBytes is not null && responseBytes.Any())
                {
                    WriteBytes(socket, responseBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Dispose();
            }
        }

        private void WriteBytes(IFluffySocket socket, byte[] bytes)
        {
            var bytesSent = socket.Send(bytes);
            Console.WriteLine($"Sent {bytesSent} of {bytes.Length} byte(s)");
        }

        private byte[] ReadBytes(IFluffySocket socket)
        {
            var bufferSize = 512;
            var data = new List<byte>(bufferSize);

            while (true)
            {
                // Read into buffer
                var buffer = new byte[bufferSize];
                var receivedBytes = socket.Receive(buffer);

                // Break if zero bytes read
                if (receivedBytes == 0)
                {
                    break;
                }

                // Add bytes
                var eofIndex = Array.IndexOf<byte>(buffer, 0);
                var bytes = eofIndex > 0 ? buffer[0..eofIndex] : buffer;
                data.AddRange(bytes);

                // Break if null byte found
                if (eofIndex > 0)
                {
                    break;
                }
            }

            return data.ToArray();
        }
    }
}
