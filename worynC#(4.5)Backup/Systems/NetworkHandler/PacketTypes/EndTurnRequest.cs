using Godot;
using System;
using System.Collections.Generic;

public partial class EndTurnRequest : PacketInfo
{
	public int SenderId;
	public PointCard PointCard;
	public int PointCardIndex;
	public ModifierCard[] ModifierCards;
	public byte[] ModifCardIndexes;
	public EndTurnRequest()
	{
		PacketType = PACKET_TYPES.END_TURN_REQUEST;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(SenderId));

		data.AddRange(BitConverter.GetBytes(PointCard.PointValue));

		data.AddRange(BitConverter.GetBytes(PointCardIndex));

		data.Add((byte)ModifierCards.Length);
		for (int i = 0; i < ModifierCards.Length; i++)
		{
			data.Add((byte)ModifierCardTypeConverter.ClassToType(ModifierCards[i]));
			data.Add(ModifCardIndexes[i]);
		}

		return data.ToArray();
    }

	public static EndTurnRequest CreateFromData(byte[] data)
	{
		EndTurnRequest packet = new EndTurnRequest();
		int index = 1;

		packet.SenderId = BitConverter.ToInt32(data, index);
		index += 4;

		packet.PointCard = new PointCard(BitConverter.ToInt32(data, index));
		index += 4;

		packet.PointCardIndex = BitConverter.ToInt32(data, index); 
		index += 4;

		int modifierLength = data[index];
		index += 1;

		packet.ModifierCards = new ModifierCard[modifierLength];
		packet.ModifCardIndexes = new byte[modifierLength];
		for (int i = 0; i < modifierLength; i++)
		{
			MODIFIER_TYPES modifierType = (MODIFIER_TYPES)data[index];
			index += 1;
			packet.ModifierCards[i] = ModifierCardTypeConverter.TypeToClass(modifierType);

			packet.ModifCardIndexes[i] = data[index];
			index += 1;
		}

		return packet;
	}
}
