using Server;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.general
{
    public class IdentifyPacketData : PacketData
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
            var data = packet.Data;
            if (data == null) return;

            IPEndPoint ep = sender.LocalEndPoint as IPEndPoint;
            Zephy.Logger.Information($"Received TCP Identify from {ep.Address} as {data.src}, sending back.");
            Zephy.serverSocket.SendPacket(new IdentifyPacket(new IdentifyPacketData("SERVER")), sender);
        }
    }

    class IdentifyPacket : Packet
    {
        public const int TYPE = 1001;

        public IdentifyPacket(IdentifyPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public IdentifyPacket(byte[] packet)
            : base(packet) { }

        public IdentifyPacketData Data
        {
            get { return ReadJsonObject<IdentifyPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
