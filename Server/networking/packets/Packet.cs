using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Packets
{
    public class Packet
    {
        protected const byte BASE_PACKET_SIZE = 4;

        private byte[] buffer;
        public byte[] Buffer { get => buffer; }

        protected Packet(ushort len, ushort type)
        {
            buffer = new byte[len];
            WriteUShort(len, 0);
            WriteUShort(type, 2);
        }

        protected Packet(byte[] packet)
        {
            buffer = packet;
        }

        public ushort PacketType
        {
            get
            {
                return ReadUShort(2);
            }
        }

        public static ushort GetPacketType(byte[] buffer)
        {
            return new Packet(buffer).PacketType;
        }

        protected ushort ReadUShort(int offset)
        {
            return BitConverter.ToUInt16(buffer, offset);
        }

        protected void WriteUShort(ushort value, int offset)
        {
            byte[] valueBuffer = BitConverter.GetBytes(value);
            Array.Copy(valueBuffer, 0, buffer, offset, valueBuffer.Length);
        }


        protected string ReadString(int offset, int count)
        {
            return Encoding.UTF8.GetString(buffer, offset, count);
        }

        protected void WriteString(string value, int offset)
        {
            byte[] valueBuffer = new byte[value.Length];
            valueBuffer = Encoding.UTF8.GetBytes(value);

            Array.Copy(valueBuffer, 0, buffer, offset, valueBuffer.Length);
        }


        protected T ReadJsonObject<T>()
        {
            string json = ReadString(BASE_PACKET_SIZE, 0);
            return JsonSerializer.Deserialize<T>(json);
        }

        protected void WriteJsonObject<T>(T toWrite)
        {
            string json = JsonSerializer.Serialize(toWrite);
            WriteString(json, BASE_PACKET_SIZE);
        }
    }
}
