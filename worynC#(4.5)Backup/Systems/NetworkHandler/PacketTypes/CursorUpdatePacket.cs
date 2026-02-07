using Godot;
using System;
using System.Collections.Generic;

public class CursorUpdatePacket : PacketInfo
{
    public int PlayerId;
    public Vector2 Position;

	public CursorUpdatePacket()
	{
		PacketType = PACKET_TYPES.CURSOR_UPDATE;
	}

    public override byte[] Encode()
    {
        List<byte> data = new();
        data.Add((byte)PACKET_TYPES.CURSOR_UPDATE);

        data.AddRange(BitConverter.GetBytes(PlayerId));
        data.AddRange(BitConverter.GetBytes(Position.X));
        data.AddRange(BitConverter.GetBytes(Position.Y));

        return data.ToArray();
    }

    public static CursorUpdatePacket CreateFromData(byte[] data)
    {
        CursorUpdatePacket packet = new CursorUpdatePacket();
		int index = 1;

		packet.Position = new Vector2();

		packet.PlayerId = BitConverter.ToInt32(data, index);
		index += 4;

		packet.Position.X = BitConverter.ToSingle(data, index);
		index += 4;

		packet.Position.Y = BitConverter.ToSingle(data, index);

		return packet;
    }
}
