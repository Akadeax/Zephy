using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public abstract class PacketStructure
    {
        private byte[] buffer;

        public PacketStructure(ushort len, ushort type)
        {
            buffer = new byte[len];
            WriteUShort(len, 0);
            WriteUShort(type, 2);
        }

        public PacketStructure(byte[] packet)
        {
            buffer = packet;
        }

        public ushort ReadUShort(int offset)
        {
            return BitConverter.ToUInt16(buffer, offset);
        }

        public void WriteUShort(ushort value, int offset)
        {
            byte[] valueBuffer = new byte[2];
            valueBuffer = BitConverter.GetBytes(value);
            Array.Copy(valueBuffer, 0, buffer, offset, valueBuffer.Length);
        }


        public string ReadString(int offset, int count)
        {
            return Encoding.UTF8.GetString(buffer, offset, count);
        }

        public void WriteString(string value, int offset)
        {
            byte[] valueBuffer = new byte[value.Length];
            valueBuffer = Encoding.UTF8.GetBytes(value);

            Array.Copy(valueBuffer, 0, buffer, offset, valueBuffer.Length);
        }

        public byte[] Buffer { get => buffer; }
    }
}
