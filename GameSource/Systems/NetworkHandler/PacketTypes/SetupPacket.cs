using Godot;
using System;
using System.Collections.Generic;

public partial class SetupPacket : PacketInfo
{
	public int PlayerCount;
	public int StarterPlayer;
	public SetupPacket()
	{
		PacketType = PACKET_TYPES.SETUP_PLACE;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(PlayerCount));

		data.AddRange(BitConverter.GetBytes(StarterPlayer));

		return data.ToArray();
    }

	public static SetupPacket CreateFromData(byte[] data)
	{
		SetupPacket packet = new SetupPacket();

		int index = 1;

		packet.PlayerCount = BitConverter.ToInt32(data, index);
		index += 4;

		packet.StarterPlayer = BitConverter.ToInt32(data, index);

		return packet;
	}
}
