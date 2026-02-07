using Godot;
using System;
using System.Collections.Generic;

public class StartGame : PacketInfo
{
	public byte senderId;
	public int playerCount;

	public StartGame()
	{
		PacketType = PACKET_TYPES.START_GAME;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.Add(senderId);

		data.AddRange(BitConverter.GetBytes(playerCount));
		
		return data.ToArray();
    }

	public static StartGame CreateFromData(byte[] data)
	{
		StartGame packet = new StartGame();
		int index = 1;

		packet.senderId = data[index];
		index++;

		packet.playerCount = BitConverter.ToInt32(data, index);

		return packet;
	}
}
