using Godot;
using System;
using System.Collections.Generic;

public class ClientReady : PacketInfo
{
	public ClientReady()
	{
		PacketType = PACKET_TYPES.CLIENT_READY;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		return data.ToArray();
    }

	public static StartGame CreateFromData(byte[] data)
	{
		StartGame packet = new StartGame();

		return packet;
	}
}
