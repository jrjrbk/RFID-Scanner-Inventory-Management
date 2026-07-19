using Inventory_Management_Dashboard.Data;
using Inventory_Management_Dashboard.Models;
using Inventory_Management_Dashboard.Services;
using System.Diagnostics;
using System.Security.Cryptography;

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
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        
    }
}