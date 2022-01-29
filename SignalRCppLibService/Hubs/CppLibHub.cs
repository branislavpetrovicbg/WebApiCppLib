using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace SignalRCppLibService
{
    public class CppLibHub : Hub
    {
        private static object _lock = new object();

        public Task<long> Calculate(long a)
        {
            Console.WriteLine($"[{DateTime.Now}] New Calculation request - process ID: {Process.GetCurrentProcess().Id} - Thread ID: {Thread.CurrentThread.ManagedThreadId}");

            long result;
            lock (_lock)
            {
                Console.WriteLine($"[{DateTime.Now}] Acquired lock - process id: {Process.GetCurrentProcess().Id} - Thread ID: {Thread.CurrentThread.ManagedThreadId}");

                result = CppLibWrapper.calculate(a);
                
                Console.WriteLine($"[{DateTime.Now}] Released lock  - process id: {Process.GetCurrentProcess().Id} - Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            }

            Console.WriteLine($"[{DateTime.Now}] Calculation request ended - process ID: {Process.GetCurrentProcess().Id} - Thread ID: {Thread.CurrentThread.ManagedThreadId}");

            return Task.FromResult(result);
        }
    }
}
