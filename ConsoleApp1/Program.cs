using System;
using System.Diagnostics;
using System.Threading;
//using NUnit.Framework;

namespace ProcessMonitor
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Arguments : [processName] [maxLifetimeMinutes] [monitorFrequencyMinutes]");
                return;
            }

            string processName = args[0];
            int maxLifetimeMinutes = Convert.ToInt32(args[1]);
            int monitorFrequencyMinutes = Convert.ToInt32(args[2]);

            Console.WriteLine($"Monitoring process '{processName}' with a maximum lifetime of {maxLifetimeMinutes} minutes...");
            Console.WriteLine($"Monitoring frequency: {monitorFrequencyMinutes} minute(s). Press 'q' to quit.");

            do
            {
                Process[] processes = Process.GetProcessesByName(processName);

                foreach (var process in processes)
                {
                    TimeSpan processLifetime = DateTime.Now - process.StartTime;
                    Console.WriteLine(processLifetime.TotalMinutes);
                    if (processLifetime.TotalMinutes >= maxLifetimeMinutes)
                    {
                        Console.WriteLine($"Killing process '{process.ProcessName}' with Process ID: {process.Id}");
                        process.Kill();
                    }
                }
               
                Thread.Sleep(monitorFrequencyMinutes * 60000); 
            }
            while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Q) ; 
        }
    }
}
