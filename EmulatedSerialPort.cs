using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware
{
    public class EmulatedSerialPort
    {
        SerialPort port;
        public EmulatedSerialPort(SerialPort p)
        {
            port = p;
        }

        // All write methods append to file C:\host_to_device
        public void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        public void WriteLine(string line)
        {
            throw new NotImplementedException();
        }
        // All read methods read from file C:\device_to_host
        // Contents that are read are removed from the file
        public string ReadLine()
        {
            // Blocks until a new line is available
            throw new NotImplementedException();
        }
        public void Read(byte[] buffer, int offset, int count)
        {
            // open file for reading
            throw new NotImplementedException();
        }

        public void Close() => port.Close();
        public string PortName => port.PortName;

        public bool IsOpen => port.IsOpen;


    }
}
