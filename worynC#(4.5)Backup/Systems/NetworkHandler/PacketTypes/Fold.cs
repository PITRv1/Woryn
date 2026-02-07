using Godot;
using System;
using System.Collections.Generic;

public partial class Fold : PacketInfo
{
	public int SenderId;
	
	public Fold()
	{
		PacketType = PACKET_TYPES.FOLD;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(SenderId));

		return data.ToArray();
    }

	public static Fold CreateFromData(byte[] data)
	{
		Fold packet = new Fold();
		int index = 1;

		packet.SenderId = BitConverter.ToInt32(data, index);

		return packet;
	}
}
