using Godot;
using System;
using System.Collections.Generic;

public partial class PlayAbility : PacketInfo
{
	public int SenderId;
	
	public PlayAbility()
	{
		PacketType = PACKET_TYPES.PLAY_ABLITIY;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(SenderId));

		return data.ToArray();
    }

	public static PlayAbility CreateFromData(byte[] data)
	{
		PlayAbility packet = new PlayAbility();
		int index = 1;

		packet.SenderId = BitConverter.ToInt32(data, index);

		return packet;
	}
}
