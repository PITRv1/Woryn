using Godot;
using System;
using System.Collections.Generic;

public partial class RoundSuccessPacket : PacketInfo
{
	public int PlayerId;
	public int[] DeletePointCards;
	public byte[] DeleteModifierCards;
	public RoundSuccessPacket()
	{
		PacketType = PACKET_TYPES.ROUND_SUCCESS;
	}

	public override byte[] Encode()
	{
		var data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(PlayerId));
		
		data.AddRange(BitConverter.GetBytes(DeletePointCards.Length));

		foreach (var index in DeletePointCards)
		{
			data.AddRange(BitConverter.GetBytes(index));
		}

		data.AddRange(BitConverter.GetBytes(DeleteModifierCards.Length));

		data.AddRange(DeleteModifierCards);

		return data.ToArray();
	}

	public static RoundSuccessPacket CreateFromData(byte[] data)
	{
		var packet = new RoundSuccessPacket();
		var index = 1;
		
		packet.PlayerId = BitConverter.ToInt32(data, index);
		index += 4;
		
		var size = BitConverter.ToInt32(data, index); 
		index += 4;
		packet.DeletePointCards = new int[size];

		for (var i = 0; i < size; i++)
		{
			packet.DeletePointCards[i] = BitConverter.ToInt32(data, index);
			index += 4;
		}

		size = BitConverter.ToInt32(data, index);
		index += 4;
		packet.DeleteModifierCards = new byte[size];

		for (var i = 0; i < size; i++)
		{
			packet.DeleteModifierCards[i] = data[index];
			index++;
		}

		return packet;
	}
}
