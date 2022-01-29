using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;

namespace WebApiCppLib.Hubs
{
    public class ProxyHub : Hub
    {
        public async Task<long> CalculateProxy (long a)
        {
            Console.WriteLine($"Web API - new request - process ID: {Process.GetCurrentProcess().Id}");

            var url = new Uri("http://signalrcpplibservice/CppLibHub");

            var connection = new HubConnectionBuilder()
                    .WithUrl(url, option =>
                    {
                        option.SkipNegotiation = true;
                        option.Transports = HttpTransportType.WebSockets;
                    })
                    .Build();

            await connection.StartAsync();

            var result = await connection.InvokeAsync<long>("Calculate", a);

            await connection.StopAsync();

            Console.WriteLine($"Call ended - process ID: {Process.GetCurrentProcess().Id}");

            return result;
        }
    }
}
