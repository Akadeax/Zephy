using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Server.database.user;

namespace Packets.auth
{
    class LoginResultPacketData : PacketData
    {
        public int statusCode;
        public PopulatedUser user;

        public LoginResultPacketData(int statusCode, PopulatedUser user)
        {
            this.statusCode = statusCode;
            this.user = user;
        }
        
    }

    class LoginResultPacketHandler : PacketHandler<LoginResultPacket>
    {
        protected override void Handle(LoginResultPacket packet, Socket sender) { }
    }

    class LoginResultPacket : Packet
    {
        public const int TYPE = 2002;

        public LoginResultPacket(LoginResultPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public LoginResultPacket(byte[] packet)
            : base(packet) { }


        public LoginResultPacketData Data
        {
            get { return ReadJsonObject<LoginResultPacketData>(); }
            set { WriteJsonObject(value); }
        }

    }
}
