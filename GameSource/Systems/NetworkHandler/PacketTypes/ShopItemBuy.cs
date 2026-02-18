using Godot;
using System;
using System.Collections.Generic;

public partial class ShopItemBuy : PacketInfo
{
    public int SenderId;
    public int CardIndex;
    public float GoldAmount;
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

        data.AddRange(BitConverter.GetBytes(GoldAmount));

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
        index += 4;

        packet.GoldAmount = BitConverter.ToSingle(data, index);
		return packet;
	}
}
