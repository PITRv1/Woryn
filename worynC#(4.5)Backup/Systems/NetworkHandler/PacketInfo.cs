using Godot;
using System;

public abstract class PacketInfo
{
    public PACKET_TYPES PacketType;
    public long Flag = ENetPacketPeer.FlagReliable;

    public virtual byte[] Encode()
    {
        byte[] data = [(byte)PacketType];
        return data;
    }

    public void Decode(byte[] data)
    {
        PacketType = (PACKET_TYPES)data[0];
    }

    public void Send(ENetPacketPeer target)
    {
        target.Send(0, Encode(), (int)Flag);
    }

    public void Broadcast(ENetConnection server)
    {
        server.Broadcast(0, Encode(), (int)Flag);
    }
}
