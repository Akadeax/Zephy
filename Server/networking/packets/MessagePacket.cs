using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class MessagePacket : Packet
    {
        public const int TYPE = 1200;

        private string message;

        public MessagePacket(string message)
            : base(Convert.ToUInt16(4 + message.Length), TYPE)
        {
            Message = message;
        }

        public MessagePacket(byte[] packet)
            : base(packet) { }

        public string Message
        {
            get
            {
                return ReadString(4, Buffer.Length - 4);
            }
            set
            {
                message = value;
                WriteString(value, 4);
            }
        }
    }
}
