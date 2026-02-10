using Godot;
using System;
using System.Collections.Generic;

public partial class RoundSuccessPacket : PacketInfo
{
	public int PlayerId;
	public RoundSuccessPacket()
	{
		PacketType = PACKET_TYPES.ROUND_SUCCESS;
	}

	public override byte[] Encode()
	{
		var data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(PlayerId));

		return data.ToArray();
	}

	public static RoundSuccessPacket CreateFromData(byte[] data)
	{
		var packet = new RoundSuccessPacket
		{
			PlayerId = BitConverter.ToInt32(data, 1)
		};

		return packet;
	}
}
