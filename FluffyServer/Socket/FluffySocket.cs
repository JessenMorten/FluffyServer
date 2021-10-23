using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FluffyServer.Socket
{
    public class FluffySocket : IFluffySocket
    {
        private readonly Lazy<System.Net.Sockets.Socket> _socket;

        public FluffySocket()
        {
            _socket = new Lazy<System.Net.Sockets.Socket>(() =>
            {
                var addressFamily = AddressFamily.InterNetwork;
                var socketType = SocketType.Stream;
                var protocolType = ProtocolType.Tcp;
                return new System.Net.Sockets.Socket(addressFamily, socketType, protocolType);
            });
        }

        private FluffySocket(System.Net.Sockets.Socket socket)
        {
            _socket = new Lazy<System.Net.Sockets.Socket>(() => socket);
        }

        public void Bind(IPEndPoint endPoint)
        {
            _socket.Value.Bind(endPoint);
        }

        public void Listen(int backlog)
        {
            _socket.Value.Listen(backlog);
        }

        public async Task<IFluffySocket> Accept()
        {
            var socket = await _socket.Value.AcceptAsync();
            return new FluffySocket(socket);
        }

        public int Receive(byte[] buffer)
        {
            return _socket.Value.Receive(buffer);
        }

        public int Send(byte[] buffer)
        {
            return _socket.Value.Send(buffer);
        }

        public void Dispose()
        {
            _socket.Value.Dispose();
        }
    }
}
