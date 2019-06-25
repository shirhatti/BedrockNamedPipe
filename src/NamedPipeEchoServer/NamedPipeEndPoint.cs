using System.Net;

namespace NamedPipeEchoServer
{
    internal class NamedPipeEndPoint : EndPoint
    {
        public string PipeName { get; }
        public NamedPipeEndPoint(string pipeName)
        {
            PipeName = pipeName;
        }
    }
}
