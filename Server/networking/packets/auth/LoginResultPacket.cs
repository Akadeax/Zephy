using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace Packets.Auth
{
    class LoginResultPacketData
    {
        public int statusCode;

        public LoginResultPacketData(int statusCode)
        {
            this.statusCode = statusCode;
        }
        
    }

    class LoginResultPacketHandler : PacketHandler<LoginResultPacket>
    {
        protected override void Handle(LoginResultPacket packet, Socket sender) { }
    }

    class LoginResultPacket : Packet
    {
        public const int TYPE = 2002;

        public LoginResultPacket(LoginResultPacketData data) : base(TYPE)
        {
            Data = data;
        }

        public LoginResultPacket(byte[] packet)
            : base(packet) { }


        protected LoginResultPacketData Data
        {
            get { return ReadJsonObject<LoginResultPacketData>(); }
            set { WriteJsonObject(value); }
        }

    }
}
