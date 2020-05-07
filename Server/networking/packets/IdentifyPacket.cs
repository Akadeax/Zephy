using System;
using System.Collections.Generic;
using System.Text;

namespace Packets.General
{
    class IdentifyPacket : Packet
    {
        public const int TYPE = 1000;

        string src = "";

        public IdentifyPacket(string src)
            : base(Convert.ToUInt16(BASE_PACKET_SIZE + src.Length), TYPE)
        {
            Src = src;
        }

        public IdentifyPacket(byte[] packet)
            : base(packet) { }

        public string Src
        {
            get 
            {
                return ReadString(BASE_PACKET_SIZE, Buffer.Length - BASE_PACKET_SIZE);
            }
            private set
            {
                src = value;
                WriteString(value, BASE_PACKET_SIZE);
            }
        }
    }
}
