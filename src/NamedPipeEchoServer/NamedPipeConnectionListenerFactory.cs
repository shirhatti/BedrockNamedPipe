using Microsoft.AspNetCore.Connections;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NamedPipeEchoServer
{
    internal class NamedPipeConnectionListenerFactory : IConnectionListenerFactory
    {
        public ValueTask<IConnectionListener> BindAsync(EndPoint endpoint, CancellationToken cancellationToken = default)
        {
            var namedPipeEndPoint = endpoint as NamedPipeEndPoint;
            if (namedPipeEndPoint == null)
            {
                throw new ArgumentException();
            }

            var transport = new NamedPipeConnectionListener(namedPipeEndPoint);
            transport.Bind();
            return new ValueTask<IConnectionListener>(transport);
        }
    }
}
