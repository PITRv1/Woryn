using Godot;
using System;
using System.Collections.Generic;

public partial class PickUpCardRequest : PacketInfo
{
	public int SenderId;

	public PickUpCardRequest()
	{
		PacketType = PACKET_TYPES.PICK_UP_CARD_REQUEST;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);
		data.AddRange(BitConverter.GetBytes(SenderId));

		return data.ToArray();
    }

	public static PickUpCardRequest CreateFromData(byte[] data)
	{
		PickUpCardRequest packet = new PickUpCardRequest();
		int index = 1;

		packet.SenderId = BitConverter.ToInt32(data, index);

		return packet;
	}
}
