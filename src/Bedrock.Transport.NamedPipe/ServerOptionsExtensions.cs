using Bedrock.Transport.NamedPipe;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.AspNetCore.Server.Kestrel.Core
{
    public static class ServerOptionsExtensions
    {
        public static void ListenNamedPipe(this KestrelServerOptions serverOptions, NamedPipeEndPoint endPoint)
        {
            var listenOptions = (ListenOptions)FormatterServices.GetUninitializedObject(typeof(ListenOptions));
            var endPointProperty = listenOptions.GetType().GetProperty("EndPoint");
            endPointProperty.SetValue(listenOptions, endPoint);
            ((List<ListenOptions>)serverOptions.GetType().GetProperty("ListenOptions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(serverOptions)).Add(listenOptions);
            return;
        }
    }
}
