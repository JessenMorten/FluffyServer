namespace FluffyServer.Response
{
    public interface IHttpResponseWriter
    {
        byte[] Write(IHttpResponse httpResponse);
    }
}
