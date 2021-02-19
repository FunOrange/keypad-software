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
using System.Threading;

namespace KeypadSoftware
{
    // This class supports creating a connection to the keypad and
    // exposes methods to easily send and receive high level information
    public class KeypadSerial
    {
        // state variables
        private SerialPort keypadPort;
        public bool IsConnected;
        public enum PortStatus
        {
            Untested,
            Failed,
            Good
        }
        public static string StatusToString(PortStatus status)
        {
            switch (status)
            {
                case PortStatus.Untested:
                    return "...";
                case PortStatus.Failed:
                    return "✖";
                case PortStatus.Good:
                    return "✔";
                default:
                    return "";
            }
        }
        // int: priority
        public Dictionary<string, (int, PortStatus)> PortList;

        public KeypadSerial()
        {
            IsConnected = false;
            PortList = new Dictionary<string, (int, PortStatus)>();
        }
        public void UpdatePortList()
        {
            List<string> connectedPorts = SerialPort.GetPortNames().ToList();

            // Add new ports to list (eg. device connected)
            foreach (string port in connectedPorts.Where((port) => !PortList.ContainsKey(port)))
                PortList.Add(port, (0, PortStatus.Untested));

            // Remove ports from list (eg. device disconnected)
            foreach (string disconnectedPort in PortList.Keys.Where((port) => !connectedPorts.Contains(port)))
                PortList.Remove(disconnectedPort);
 
            // Mark high priority ports
            List<string> candidatePorts = new List<string>();
            #region Look in registry for devices with matching VID/PID
            Regex vidPidPattern = new Regex("^VID_03EB.PID_2042", RegexOptions.IgnoreCase);
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
                            candidatePorts.Add(PortName.ToString());
                    }
                }
            }
            #endregion

            foreach (string highPriorityPort in candidatePorts.Where((port) => PortList.ContainsKey(port)))
            {
                (int priority, PortStatus status) = PortList[highPriorityPort];
                PortList[highPriorityPort] = (1, status);
            }
        }

        public bool UntestedPortsAvailable()
        {
            return PortList.Count > 0;
        }
        public string NextPort()
        {
            // high priority ports
            var p = PortList.FirstOrDefault(kvp => (kvp.Value.Item1 == 1 && kvp.Value.Item2 == PortStatus.Untested));
            if (!p.Equals(default(KeyValuePair<string, (int, PortStatus)>)))
                return p.Key;

            // low priority ports
            p = PortList.FirstOrDefault(kvp => (kvp.Value.Item1 == 0 && kvp.Value.Item2 == PortStatus.Untested));
            if (!p.Equals(default(KeyValuePair<string, (int, PortStatus)>)))
                return p.Key;

            return "";
        }
        public async Task TryNextPortAsync()
        {
            string port = NextPort();
            Console.WriteLine($"Trying port {port}... ");
            SerialPort result = await Task.Run(() => TryHandShake(port));
            if (result != null)
            {
                Console.WriteLine("handshake succeeded!");
                keypadPort = result;
                IsConnected = true;
                // set port status to good
                (int priority1, PortStatus status1) = PortList[port];
                PortList[port] = (priority1, PortStatus.Good);
                return;
            }
            
            // set port status to failed
            (int priority2, PortStatus status2) = PortList[port];
            PortList[port] = (priority2, PortStatus.Failed);
            Console.WriteLine("handshake failed.");
        }

        // Tries to send a handshake packet to the COM Port. If the keypad responds, then return the SerialPort object.
        // Otherwise, return null on failure;
        private SerialPort TryHandShake(string portName)
        {
            try
            {
                SerialPort testPort = new SerialPort(portName, 9600);
                testPort.ReadTimeout = 500;
                testPort.Open();
                byte[] handShakePacket = KeypadSerialProtocol.CreateEmptyPacket(KeypadSerialProtocol.KEYPAD_PACKET_ID_HEARTBEAT);
                //testPort.Write(handShakePacket, 0, handShakePacket.Length);
                testPort.WriteLine("fun");
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
