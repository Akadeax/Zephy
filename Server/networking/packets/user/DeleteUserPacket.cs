using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packets.User
{
    public class DeleteUserPacketData
    {
        public string toDeleteId;

        public DeleteUserPacketData(string toDeleteId)
        {
            this.toDeleteId = toDeleteId.ToString();
        }
    }

    class DeleteUserPacket : Packet
    {
        public const int TYPE = 2101;

        public DeleteUserPacket(DeleteUserPacketData data) : base(TYPE)
        {
            Data = data;
        }

        public DeleteUserPacket(byte[] packet)
            : base(packet) { }

        public DeleteUserPacketData Data
        {
            get { return ReadJsonObject<DeleteUserPacketData>(); }
            set { WriteJsonObject(value); }
        }


        public ObjectId ToDeleteId
        {
            get { return new ObjectId(Data.toDeleteId); }
        }
    }
}
