using Godot;
using System;
using System.Collections.Generic;

public class LookAtPacket : PacketInfo
{
	public int PlayerId;
	public Vector3 TargetPosition;
	public LookAtPacket()
	{
		PacketType = PACKET_TYPES.LOOK_AT_PACKET;
	}

	public override byte[] Encode()
	{
		var data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(PlayerId));

		data.AddRange(BitConverter.GetBytes(TargetPosition.X));
		data.AddRange(BitConverter.GetBytes(TargetPosition.Y));
		data.AddRange(BitConverter.GetBytes(TargetPosition.Z));
		
		
		return data.ToArray();
	}

	public static LookAtPacket CreateFromData(byte[] data)
	{
		var packet = new LookAtPacket();

		var index = 1;

		packet.PlayerId = BitConverter.ToInt32(data, index);
		
		index += 4;
		
		var X = BitConverter.ToSingle(data, index);
		index += 4;
		var Y = BitConverter.ToSingle(data, index);
		index += 4;
		var Z = BitConverter.ToSingle(data, index);
		
		packet.TargetPosition = new Vector3(X, Y, Z);
		
		return packet;
	}
}
