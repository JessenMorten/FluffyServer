using System.Net;
using System.Threading.Tasks;
using FluffyServer.Router;

namespace FluffyServer
{
    public interface IFluffyServer
    {
        IHttpRouter Router { get; }

        Task Listen(IPEndPoint endPoint);
    }
}
