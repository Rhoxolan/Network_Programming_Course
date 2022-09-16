using System.Diagnostics;

namespace _2022._09._09_HW__Part_I___Server_
{
    internal class Program
    {
        static void Main()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            ServerApplication app = new();
            app.Start();
        }
    }
}