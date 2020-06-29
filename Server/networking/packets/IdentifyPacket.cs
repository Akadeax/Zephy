using System;
using System.Collections.Generic;
using System.Text;

namespace Packets.General
{
    public class IdentifyPacketData
    {
        public string src;

        public IdentifyPacketData(string src)
        {
            this.src = src;
        }
    }

    class IdentifyPacket : Packet
    {
        public const int TYPE = 1000;

        public IdentifyPacket(IdentifyPacketData data) : base(TYPE)
        {
            Data = data;
        }

        public IdentifyPacket(byte[] packet)
            : base(packet) { }

        public IdentifyPacketData Data
        {
            get
            {
                return ReadJsonObject<IdentifyPacketData>();
            }
            set
            {
                WriteJsonObject(value);
            }
        }


        public string Src
        {
            get 
            {
                return Data.src;
            }
        }
    }
}
