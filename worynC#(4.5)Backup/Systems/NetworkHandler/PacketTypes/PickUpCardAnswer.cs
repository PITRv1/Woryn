using Godot;
using System;
using System.Collections.Generic;

public partial class PickUpCardAnswer : PacketInfo
{
	public PointCard[] PointCards;
	public ModifierCard[] ModifierCards;
	
	public PickUpCardAnswer()
	{
		PacketType = PACKET_TYPES.PICK_UP_CARD_ANSWER;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.Add((byte)PointCards.Length);

		foreach (PointCard card in PointCards)
		{
			data.AddRange(BitConverter.GetBytes(card.PointValue));
		}

		data.Add((byte)ModifierCards.Length);
		foreach (ModifierCard card in ModifierCards)
		{
			data.Add((byte)ModifierCardTypeConverter.ClassToType(card));
			if (card is ModifierCardMultiplier or ModifierCardAddition)
			{
				var cardModif = (dynamic)card;
				data.AddRange(BitConverter.GetBytes(cardModif.Amount));
			}
		}

		return data.ToArray();
    }

	public static PickUpCardAnswer CreateFromData(byte[] data)
	{
		PickUpCardAnswer packet = new PickUpCardAnswer();
		int index = 1;

		int PointCardsLength = data[index];
		index += 1;

		packet.PointCards = new PointCard[PointCardsLength];
		for (int i = 0; i < PointCardsLength; i++)
		{
			int pointValue = BitConverter.ToInt32(data, index);
			index += 4;
			packet.PointCards[i] = new PointCard(pointValue);
		}

		int modifierLength = data[index];
		index += 1;

		packet.ModifierCards = new ModifierCard[modifierLength];
		for (int i = 0; i < modifierLength; i++)
		{
			MODIFIER_TYPES modifierType = (MODIFIER_TYPES)data[index];
			index += 1;

			ModifierCard card = ModifierCardTypeConverter.TypeToClass(modifierType);
			if (card is ModifierCardMultiplier or ModifierCardAddition)
			{
				var cardModif = (dynamic)card;
				cardModif.Amount = BitConverter.ToInt32(data, index);
				index += 4;
			}

			packet.ModifierCards[i] = card;
		}

		return packet;
	}
}
