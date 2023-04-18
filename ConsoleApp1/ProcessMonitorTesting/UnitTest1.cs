using System.Windows;
using System.Diagnostics;

namespace ProcessMonitor.Tests
{
    [TestFixture]
    public class Tests
    {

        // Test case for checking if process is killed when it exceeds max lifetime
        [Test]
        public void Test_ProcessKilledWhenExceedsMaxLifetime()
        {
           
            string processName = "notepad";
            int maxLifetimeMinutes = 1;
            int monitorFrequencyMinutes = 1;

      
            using (Process process = new())
            {
                process.StartInfo.FileName = "notepad.exe";
                process.Start();

                
                process.WaitForInputIdle();


                Thread.Sleep((maxLifetimeMinutes + 1) * 60000);

        
                Assert.IsFalse(Process.GetProcessesByName(processName).Length > 0);
            }
        }

        // Test case for checking if process is not killed when it is within max lifetime
        [Test]
        public void Test_ProcessNotKilledWithinMaxLifetime()
        {
      
            string processName = "notepad";
            int maxLifetimeMinutes = 10;
            int monitorFrequencyMinutes = 1;

           
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "notepad.exe";
                process.Start();

                
                process.WaitForInputIdle();

             
                System.Threading.Thread.Sleep((maxLifetimeMinutes - 1) * 60000);

              
                Assert.IsTrue(Process.GetProcessesByName(processName).Length > 0);
            }
        }



        [Test]
        public void Test_ProcessKilledOnKeyPress()
        {
            // Arrange
            string processName = "notepad";
            int maxLifetimeMinutes = 10;
            int monitorFrequencyMinutes = 1;

            // Create a new thread to simulate key press
            Thread keyPressThread = new Thread(() =>
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.KeyChar == 'q' || keyInfo.KeyChar == 'Q')
                        {
                            break;
                        }
                    }
                }
            });

            // Act
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "notepad.exe";
                process.Start();

                //
                process.WaitForInputIdle();

           
                Thread monitoringThread = new Thread(() =>
                {
                    Program.Main(new string[] { processName, maxLifetimeMinutes.ToString(), monitorFrequencyMinutes.ToString() });
                });
                monitoringThread.Start();

              
                keyPressThread.Start();

           
                Thread.Sleep(1000);

                // Press q key
                keyPressThread.Join();

                Thread.Sleep(2000);

     
                Assert.IsFalse(Process.GetProcessesByName(processName).Length > 0);
            }
        }
    }
}