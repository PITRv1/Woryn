using Godot;
using System;
using System.Collections.Generic;

public class ShopItems : PacketInfo
{
	public ItemType[] ItemTypes;
	public MODIFIER_TYPES[] ModifierTypes;
	
	public ShopItems()
	{
		PacketType = PACKET_TYPES.SHOP_ITEMS;
	}
	
	public override byte[] Encode()
	{
		var data = new List<byte>();

		data.Add((byte)PacketType);

		data.Add((byte)ItemTypes.Length);

		foreach (var item in ItemTypes)
		{
			data.AddRange(BitConverter.GetBytes((int)item));
		}

		data.Add((byte)ModifierTypes.Length);
		
		foreach (var modifier in ModifierTypes)
		{
			data.AddRange(BitConverter.GetBytes((int)modifier));
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
		for (var i = 0; i < itemTypesLength; i++)
		{
			packet.ItemTypes[i] = (ItemType)BitConverter.ToInt32(data, index);
			index += 4;
		}

		int modifierLength = data[index];
		index += 1;

		packet.ModifierTypes = new MODIFIER_TYPES[modifierLength];
		for (var i = 0; i < modifierLength; i++)
		{
			packet.ModifierTypes[i] = (MODIFIER_TYPES)BitConverter.ToInt32(data, index);
			index += 4;
		}

		return packet;
	}
}
