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
using Caliburn.Micro;
using System.Windows.Media;

namespace KeypadSoftware
{
    // This class supports creating a connection to the keypad and
    // exposes methods to easily send and receive high level information
    public class KeypadSerial
    {
        // state variables
        private SerialPort keypadPort;

        private const int NUM_LEDS = 12;

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; }
        }

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

        public BindableCollection<Tuple<string, string>> GetPresentablePrioritylist(int priority)
        {
            string currentPort = NextPort();
            var pl =
                PortList
                .Where(kvp => kvp.Value.Item1 == priority)
                .Select(kvp => new Tuple<string, string>(kvp.Key, $"{(kvp.Key == currentPort ? "＊" : KeypadSerial.StatusToString(kvp.Value.Item2))}"));
            return new BindableCollection<Tuple<string, string>>(pl);

        }

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
            if (IsConnected)
                return "";

            // high priority ports
            var p = PortList.FirstOrDefault(kvp => (kvp.Value.Item1 == 1 && kvp.Value.Item2 == PortStatus.Untested));
            if (!p.Equals(default(KeyValuePair<string, (int, PortStatus)>)))
                return p.Key;

            // if all high priority ports have already been tried, mark them as untested so that they can be retried
            var retryPorts = PortList.Where(kvp => kvp.Value.Item1 == 1 && kvp.Value.Item2 == PortStatus.Failed).Select(kvp => kvp.Key).ToList();
            foreach (string port in retryPorts)
                PortList[port] = (1, PortStatus.Untested);

            // low priority ports
            //p = PortList.FirstOrDefault(kvp => (kvp.Value.Item1 == 0 && kvp.Value.Item2 == PortStatus.Untested));
            //if (!p.Equals(default(KeyValuePair<string, (int, PortStatus)>)))
            //    return p.Key;
            return "";
        }
        // This function sets IsConnected to true on success
        public void TryNextPort()
        {
            string port = NextPort();
            if (port == "")
            {
                return;
            }
            Console.Write($"Trying port {port}... ");
            SerialPort result = TryHandShake(port);
            if (result != null)
            {
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
                //byte[] handShakePacket = KeypadSerialProtocol.CreateEmptyPacket(KeypadSerialProtocol.KEYPAD_PACKET_ID_HEARTBEAT);
                //testPort.Write(handShakePacket, 0, handShakePacket.Length);
                testPort.WriteLine("fun");
                try
                {
                    if (testPort.ReadLine() == "orange")
                    {
                        Console.WriteLine("handshake success!");
                        return testPort;
                    }
                    else
                    {
                        Console.WriteLine("received garbage");
                        return null;
                    }
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Timed out while waiting for handshake");
                    testPort.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                // eg. port does not exist
                Thread.Sleep(2000);
                Console.WriteLine(e.Message);
                return null;
            }
        }

        // This function sets IsConnected to false if no heartbeat is detected
        public bool Heartbeat()
        {
            if (!IsConnected)
                throw new Exception("heartbeat method called while keypad is not connected.");

            void KeypadDisconnect()
            {
                IsConnected = false;
                keypadPort.Close();
                PortList.Remove(keypadPort.PortName);
            }

            if (!keypadPort.IsOpen)
            {
                Console.WriteLine("keypad disconnected (port was closed)");
                KeypadDisconnect();
                return false;
            }

            //byte[] handShakePacket = KeypadSerialProtocol.CreateEmptyPacket(KeypadSerialProtocol.KEYPAD_PACKET_ID_HEARTBEAT);
            //testPort.Write(handShakePacket, 0, handShakePacket.Length);
            keypadPort.WriteLine("fun");
            try
            {
                if (keypadPort.ReadLine() == "orange")
                {
                    IsConnected = true;
                    return true;
                }
                else
                {
                    KeypadDisconnect();
                    return false;
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine("keypad disconnected (heartbeat timed out)");
                KeypadDisconnect();
                return false;
            }
        }

        #region Read Data
        public byte[] RequestDataGeneric(byte requestPacketId, int expectedBytes)
        {
            if (!IsConnected)
                throw new Exception("Can't read data when keypad is not connected");
            // Send packet to request keybinds
            byte[] packet = KeypadSerialPacket.CreateEmptyPacket(requestPacketId);
            keypadPort.Write(packet, 0, packet.Length);

            // Receive data
            byte[] receivedData = new byte[expectedBytes];
            keypadPort.Read(receivedData, 0, expectedBytes);
            return receivedData;
        }

        public List<Color> ReadBaseColour()
        {
            byte[] rawData = RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_BASE_COLOUR, 3 * NUM_LEDS);
            return KeypadSerialPacket.DeserializeRgbList(rawData);
        }
        public List<UInt16> ReadHueDelta(byte[] data) {
            byte[] rawData = RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_HUE_DELTA, 2 * NUM_LEDS);
            return KeypadSerialPacket.DeserializeUint16List(rawData);
        }
        public List<UInt32> ReadHuePeriod(byte[] data) {
            byte[] rawData = RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_HUE_PERIOD, 4 * NUM_LEDS);
            return KeypadSerialPacket.DeserializeUint32List(rawData);
        }
        public List<float> ReadValueDim(byte[] data) {
            byte[] rawData = RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_VALUE_DIM, 4 * NUM_LEDS);
            return KeypadSerialPacket.DeserializeFloatList(rawData);
        }
        public List<UInt32> ReadValuePeriod(byte[] data) {
            byte[] rawData = RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_VALUE_PERIOD, 4 * NUM_LEDS);
            return KeypadSerialPacket.DeserializeUint32List(rawData);
        }
        public List<Color> ReadFlashLeftColour(byte[] data) {
            byte[] rawData = RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_FLASH_LEFT_COLOUR, 3 * NUM_LEDS);
            return KeypadSerialPacket.DeserializeRgbList(rawData);
        }
        public List<Color> ReadFlashRightColour(byte[] data) {
            byte[] rawData = RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_FLASH_RIGHT_COLOUR, 3 * NUM_LEDS);
            return KeypadSerialPacket.DeserializeRgbList(rawData);
        }
        //public List<byte> ReadFlashBlendingMethod(byte[] data) {}
        //public List<byte> ReadFlashHoldMethod(byte[] data) {}
        public List<float> ReadFlashDecayRate(byte[] data) {
            byte[] rawData = RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_FLASH_DECAY_RATE, 4 * NUM_LEDS);
            return KeypadSerialPacket.DeserializeFloatList(rawData);
        }
        public byte ReadDelayMultiplier(byte[] data) {
            return RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_DELAY_MULTIPLIER, 1)[0];
        }
        public byte[] ReadLineDelay(byte[] data) {
            return RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_LINE_DELAY, NUM_LEDS);
        }

        public byte[] ReadKeybinds()
        {
            return RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_KEYBINDS, 3);
        }
        public byte[] ReadDebounce()
        {
            return RequestDataGeneric(KeypadSerialPacket.KEYPAD_PACKET_ID_GET_DEBOUNCE, 5);
        }
        #endregion

        #region Write Data
        public void WriteBaseColor(List<Color> c)
        {
            byte[] data = KeypadSerialPacket.SerializeRgbList(c);
            byte[] packet = KeypadSerialPacket.CreatePacket(KeypadSerialPacket.KEYPAD_PACKET_ID_SET_BASE_COLOUR, data);
            keypadPort.Write(packet, 0, packet.Length);
        }
        public void WriteHueDelta(List<UInt16> d)
        {
            byte[] data = KeypadSerialPacket.SerializeUint16List(d);
            byte[] packet = KeypadSerialPacket.CreatePacket(KeypadSerialPacket.KEYPAD_PACKET_ID_SET_HUE_DELTA, data);
            keypadPort.Write(packet, 0, packet.Length);
        }
        public void WriteHuePeriod(List<UInt32> p)
        {
            byte[] data = KeypadSerialPacket.SerializeUint32List(p);
            byte[] packet = KeypadSerialPacket.CreatePacket(KeypadSerialPacket.KEYPAD_PACKET_ID_SET_HUE_PERIOD, data);
            keypadPort.Write(packet, 0, packet.Length);
        }
        public void WriteValueDim(List<float> d)
        {
            byte[] data = KeypadSerialPacket.SerializeFloatList(d);
            byte[] packet = KeypadSerialPacket.CreatePacket(KeypadSerialPacket.KEYPAD_PACKET_ID_SET_VALUE_DIM, data);
            keypadPort.Write(packet, 0, packet.Length);
        }
        public void WriteValuePeriod(List<UInt32> p)
        {
            byte[] data = KeypadSerialPacket.SerializeUint32List(p);
            byte[] packet = KeypadSerialPacket.CreatePacket(KeypadSerialPacket.KEYPAD_PACKET_ID_SET_VALUE_PERIOD, data);
            keypadPort.Write(packet, 0, packet.Length);
        }
        public void WriteFlashLeftColour(List<Color> c)
        {
            byte[] data = KeypadSerialPacket.SerializeRgbList(c);
            byte[] packet = KeypadSerialPacket.CreatePacket(KeypadSerialPacket.KEYPAD_PACKET_ID_SET_FLASH_LEFT_COLOUR, data);
            keypadPort.Write(packet, 0, packet.Length);
        }
        public void WriteFlashRightColour(List<Color> c)
        {
            byte[] data = KeypadSerialPacket.SerializeRgbList(c);
            byte[] packet = KeypadSerialPacket.CreatePacket(KeypadSerialPacket.KEYPAD_PACKET_ID_SET_FLASH_RIGHT_COLOUR, data);
            keypadPort.Write(packet, 0, packet.Length);
        }
        public void WriteFlashDecayRate(List<float> r)
        {
            byte[] data = KeypadSerialPacket.SerializeFloatList(r);
            byte[] packet = KeypadSerialPacket.CreatePacket(KeypadSerialPacket.KEYPAD_PACKET_ID_SET_FLASH_DECAY_RATE, data);
            keypadPort.Write(packet, 0, packet.Length);
        }
        public void WriteDelayMultiplier(byte dm)
        {
            byte[] data = new byte[1];
            data[0] = dm;
            byte[] packet = KeypadSerialPacket.CreatePacket(KeypadSerialPacket.KEYPAD_PACKET_ID_SET_DELAY_MULTIPLIER, data);
            keypadPort.Write(packet, 0, packet.Length);
        }
        public void WriteLineDelay(byte[] d)
        {
            byte[] packet = KeypadSerialPacket.CreatePacket(KeypadSerialPacket.KEYPAD_PACKET_ID_SET_LINE_DELAY, d);
            keypadPort.Write(packet, 0, packet.Length);
        }


        #endregion
    }
}
