using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware
{
    class KeypadSerialPacketReader
    {
        static byte START_BYTE = 0x80;
        static byte END_BYTE = 0x08;

        public enum ProtocolReadState {
            IDLE,
            EXPECTING_ID,
            EXPECTING_LENGTH,
            READING_DATA,
            EXPECTING_END,
            PACKET_COMPLETE
        };
        // buffer to use when reading in packets
        byte[] buffer = new byte[128 + 4];
        int buffer_ptr = 0;

        // FSM
        ProtocolReadState state = ProtocolReadState.IDLE;
        public ProtocolReadState protocol_read_byte(byte data)
        {
            switch (state)
            {
            case ProtocolReadState.IDLE:
                if (data == START_BYTE) {
                    // start reading
                    buffer_ptr = 0;
                    buffer[0] = data;
                    state = ProtocolReadState.EXPECTING_ID;
                }
                break;
            case ProtocolReadState.EXPECTING_ID:
                buffer[++buffer_ptr] = data;
                state = ProtocolReadState.EXPECTING_LENGTH;
                break;
            case ProtocolReadState.EXPECTING_LENGTH:
                buffer[++buffer_ptr] = data;
                state = (data > 0) ? ProtocolReadState.READING_DATA : ProtocolReadState.EXPECTING_END;
                break;
            case ProtocolReadState.READING_DATA:
            {
                // read in byte into buffer
                buffer[++buffer_ptr] = data;
                byte data_length = buffer[2];
                if (buffer_ptr == data_length + 2) {
                    state = ProtocolReadState.EXPECTING_END;
                }
            }
                break;
            case ProtocolReadState.EXPECTING_END:
                if (data == END_BYTE) {
                    buffer[++buffer_ptr] = data; // unnecessary but w/e
                    state = ProtocolReadState.PACKET_COMPLETE;
                } else {
                    state = ProtocolReadState.IDLE; // throw away packet
                }
                break;
            case ProtocolReadState.PACKET_COMPLETE:
                break;
            default:
                break;
            }
            return state;
        }

        public bool protocol_packet_ready(out KeypadSerialPacket packet)
        {
            packet = new KeypadSerialPacket();
            if (state == ProtocolReadState.PACKET_COMPLETE) {
                packet.id = buffer[1];
                packet.length = buffer[2];
                packet.data = new byte[packet.length];
                Buffer.BlockCopy(buffer, 3, packet.data, 0, (int)packet.length);
                state = ProtocolReadState.IDLE;
                return true;
            }
            return false;
        }
    }
}
