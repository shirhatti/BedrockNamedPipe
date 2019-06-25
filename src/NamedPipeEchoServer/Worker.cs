using Bedrock.Transport.NamedPipe;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NamedPipeEchoServer
{
    public class Worker : BackgroundService
    {
        private readonly IConnectionListenerFactory _factory;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger, IConnectionListenerFactory factory)
        {
            _factory = factory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var listener = await _factory.BindAsync(new NamedPipeEndPoint("13f10fec-abcf-4f01-be97-2003f870600e"));
            var connection = await listener.AcceptAsync(stoppingToken);

            while (true)
            {
                var result = await connection.Transport.Input.ReadAsync(stoppingToken);
                if (stoppingToken.IsCancellationRequested)
                {
                    connection.Abort();
                    break;
                }

                if (result.IsCompleted)
                {
                    break;
                }

                await connection.Transport.Output.WriteAsync(result.Buffer.ToArray());
                connection.Transport.Input.AdvanceTo(result.Buffer.End);
            }
        }
    }
}
