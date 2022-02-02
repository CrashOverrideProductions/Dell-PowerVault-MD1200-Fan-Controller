using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanController
{
    internal class SerialCommunication
    {
        internal int GetTemperature(string port)
        {
            // Var for holding Temperature response
            int temp = 0;
            
            // Send and Get Command/Response
            string command = "_temp_rd";
            string response = SendAndGetCommand(port, command);

            // Verifiy Response... probably need to filter it first
            if (response == null)
            {
                return 0;
            }
            temp = Convert.ToInt32(response);


            return temp;
        }

        internal bool SetFanSpeed(string port, int speed)
        {
            string command = "_shutup " + speed;
            string response = SendAndGetCommand(port, command);

            // Verifiy Response... probably need to filter it first
            if (response == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        internal string SendAndGetCommand(string selectedPort, string command)
        {
            using SerialPort port = new SerialPort()
            {
                PortName = selectedPort,
                BaudRate = 38400,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                Handshake = Handshake.XOnXOff,
                ReadTimeout = 256
            };

            port.Write(command);
            System.Threading.Thread.Sleep(2);

            string response = port.ReadExisting();

            return response;

        }
}
}

// Command to read Temp - $port.WriteLine("_temp_rd")
// Command to Set Temp - $port.WriteLine("_shutup 20")

