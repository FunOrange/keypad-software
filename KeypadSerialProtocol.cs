using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KeypadSoftware
{
    // This class contains functions for creating the packets to be sent to the keypad
    static class KeypadSerialProtocol
    {
        static int NUM_LEDS = 12;
        // Packet IDs
        public static byte KEYPAD_PACKET_ID_HEARTBEAT = 0;
        public static byte KEYPAD_PACKET_ID_RESET_EEPROM = 1;
        public static byte KEYPAD_PACKET_ID_SET_BASE_COLOUR = 2;
        public static byte KEYPAD_PACKET_ID_GET_BASE_COLOUR = 3;
        public static byte KEYPAD_PACKET_ID_SET_HUE_DELTA = 4;
        public static byte KEYPAD_PACKET_ID_GET_HUE_DELTA = 5;
        public static byte KEYPAD_PACKET_ID_SET_HUE_PERIOD = 6;
        public static byte KEYPAD_PACKET_ID_GET_HUE_PERIOD = 7;
        public static byte KEYPAD_PACKET_ID_SET_VALUE_DIM = 8;
        public static byte KEYPAD_PACKET_ID_GET_VALUE_DIM = 9;
        public static byte KEYPAD_PACKET_ID_SET_VALUE_PERIOD = 10;
        public static byte KEYPAD_PACKET_ID_GET_VALUE_PERIOD = 11;
        public static byte KEYPAD_PACKET_ID_SET_FLASH_LEFT_COLOUR = 12;
        public static byte KEYPAD_PACKET_ID_GET_FLASH_LEFT_COLOUR = 13;
        public static byte KEYPAD_PACKET_ID_SET_FLASH_RIGHT_COLOUR = 14;
        public static byte KEYPAD_PACKET_ID_GET_FLASH_RIGHT_COLOUR = 15;
        public static byte KEYPAD_PACKET_ID_SET_FLASH_BLENDING_METHOD = 16;
        public static byte KEYPAD_PACKET_ID_GET_FLASH_BLENDING_METHOD = 17;
        public static byte KEYPAD_PACKET_ID_SET_FLASH_HOLD_METHOD = 18;
        public static byte KEYPAD_PACKET_ID_GET_FLASH_HOLD_METHOD = 19;
        public static byte KEYPAD_PACKET_ID_SET_FLASH_DECAY_RATE = 20;
        public static byte KEYPAD_PACKET_ID_GET_FLASH_DECAY_RATE = 21;
        public static byte KEYPAD_PACKET_ID_SET_DELAY_MULTIPLIER = 22;
        public static byte KEYPAD_PACKET_ID_GET_DELAY_MULTIPLIER = 23;
        public static byte KEYPAD_PACKET_ID_SET_LINE_DELAY = 24;
        public static byte KEYPAD_PACKET_ID_GET_LINE_DELAY = 25;
        public static byte KEYPAD_PACKET_ID_SET_COMPONENT_ENABLE_MASK = 26;
        public static byte KEYPAD_PACKET_ID_GET_COMPONENT_ENABLE_MASK = 27;
        public static byte KEYPAD_PACKET_ID_SET_GLOBAL_BRIGHTNESS = 28;
        public static byte KEYPAD_PACKET_ID_GET_GLOBAL_BRIGHTNESS = 29;
        public static byte KEYPAD_PACKET_ID_SET_KEYBINDS = 30;
        public static byte KEYPAD_PACKET_ID_GET_KEYBINDS = 31;
        public static byte KEYPAD_PACKET_ID_SET_DEBOUNCE = 32;
        public static byte KEYPAD_PACKET_ID_GET_DEBOUNCE = 33;

        public static byte[] CreateEmptyPacket(byte packetId)
        {
            return CreatePacket(packetId, new byte[0]);
        }

        public static byte[] CreatePacket(byte packetId, byte[] data)
        {
            if (data.Length > 128)
                throw new ArgumentException("data exceeds max length of 128");
            byte[] packet = new byte[data.Length + 4];
            packet[0] = 0x80;
            packet[1] = packetId;
            packet[2] = (byte)data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                packet[3 + i] = data[i];
            }
            packet[3 + data.Length] = 0x08;
            return packet;
        }

        public static byte[] SerializeRgbList(List<Color> colors)
        {
            if (colors.Count != NUM_LEDS)
                throw new ArgumentException($"list does not match expected length of {NUM_LEDS}");

            byte[] data = new byte[colors.Count * 3];
            for (int i = 0; i < colors.Count; i++)
            {
                data[3*i + 0] = colors[i].R;
                data[3*i + 1] = colors[i].G;
                data[3*i + 2] = colors[i].B;
            }
            return data;
        }

        // Convert a uint16 list into a little-endian byte array
        public static byte[] SerializeUint16List(List<UInt16> ints)
        {
            if (ints.Count != NUM_LEDS)
                throw new ArgumentException($"list does not match expected length of {NUM_LEDS}");

            byte[] data = new byte[ints.Count * 2];
            for (int i = 0; i < ints.Count; i++)
            {
                data[2*i + 0] = (byte)((ints[i] >> 0) & 0xff); // LSB
                data[2*i + 1] = (byte)((ints[i] >> 8) & 0xff); // MSB
            }
            return data;
        }

        // Convert a uint32 list into a little-endian byte array
        public static byte[] SerializeUint32List(List<UInt32> ints)
        {
            if (ints.Count != NUM_LEDS)
                throw new ArgumentException($"list does not match expected length of {NUM_LEDS}");

            byte[] data = new byte[ints.Count * 4];
            for (int i = 0; i < ints.Count; i++)
            {
                data[4*i + 0] = (byte)((ints[i] >>  0) & 0xff); // LSB
                data[4*i + 1] = (byte)((ints[i] >>  8) & 0xff);
                data[4*i + 2] = (byte)((ints[i] >> 16) & 0xff);
                data[4*i + 3] = (byte)((ints[i] >> 24) & 0xff); // MSB
            }
            return data;
        }

        // Convert a float list into a little-endian byte array
        public static byte[] SerializeFloatList(List<float> floats)
        {
            if (floats.Count != NUM_LEDS)
                throw new ArgumentException($"list does not match expected length of {NUM_LEDS}");

            byte[] data = new byte[floats.Count * 4];
            for (int i = 0; i < floats.Count; i++)
            {
                byte[] floatBytes = BitConverter.GetBytes(floats[i]);
                if (BitConverter.IsLittleEndian)
                {
                    data[4*i + 0] = floatBytes[0]; // LSB
                    data[4*i + 1] = floatBytes[1];
                    data[4*i + 2] = floatBytes[2];
                    data[4*i + 3] = floatBytes[3]; // MSB
                }
                else // Big Endian
                {
                    data[4*i + 0] = floatBytes[3]; // LSB
                    data[4*i + 1] = floatBytes[2];
                    data[4*i + 2] = floatBytes[1];
                    data[4*i + 3] = floatBytes[0]; // MSB
                }
            }
            return data;
        }

    }
}
