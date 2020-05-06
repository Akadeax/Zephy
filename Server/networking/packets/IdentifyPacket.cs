using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class IdentifyPacket : Packet
    {
        public const int TYPE = 1100;

        string src = "";

        public IdentifyPacket(string src)
            : base(Convert.ToUInt16(4 + src.Length), TYPE)
        {
            Src = src;
        }

        public IdentifyPacket(byte[] packet)
            : base(packet) { }

        public string Src
        {
            get 
            {
                return ReadString(4, Buffer.Length - 4);
            }
            set
            {
                src = value;
                WriteString(value, 4);
            }
        }
    }
}
