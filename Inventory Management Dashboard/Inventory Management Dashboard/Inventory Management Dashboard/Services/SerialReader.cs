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
        StringBuilder _buffer = new StringBuilder();
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
                string chunk = _serialPort.ReadExisting();
                _buffer.Append(chunk);

                string currentText = _buffer.ToString();

                int lineEnd = currentText.IndexOf('\n');

                while (lineEnd != -1)
                {
                    // Extract single line from buffer
                    string line = currentText.Substring(0, lineEnd).Trim();

                    if (!string.IsNullOrEmpty(line))
                    {
                        // Fire event, passing data to subscriber
                        RFIDDataReceived?.Invoke(line);
                        //Console.WriteLine(data.Trim());
                    }

                    // Remove the line from buffer
                    _buffer.Remove(0, lineEnd + 1);

                    currentText = _buffer.ToString();
                    lineEnd = currentText.IndexOf('\n');
                }
            }

            catch(Exception ex)
            {
                Debug.WriteLine($"An reading from port: {ex.Message}");
            }
        }
        
            
    }
}
