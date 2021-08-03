using Server;
using Server.Database.Channel;
using Server.Database.User;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.channel
{
    public class ModifyChannelResponsePacketData : PacketData
    {
        public int httpStatus;
        public string channel;
        public int action;
        public string data;

        public ModifyChannelResponsePacketData(int httpStatus, string channel, int action, string data)
        {
            this.httpStatus = httpStatus;
            this.channel = channel;
            this.action = action;
            this.data = data;
        }
    }

    class ModifyChannelResponsePacket : Packet
    {
        public const int TYPE = 3008;

        public ModifyChannelResponsePacket(ModifyChannelResponsePacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public ModifyChannelResponsePacket(byte[] packet)
            : base(packet) { }

        public ModifyChannelResponsePacketData Data
        {
            get { return ReadJsonObject<ModifyChannelResponsePacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
