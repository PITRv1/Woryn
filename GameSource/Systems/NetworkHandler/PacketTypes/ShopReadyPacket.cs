using Godot;
using System;
using System.Collections.Generic;

public partial class ShopReadyPacket : PacketInfo
{
	public int SenderId;
	public ShopReadyPacket()
	{
		PacketType = PACKET_TYPES.SHOP_READY;
	}

	public override byte[] Encode()
	{
		List<byte> data = new List<byte>();

		data.Add((byte)PacketType);
		
		data.AddRange(BitConverter.GetBytes(SenderId));

		return data.ToArray();
	}

	public static ShopReadyPacket CreateFromData(byte[] data)
	{
		var packet = new ShopReadyPacket();

		packet.SenderId = BitConverter.ToInt32(data, 1);

		return packet;
	}
}
