using Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Packets.general
{
    public class IdentifyPacketData
    {
        public string src;

        public IdentifyPacketData(string src)
        {
            this.src = src;
        }
    }

    class IdentifyPacketHandler : PacketHandler<IdentifyPacket>
    {
        protected override void Handle(IdentifyPacket packet, Socket sender)
        {
            IPEndPoint ep = sender.LocalEndPoint as IPEndPoint;
            Zephy.Logger.Information($"Received TCP Identify from {ep.Address} as {packet.Src}, sending back in 1 second.");
            Zephy.Logger.Debug("Received!");
            Thread.Sleep(1000);
            Zephy.Logger.Debug("Done sleeping!");
            sender.Send(new IdentifyPacket(new IdentifyPacketData("SERVER")).Buffer);
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

        protected IdentifyPacketData Data
        {
            get { return ReadJsonObject<IdentifyPacketData>(); }
            set { WriteJsonObject(value); }
        }


        public string Src
        {
            get { return Data.src; }
        }
    }
}
