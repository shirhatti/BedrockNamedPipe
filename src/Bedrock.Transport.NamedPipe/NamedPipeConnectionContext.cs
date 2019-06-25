using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;

namespace Bedrock.Transport.NamedPipe
{
    internal class NamedPipeConnectionContext : ConnectionContext
    {
        private IDuplexPipe _transport;
        private string _connectionId;
        public NamedPipeConnectionContext(Stream stream)
        {
            var pipeReader = PipeReader.Create(stream);
            var pipeWriter = PipeWriter.Create(stream);
            var duplexPipe = new DuplexPipe(pipeReader, pipeWriter);
            _transport = duplexPipe;
        }

        public override string ConnectionId { get => _connectionId; set => _connectionId = value; }

        public override IFeatureCollection Features => new FeatureCollection();

        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IDuplexPipe Transport { get => _transport; set => _transport = value; }
    }
}
