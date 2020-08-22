using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace Packets.Auth
{
    class LoginAcceptedPacketData
    {

        public LoginAcceptedPacketData()
        {

        }
    }

    class LoginAcceptedPacketHandler : PacketHandler<LoginAcceptedPacket>
    {
        protected override void Handle(LoginAcceptedPacket packet, Socket sender) { }
    }

    class LoginAcceptedPacket : Packet
    {
        public const int TYPE = 2002;

        public LoginAcceptedPacket(LoginAcceptedPacketData data) : base(TYPE)
        {
            Data = data;
        }

        public LoginAcceptedPacket(byte[] packet)
            : base(packet) { }


        protected LoginAcceptedPacketData Data
        {
            get { return ReadJsonObject<LoginAcceptedPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
