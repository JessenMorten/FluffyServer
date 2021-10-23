namespace FluffyServer.Request
{
    public interface IHttpRequest
    {
        public string Method { get; }

        public string Protocol { get; }

        public string Route { get; }

        public string Hash { get; }

        public string Query { get; }

        public bool HasHeader(string key);

        public string GetHeaderValueOrDefault(string key);
    }
}
