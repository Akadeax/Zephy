﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace Packets
{
    /* Top class makes it usable in contexts like 'List<PacketHandler>' without
       having to provide a generic type, which is needed in PacketReceiver.cs */
    public abstract class PacketHandler
    {
        public abstract void Handle(byte[] buffer, Socket sender);
    }

    public abstract class PacketHandler<TPacketType> : PacketHandler where TPacketType : Packet
    {
        protected abstract void Handle(TPacketType packet, Socket sender);

        public override void Handle(byte[] buffer, Socket sender)
        {
            // convert from byte[] to new instance of specified Packet
            // by calling T's constructor with buffer as argument
            object packet = Activator.CreateInstance(typeof(TPacketType), buffer);
            // cast is safe -> packet is always typeof(TPacketType)
            Handle(packet as TPacketType, sender);
        }
    }

    public class Packet
    {
        protected const byte BASE_PACKET_SIZE = 2;

        protected byte[] buffer;
        public byte[] Buffer { get => buffer; }

        protected Packet(ushort type)
        {
            buffer = new byte[BASE_PACKET_SIZE];
            WriteUShort(type, 0);
        }

        public Packet(byte[] packet)
        {
            buffer = packet;
        }


        public ushort PacketType
        {
            get
            {
                return ReadUShort(0);
            }
        }

        public static ushort GetPacketType(byte[] buffer)
        {
            return new Packet(buffer).PacketType;
        }

        protected ushort ReadUShort(int offset)
        {
            return BitConverter.ToUInt16(buffer, offset);
        }

        protected void WriteUShort(ushort value, int offset)
        {
            byte[] valueBuffer = BitConverter.GetBytes(value);
            Array.Copy(valueBuffer, 0, buffer, offset, valueBuffer.Length);
        }


        protected string ReadString(int offset, int count)
        {
            return Encoding.UTF8.GetString(buffer, offset, count);
        }

        protected void WriteString(string value, int offset)
        {
            byte[] valueBuffer = new byte[value.Length];
            valueBuffer = Encoding.UTF8.GetBytes(value);

            Array.Copy(valueBuffer, 0, buffer, offset, valueBuffer.Length);
        }


        protected T ReadJsonObject<T>()
        {
            string json = ReadString(BASE_PACKET_SIZE, buffer.Length - BASE_PACKET_SIZE);
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected void WriteJsonObject<T>(T toWrite)
        {
            string json = JsonConvert.SerializeObject(toWrite);
            int TARGET_LEN = BASE_PACKET_SIZE + json.Length;
            if (buffer.Length != TARGET_LEN)
            {
                byte[] newBuffer = new byte[TARGET_LEN];
                Array.Copy(buffer, 0, newBuffer, 0, BASE_PACKET_SIZE);
                buffer = newBuffer;
            }
            WriteString(json, BASE_PACKET_SIZE);
        }
    }
}
