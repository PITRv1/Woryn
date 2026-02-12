using Godot;
using System;
using System.Collections.Generic;

public partial class PlayAbility : PacketInfo
{
	public int SenderId;
	public ItemType Ability;
	
	public PlayAbility()
	{
		PacketType = PACKET_TYPES.PLAY_ABILITY;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(SenderId));
		
		data.AddRange(BitConverter.GetBytes((int)Ability));

		return data.ToArray();
    }

	public static PlayAbility CreateFromData(byte[] data)
	{
		var packet = new PlayAbility();
		var index = 1;

		packet.SenderId = BitConverter.ToInt32(data, index);

		index += 4;
		
		packet.Ability = (ItemType)BitConverter.ToInt32(data, index);

		return packet;
	}
}
