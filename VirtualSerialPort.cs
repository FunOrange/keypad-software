using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeypadSoftware
{
    public class VirtualSerialPort
    {
        public static readonly string HostToDevice = Path.Combine(Environment.GetEnvironmentVariable("userprofile"), "host_to_device");
        public static readonly string DeviceToHost = Path.Combine(Environment.GetEnvironmentVariable("userprofile"), "device_to_host");

        SerialPort port;
        public VirtualSerialPort(SerialPort p)
        {
            port = p;
        }

        // Write bytes to virtual serial tx buffer
        public void Write(byte[] buffer, int offset, int count)
        {
            using (var fileStream = new FileStream(HostToDevice, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fileStream))
            {
                bw.Write(buffer);
            }
        }

        public void WriteLine(string line)
        {
            throw new NotImplementedException();
        }

        // Reads a UTF8 string line from virtual serial rx buffer
        // Blocks until a line is available to be read
        public string ReadLine()
        {
            // Block until a new line is available
            byte[] data;
            do
            {
                data = File.ReadAllBytes(DeviceToHost);
            } while (data.Contains((byte)'\n'));

            // Write back the data minus the part that's being read
            int count = Array.IndexOf(data, (byte)'\n') + 1;
            using (var fileStream = new FileStream(DeviceToHost, FileMode.Open, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fileStream))
            {
                bw.Write(data, count, data.Length - count);
            }

            // Return the data being read
            return Encoding.UTF8.GetString(data).TrimEnd();
        }

        // Read <count> bytes from virtual serial rx buffer
        // Blocks until all bytes are available
        public void Read(out byte[] buffer, int offset, int count)
        {
            // Block until <count> bytes are available
            byte[] data;
            do
            {
                data = File.ReadAllBytes(DeviceToHost);
            } while (data.Length < count);

            using (var fileStream = new FileStream(DeviceToHost, FileMode.Open, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fileStream))
            {
                // write back the data minus the part that's being read
                bw.Write(data, count, data.Length - count);
            }
            // Return the data being read
            buffer = new byte[count];
            Buffer.BlockCopy(data, 0, buffer, 0, count);
        }

        public void Close() => port.Close();
        public string PortName => port.PortName;

        public bool IsOpen => port.IsOpen;


    }
}
