using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Text;
using System.Diagnostics;

namespace Inventory_Management_Dashboard.Services
{
    internal class SerialReader
    {
        SerialPort _serialPort = new SerialPort();
        public event Action<string> RFIDDataReceived;

        //Constructor
        public SerialReader()
        {
            // Create new serial port object


            // SerialPort properties
            
            //_serialPort.Parity = _serialPort.Parity;
            //_serialPort.DataBits = _serialPort.DataBits;
            //_serialPort.StopBits = _serialPort.StopBits;
            //_serialPort.Handshake = _serialPort.Handshake;
        }

        public void StartReading(string portName, int baudRate)
        {
            if (_serialPort.IsOpen) return;

            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;
            // Subscribe handler function to the event. When _SerialPort has data, call PortDataReceived
            
            try
            {
                _serialPort.DataReceived += PortDataReceived;
                // Open serial port to start reading
                _serialPort.Open();
                Console.WriteLine("Serial port opened successfully.");
            }

            catch(Exception e)
            {
                Console.WriteLine($"Error opening serial port: {e.Message}");
            }
        }
        
        public void StopReading()
        {
            if (!_serialPort.IsOpen) return;

            
            try
            {
                _serialPort.DataReceived -= PortDataReceived;
                _serialPort.Close();
                Console.WriteLine("Serial port closed.");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error closing serial port: {e.Message}");
            }
        }

        void PortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Read all the data currently in the buffer
                string data = _serialPort.ReadLine();
                if (!string.IsNullOrEmpty(data))
                {
                    // Fire event, passing data to subscriber
                    RFIDDataReceived?.Invoke(data.Trim());
                    //Console.WriteLine(data.Trim());
                }
            }

            catch(Exception ex)
            {
                Debug.WriteLine($"An reading from port: {ex.Message}");
            }
        }
        
            
    }
}
