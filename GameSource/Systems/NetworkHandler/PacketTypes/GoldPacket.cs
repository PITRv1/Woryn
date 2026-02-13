using Godot;
using System;
using System.Collections.Generic;

public class GoldPacket : PacketInfo
{
	public int SenderId;
	public int PointAmount;
	public int GoldAmount = 0;

	public GoldPacket()
	{
		PacketType = PACKET_TYPES.GOLD_CONVERT;
	}

	public override byte[] Encode()
	{
		var data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(SenderId));

		data.AddRange(BitConverter.GetBytes(PointAmount));
		
		data.AddRange(BitConverter.GetBytes(GoldAmount));
		
		return data.ToArray();
	}

	public static GoldPacket CreateFromData(byte[] data)
	{
		var packet = new GoldPacket();
		var index = 1;

		packet.SenderId = BitConverter.ToInt32(data, index);
		index += 4;

		packet.PointAmount = BitConverter.ToInt32(data, index);
		index += 4;
		
		packet.GoldAmount = BitConverter.ToInt32(data, index);

		return packet;
	}
}
