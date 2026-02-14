using Godot;
using System;
using System.Collections.Generic;

public partial class ShopItemBuy : PacketInfo
{
    public int SenderId;
    public int CardIndex;
    public byte IsPublicShop;
    
	public ShopItemBuy()
    {
        PacketType = PACKET_TYPES.SHOP_ITEM_BUY;
    }
	
	public override byte[] Encode()
	{
		var data = new List<byte>();

		data.Add((byte)PacketType);

        data.AddRange(BitConverter.GetBytes(SenderId));

        data.AddRange(BitConverter.GetBytes(CardIndex));

        data.Add(IsPublicShop);

		return data.ToArray();
	}
	
	public static ShopItemBuy CreateFromData(byte[] data)
	{
		var packet = new ShopItemBuy();
		var index = 1;

        packet.SenderId = BitConverter.ToInt32(data, index);
        index += 4;

        packet.CardIndex = BitConverter.ToInt32(data, index);
		return packet;
	}
}
