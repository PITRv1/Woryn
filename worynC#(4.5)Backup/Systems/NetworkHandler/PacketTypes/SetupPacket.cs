using Godot;
using System;
using System.Collections.Generic;

public partial class SetupPacket : PacketInfo
{
	public int PlayerCount;
	public SetupPacket()
	{
		PacketType = PACKET_TYPES.SETUP_PLACE;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(PlayerCount));

		return data.ToArray();
    }

	public static SetupPacket CreateFromData(byte[] data)
	{
		SetupPacket packet = new SetupPacket();

		int index = 1;

		packet.PlayerCount = BitConverter.ToInt32(data, index);

		return packet;
	}
}
