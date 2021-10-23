namespace FluffyServer.Socket
{
    public class FluffySocketFactory : IFluffySocketFactory
    {
        public IFluffySocket Create()
        {
            return new FluffySocket();
        }
    }
}
