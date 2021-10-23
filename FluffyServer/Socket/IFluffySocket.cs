using System;
using System.Net;
using System.Threading.Tasks;

namespace FluffyServer.Socket
{
    public interface IFluffySocket : IDisposable
    {
        void Bind(IPEndPoint endPoint);

        void Listen(int backlog);

        Task<IFluffySocket> Accept();

        int Receive(byte[] buffer);

        int Send(byte[] buffer);
    }
}
