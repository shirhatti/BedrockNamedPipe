using Microsoft.AspNetCore.Connections;
using System;
using System.IO.Pipelines;
using System.IO.Pipes;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PipeOptions = System.IO.Pipes.PipeOptions;

namespace NamedPipeEchoServer
{
    internal class NamedPipeConnectionListener : IConnectionListener
    {
        private NamedPipeEndPoint _namedPipeEndPoint;
        private NamedPipeServerStream _namedPipe;

        public NamedPipeConnectionListener(NamedPipeEndPoint namedPipeEndPoint)
        {
            _namedPipeEndPoint = namedPipeEndPoint;
            _namedPipe = new NamedPipeServerStream(_namedPipeEndPoint.PipeName,
                                           PipeDirection.InOut,
                                           1,
                                           PipeTransmissionMode.Byte,
                                           PipeOptions.WriteThrough);
        }

        public EndPoint EndPoint => _namedPipeEndPoint;

        public async ValueTask<ConnectionContext> AcceptAsync(CancellationToken cancellationToken = default)
        {
            await _namedPipe.WaitForConnectionAsync(cancellationToken);
            var pipeReader = PipeReader.Create(_namedPipe);
            var pipeWriter = PipeWriter.Create(_namedPipe);
            var duplexPipe = new DuplexPipe(pipeReader, pipeWriter);
            return new NamedPipeConnectionContext(duplexPipe);
        }

        public async ValueTask DisposeAsync()
        {
            await _namedPipe.DisposeAsync();
        }

        public ValueTask UnbindAsync(CancellationToken cancellationToken = default)
        {
            _namedPipe.Disconnect();
            return default;
        }

        internal void Bind()
        {
            return;
        }
    }
}