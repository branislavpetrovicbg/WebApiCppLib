using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    internal class Program
    {
        const int NumOfConnections = 4;

        static async Task<int> Main(string[] args)
        {

            Console.WriteLine("Enter a port number:");
            var port = Console.ReadLine();

            Console.WriteLine("Use SSL?");
            var url = Console.ReadLine().ToLower() == "y"
                ? new Uri($"https://localhost:{port}/ProxyHub")
                : new Uri($"http://localhost:{port}/ProxyHub");

            HubConnection[] connections = await EstablishConnectionsAsync(url);

            Console.WriteLine("Enter a number: ");

            string input;
            while ((input = Console.ReadLine()) != String.Empty)
            {
                if (!long.TryParse(input, out var inputNumber))
                    continue;

                var stopwatch = Stopwatch.StartNew();

                try
                {
                    var results = await CalculateResultsAsync(connections, inputNumber);

                    Console.WriteLine($"Result is: {results.Sum()}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return 1;
                }

                Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

                Console.WriteLine($"Enter another number. Enter an empty string to exit.");
            }

            await CloseConnectionsAsync(connections);

            return 0;
        }

        static async Task<IEnumerable<long>> CalculateResultsAsync(HubConnection[] connections, long inputNumber)
        {
            var calculations = new ConcurrentBag<Task<long>>();
            for (int i = 0; i < 10; i++)
            {
                calculations.Add(connections[i % NumOfConnections].InvokeAsync<long>("Calculate", inputNumber));
            }

            return await Task.WhenAll(calculations);
        }

        static async Task CloseConnectionsAsync(HubConnection[] connections)
        {
            for (int i = 0; i < NumOfConnections; i++)
            {
                await connections[i].StopAsync();

                await connections[i].DisposeAsync();
            }
        }

        static async Task<HubConnection[]> EstablishConnectionsAsync(Uri url)
        {
            Console.WriteLine("Establishing connection with server...");

            HubConnection[] connections = new HubConnection[NumOfConnections];
            for (int i = 0; i < NumOfConnections; i++)
            {
                connections[i] = new HubConnectionBuilder()
                    .WithUrl(url, option =>
                    {
                // Configure WebSockets - necessary to avoid using sticky sessions
                option.SkipNegotiation = true;
                        option.Transports = HttpTransportType.WebSockets;
                    })
                    .Build();

                await connections[i].StartAsync();

                Console.WriteLine($"Connection {i} established.");
            }

            return connections;
        }
    }
}
