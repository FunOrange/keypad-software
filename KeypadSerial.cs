using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace KeypadSoftware
{
    // This class supports creating a connection to the keypad and
    // exposes methods to easily send and receive high level information
    public class KeypadSerial
    {
        // state variables
        private SerialPort keypadPort;
        private bool connectionEstablished;

        // simulated eeprom
        public KeypadSerial()
        {
            connectionEstablished = false;
        }

        // Tries to automatically connect to the keypad
        public void ConnectKeypad()
        {
            // Get a list of serial port names.
            string[] connectedPorts = SerialPort.GetPortNames();

            Console.WriteLine("The following serial ports were found:");

            // Display each port name to the console.
            foreach (string port in connectedPorts)
            {
                Console.WriteLine(port);
            }

            // Try to narrow down possible ports by matching USB PID/VID
            Regex vidPidPattern = new Regex("^VID_03EB.PID_2042", RegexOptions.IgnoreCase);
            List<string> candidatePorts = new List<string>();

            RegistryKey USBRegistryKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USB");
            var possibleDeviceKeys =
                USBRegistryKey.GetSubKeyNames().
                Where((name) => vidPidPattern.IsMatch(name))
                .Select((name) => USBRegistryKey.OpenSubKey(name));

            foreach (RegistryKey possibleKey in possibleDeviceKeys)
            {
                foreach (RegistryKey subKey in possibleKey.GetSubKeyNames().Select((name) => possibleKey.OpenSubKey(name)))
                {
                    if (subKey.GetSubKeyNames().Contains("Device Parameters"))
                    {
                        RegistryKey deviceParams = subKey.OpenSubKey("Device Parameters");
                        object PortName = deviceParams.GetValue("PortName");
                        if (PortName != null)
                        {
                            candidatePorts.Add(PortName.ToString());
                        }
                    }
                }
            }

            List<string> highPriorityPorts = connectedPorts.Intersect(candidatePorts).ToList();
            List<string> lowPriorityPorts = connectedPorts.Except(highPriorityPorts).ToList();

            Console.WriteLine("\nTrying ports... (high priority)");
            foreach (string portName in highPriorityPorts)
            {
                Console.WriteLine($"Trying port {portName}... ");
                SerialPort result = TryHandShake(portName);
                if (result == null)
                {
                    Console.WriteLine("handshake failed.");
                }
                else
                {
                    Console.WriteLine("handshake succeeded!");
                    keypadPort = result;
                    return;
                }
            }

            Console.WriteLine("\nTrying ports... (low priority)");
            foreach (string portName in lowPriorityPorts)
            {
                Console.WriteLine($"Trying port {portName}... ");
                SerialPort result = TryHandShake(portName);
                if (result == null)
                {
                    Console.WriteLine("handshake failed.");
                }
                else
                {
                    Console.WriteLine("handshake succeeded!");
                    keypadPort = result;
                    return;
                }
            }
        }

        // Tries to send a handshake packet to the COM Port. If the keypad responds, then return the SerialPort object.
        // Otherwise, return null on failure;
        private SerialPort TryHandShake(string portName)
        {
            try
            {
                SerialPort testPort = new SerialPort(portName, 9600);
                testPort.ReadTimeout = 1000;
                testPort.Open();
                byte[] handShakePacket = KeypadSerialProtocol.CreateEmptyPacket(KeypadSerialProtocol.KEYPAD_PACKET_ID_HEARTBEAT);
                testPort.Write(handShakePacket, 0, handShakePacket.Length);
                try
                {
                    return (testPort.ReadLine() == "orange") ? testPort : null;
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Timed out while waiting for handshake");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
