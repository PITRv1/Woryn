using Godot;
using System;
using System.Collections.Generic;

public class ShopSceneChange : PacketInfo
{
	public ShopSceneChange()
	{
		PacketType = PACKET_TYPES.SHOP_SCENE_CHANGE;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		return data.ToArray();
    }

	public static ShopSceneChange CreateFromData(byte[] data)
	{
		ShopSceneChange packet = new ShopSceneChange();

		return packet;
	}
}
