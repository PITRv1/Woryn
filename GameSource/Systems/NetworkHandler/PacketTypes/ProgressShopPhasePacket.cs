using Godot;
using System;
using System.Collections.Generic;

public partial class ProgressShopPhasePacket : PacketInfo
{
	public int ShopPhase;
    
	public ProgressShopPhasePacket()
	{
		PacketType = PACKET_TYPES.PROGRESS_SHOP_PHASE;
	}
	
	public override byte[] Encode()
	{
		var data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(ShopPhase));

		return data.ToArray();
	}
	
	public static ProgressShopPhasePacket CreateFromData(byte[] data)
	{
		var packet = new ProgressShopPhasePacket();
		var index = 1;

		packet.ShopPhase = BitConverter.ToInt32(data, index);

		return packet;
	}
}
