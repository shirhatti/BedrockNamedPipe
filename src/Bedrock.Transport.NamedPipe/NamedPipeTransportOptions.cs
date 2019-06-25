using System.IO.Pipes;

namespace Bedrock.Transport.NamedPipe
{
    public class NamedPipeTransportOptions
    {
        public PipeOptions PipeOptions { get; set; }
        public int MaxNumberOfServerInstances { get; set; }
    }
}