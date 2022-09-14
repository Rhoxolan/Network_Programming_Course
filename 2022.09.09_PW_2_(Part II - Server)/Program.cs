namespace _2022._09._09_PW_2__Part_II___Server_
{
    internal class Program
    {
        private static ManualResetEvent manualExit = new ManualResetEvent(false);

        static void Main()
        {
            Task.Run(ExitWait);
            Task.Run(ManualExitWait);
            ServerApplication app = new();
            app.Start();
            manualExit.WaitOne();
        }

        static private void ExitWait()
        {
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Delete || Console.ReadKey().Key == ConsoleKey.Backspace)
                {
                    Environment.Exit(0);
                }
            }
        }

        static private void ManualExitWait()
        {
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.End)
                {
                    manualExit.Set();
                }
            }
        }
    }
}