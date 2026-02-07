using Godot;
using System;
using System.Collections.Generic;

public class NewPlayer : PacketInfo
{
	public byte[] playerArray;

	public NewPlayer()
	{
		PacketType = PACKET_TYPES.NEW_PLAYER;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.Add((byte)playerArray.Length);
		
		foreach (byte player in playerArray)
			data.Add(player);

		return data.ToArray();
    }

	public static NewPlayer CreateFromData(byte[] data)
	{
		NewPlayer packet = new NewPlayer();
		int index = 1;

		int size = data[index];
		index += 1;

		packet.playerArray = new byte[size];
		for (int i = 0; i < size; i++)
		{
			packet.playerArray[i] = data[index];
			index += 1;
		}

		return packet;
	}
}
