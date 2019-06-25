using System;
using System.Buffers;
using System.IO.Pipelines;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var namedPipe = new NamedPipeClientStream("13f10fec-abcf-4f01-be97-2003f870600e");
            await namedPipe.ConnectAsync();
            var pipeReader = PipeReader.Create(namedPipe);
            var pipeWriter = PipeWriter.Create(namedPipe);
            while (true)
            {
                var inputLine = Console.ReadLine();
                if (string.IsNullOrEmpty(inputLine))
                {
                    break;
                }
                inputLine = "GET / HTTP/1.0\r\n\r\n";
                var writeResult = await pipeWriter.WriteAsync(Encoding.Default.GetBytes(inputLine));
                if (writeResult.IsCompleted)
                {
                    break;
                }
                var readResult = await pipeReader.ReadAsync();
                if (readResult.IsCompleted)
                {
                    break;
                }
                Console.WriteLine(Encoding.Default.GetString(readResult.Buffer.ToArray()));
                pipeReader.AdvanceTo(readResult.Buffer.End);

            }
        }
    }
}
