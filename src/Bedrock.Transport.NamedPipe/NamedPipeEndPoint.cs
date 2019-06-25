using System.Net;
using System.Net.Sockets;

namespace Bedrock.Transport.NamedPipe
{
    public class NamedPipeEndPoint : EndPoint
    {
        public string PipeName { get; }
        public NamedPipeEndPoint(string pipeName)
        {

            PipeName = pipeName;
        }
        public override AddressFamily AddressFamily => AddressFamily.Unknown;
    }
}
