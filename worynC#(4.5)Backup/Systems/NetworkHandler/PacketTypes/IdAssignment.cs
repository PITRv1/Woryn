using Godot;
using System.Collections.Generic;

public partial class IDAssignment : PacketInfo
{
    public int Id;
    public List<int> RemoteIds = new();

    public static IDAssignment Create(int id, List<int> remoteIds)
    {
        IDAssignment info = new IDAssignment();

        info.PacketType = PACKET_TYPES.ID_ASSIGNMENT;
        info.Id = id;
        info.RemoteIds = remoteIds;

        return info;
    }

    public static IDAssignment CreateFromData(byte[] data)
    {
        IDAssignment info = new IDAssignment();
        info.Decode(data);
        return info;
    }

    public new byte[] Encode()
    {
        // 1 byte: packet type
        // 1 byte: id
        // N bytes: remote ids

        byte[] baseData = base.Encode();
        byte[] data = new byte[2 + RemoteIds.Count];

        data[0] = baseData[0];
        data[1] = (byte)Id;

        for (int i = 0; i < RemoteIds.Count; i++)
            data[2 + i] = (byte)RemoteIds[i];
        
        return data;
    }

    public new void Decode(byte[] data)
    {
        base.Decode(data);

        Id = data[1];
        RemoteIds.Clear();

        for (int i = 1; i < data.Length; i++)
            RemoteIds.Add(data[i]);   
    }

    public new void Broadcast(ENetConnection server)
    {
        server.Broadcast(0, Encode(), (int)Flag);
    }
}
