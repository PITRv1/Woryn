using Godot;
using System;
using System.Collections.Generic;

public partial class TurnInfoPacket : PacketInfo
{
	public int LastPlayer;
	public int CurrentPlayerId;
	public int CurrentRound;
	public int MaxValue;
	public int ThrowDeckValue;
	public int CurrentPointValue;
	public int[] DeletePointCards;
	public byte[] DeleteModifierCards;
	public TurnInfoPacket()
	{
		PacketType = PACKET_TYPES.TURN_INFO;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(LastPlayer));

		data.AddRange(BitConverter.GetBytes(CurrentPlayerId));

		data.AddRange(BitConverter.GetBytes(CurrentRound));

		data.AddRange(BitConverter.GetBytes(MaxValue));

		data.AddRange(BitConverter.GetBytes(ThrowDeckValue));

		data.AddRange(BitConverter.GetBytes(CurrentPointValue));

		data.AddRange(BitConverter.GetBytes(DeletePointCards.Length));

		foreach (int index in DeletePointCards)
		{
			data.AddRange(BitConverter.GetBytes(index));
		}

		data.AddRange(BitConverter.GetBytes(DeleteModifierCards.Length));

		foreach (byte index in DeleteModifierCards)
			data.Add(index);

		return data.ToArray();
    }

	public static TurnInfoPacket CreateFromData(byte[] data)
	{
		TurnInfoPacket packet = new TurnInfoPacket();
		int index = 1;

		packet.LastPlayer = BitConverter.ToInt32(data, index);
		index += 4;

		packet.CurrentPlayerId = BitConverter.ToInt32(data, index);
		index += 4;

		packet.CurrentRound = BitConverter.ToInt32(data, index);
		index += 4;

		packet.MaxValue = BitConverter.ToInt32(data, index);
		index += 4;

		packet.ThrowDeckValue = BitConverter.ToInt32(data, index);
		index += 4;

		packet.CurrentPointValue = BitConverter.ToInt32(data, index);
		index += 4;

		int size = BitConverter.ToInt32(data, index); 
		index += 4;
		packet.DeletePointCards = new int[size];

		for (int i = 0; i < size; i++)
		{
			packet.DeletePointCards[i] = BitConverter.ToInt32(data, index);
			index += 4;
		}

		size = BitConverter.ToInt32(data, index);
		index += 4;
		packet.DeleteModifierCards = new byte[size];

		for (int i = 0; i < size; i++)
		{
			packet.DeleteModifierCards[i] = data[index];
			index++;
		}

		return packet;
	}


}
