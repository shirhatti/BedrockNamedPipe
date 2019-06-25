using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Bedrock.Transport.NamedPipe
{
    public class NamedPipeConnectionListenerFactory : IConnectionListenerFactory
    {
        public NamedPipeConnectionListenerFactory(
            IOptions<NamedPipeTransportOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }
        public ValueTask<IConnectionListener> BindAsync(EndPoint endpoint, CancellationToken cancellationToken = default)
        {
            var namedPipeEndPoint = endpoint as NamedPipeEndPoint;
            if (namedPipeEndPoint == null)
            {
                namedPipeEndPoint = new NamedPipeEndPoint("13f10fec-abcf-4f01-be97-2003f870600e");
            }

            var transport = new NamedPipeConnectionListener(namedPipeEndPoint);
            return new ValueTask<IConnectionListener>(transport);
        }
    }
}
