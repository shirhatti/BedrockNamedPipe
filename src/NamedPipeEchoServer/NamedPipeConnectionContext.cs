using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;

namespace NamedPipeEchoServer
{
    internal class NamedPipeConnectionContext : ConnectionContext
    {
        private IDuplexPipe _transport;

        public NamedPipeConnectionContext(IDuplexPipe duplexPipe)
        {
            _transport = duplexPipe;
        }

        public override string ConnectionId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IFeatureCollection Features => new FeatureCollection();

        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IDuplexPipe Transport { get => _transport; set => _transport = value; }
    }
}
