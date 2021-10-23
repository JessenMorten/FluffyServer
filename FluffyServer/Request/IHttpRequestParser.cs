namespace FluffyServer.Request
{
    public interface IHttpRequestParser
    {
        IHttpRequest Parse(byte[] buffer);
    }
}
