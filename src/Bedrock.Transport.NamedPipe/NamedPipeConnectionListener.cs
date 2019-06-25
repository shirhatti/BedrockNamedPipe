using Microsoft.AspNetCore.Connections;
using System;
using System.IO.Pipes;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PipeOptions = System.IO.Pipes.PipeOptions;

namespace Bedrock.Transport.NamedPipe
{
    public class NamedPipeConnectionListener : IConnectionListener
    {
        private NamedPipeEndPoint _namedPipeEndPoint;
        private readonly CancellationTokenSource _listeningSource = new CancellationTokenSource();

        public NamedPipeConnectionListener(NamedPipeEndPoint namedPipeEndPoint)
        {
            _namedPipeEndPoint = namedPipeEndPoint;
            ListeningToken = _listeningSource.Token;
        }

        public CancellationToken ListeningToken { get; }
        public EndPoint EndPoint => _namedPipeEndPoint;

        public async ValueTask<ConnectionContext> AcceptAsync(CancellationToken cancellationToken = default)
        {

            if (ListeningToken.IsCancellationRequested)
            {
                // We're done listening
                return null;
            }

            var stream = new NamedPipeServerStream(_namedPipeEndPoint.PipeName,
                                                   PipeDirection.InOut,
                                                   NamedPipeServerStream.MaxAllowedServerInstances,
                                                   PipeTransmissionMode.Byte,
                                                   PipeOptions.WriteThrough);

            CancellationTokenSource source = null;
            CancellationToken effectiveToken = default;
            if (cancellationToken.CanBeCanceled)
            {
                source = CancellationTokenSource.CreateLinkedTokenSource(effectiveToken, ListeningToken);
                effectiveToken = source.Token;
            }
            else
            {
                effectiveToken = ListeningToken;
            }

            try
            {
                using (source)
                {
                    await stream.WaitForConnectionAsync(effectiveToken);
                }
            }
            catch (OperationCanceledException) when (ListeningToken.IsCancellationRequested)
            {
                // Cancelled the current token
                return null;
            }
            catch (ObjectDisposedException)
            {
                // Listener disposed
                return null;
            }

            return new NamedPipeConnectionContext(stream);
        }

        public ValueTask DisposeAsync()
        {
            _listeningSource.Dispose();
            return default;
        }

        public ValueTask UnbindAsync(CancellationToken cancellationToken = default)
        {
            _listeningSource.Cancel();
            return default;
        }
    }
}