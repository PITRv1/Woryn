using Godot;
using System;
using System.Collections.Generic;

public class ShopItems : PacketInfo
{
	public ItemType[] ItemTypes;
	public MODIFIER_TYPES[] ModifierTypes;
	public List<int> itemPrices;
	public List<int> modifierPrices;
	
	public ShopItems()
	{
		PacketType = PACKET_TYPES.SHOP_ITEMS;
	}
	
	public override byte[] Encode()
	{
		var data = new List<byte>();

		data.Add((byte)PacketType);

		data.Add((byte)ItemTypes.Length);

		for (var i = 0; i < ItemTypes.Length; i++)
		{
			data.AddRange(BitConverter.GetBytes((int)ItemTypes[i]));
			data.AddRange(BitConverter.GetBytes(itemPrices[i]));
		}

		data.Add((byte)ModifierTypes.Length);
		
		for (var i = 0; i < ModifierTypes.Length; i++)
		{
			data.AddRange(BitConverter.GetBytes((int)ModifierTypes[i]));
			data.AddRange(BitConverter.GetBytes(modifierPrices[i]));
		}

		return data.ToArray();
	}
	
	public static ShopItems CreateFromData(byte[] data)
	{
		var packet = new ShopItems();
		var index = 1;

		int itemTypesLength = data[index];
		index += 1;

		packet.ItemTypes = new ItemType[itemTypesLength];
		packet.itemPrices = new List<int>();
		for (var i = 0; i < itemTypesLength; i++)
		{
			packet.ItemTypes[i] = (ItemType)BitConverter.ToInt32(data, index);
			index += 4;
			packet.itemPrices.Add(BitConverter.ToInt32(data, index));
			index += 4;
		}

		int modifierLength = data[index];
		index += 1;

		packet.ModifierTypes = new MODIFIER_TYPES[modifierLength];
		packet.modifierPrices = new List<int>();
		for (var i = 0; i < modifierLength; i++)
		{
			packet.ModifierTypes[i] = (MODIFIER_TYPES)BitConverter.ToInt32(data, index);
			index += 4;
			packet.modifierPrices.Add(BitConverter.ToInt32(data, index));
			index += 4;
		}

		return packet;
	}
}
