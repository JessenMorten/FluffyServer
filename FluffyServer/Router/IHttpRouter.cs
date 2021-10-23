using FluffyServer.Request;
using FluffyServer.Response;
using System;
using System.Threading.Tasks;

namespace FluffyServer.Router
{
    public interface IHttpRouter
    {
        Func<IHttpRequest, Task<IHttpResponse>> GetHandlerOrDefault(IHttpRequest httpRequest);

        void On(string method, string template, Func<IHttpRequest, Task<IHttpResponse>> requestHandler);

        void On<P1>(string method, string template, Func<IHttpRequest, P1, Task<IHttpResponse>> requestHandler);

        void On<P1, P2>(string method, string template, Func<IHttpRequest, P1, P2, Task<IHttpResponse>> requestHandler);

        void On<P1, P2, P3>(string method, string template, Func<IHttpRequest, P1, P2, P3, Task<IHttpResponse>> requestHandler);
    }
}
