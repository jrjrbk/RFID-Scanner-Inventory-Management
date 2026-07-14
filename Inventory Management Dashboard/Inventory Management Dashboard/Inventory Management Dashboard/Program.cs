using System.Diagnostics;
using Inventory_Management_Dashboard.Services;

namespace Inventory_Management_Dashboard
{
    
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());
            Console.WriteLine("helloworld");
            
            var RFIDReader = new SerialReader();
            RFIDReader.RFIDDataReceived += RFIDDataReceivedHandler;

            RFIDReader.StartReading("COM14", 9600);
            Console.WriteLine("Press any key to stop");
            Console.ReadKey();
            RFIDReader.StopReading();
        }

        static void RFIDDataReceivedHandler(string data)
        {
            string UID;
            Console.WriteLine($"Data received: {data}");
            
            if (data.Contains(":"))
            {
                UID = data.Split(':')[1].Trim();
                Console.WriteLine(UID);
            }
        }
    }
}