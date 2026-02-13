using Godot;
using System;
using System.Collections.Generic;

public partial class TurnInfoPacket : PacketInfo
{
	public int LastPlayer;
	public int CurrentPlayerId;
	public int CurrentRound;
	public int MaxValue;
	public int ThrowDeckValue;
	public int CurrentPointValue;
	
	public TurnInfoPacket()
	{
		PacketType = PACKET_TYPES.TURN_INFO;
	}

	public override byte[] Encode()
    {
        List<byte> data = new List<byte>();

		data.Add((byte)PacketType);

		data.AddRange(BitConverter.GetBytes(LastPlayer));

		data.AddRange(BitConverter.GetBytes(CurrentPlayerId));

		data.AddRange(BitConverter.GetBytes(CurrentRound));

		data.AddRange(BitConverter.GetBytes(MaxValue));

		data.AddRange(BitConverter.GetBytes(ThrowDeckValue));

		data.AddRange(BitConverter.GetBytes(CurrentPointValue));

		

		return data.ToArray();
    }

	public static TurnInfoPacket CreateFromData(byte[] data)
	{
		var packet = new TurnInfoPacket();
		var index = 1;

		packet.LastPlayer = BitConverter.ToInt32(data, index);
		index += 4;

		packet.CurrentPlayerId = BitConverter.ToInt32(data, index);
		index += 4;

		packet.CurrentRound = BitConverter.ToInt32(data, index);
		index += 4;

		packet.MaxValue = BitConverter.ToInt32(data, index);
		index += 4;

		packet.ThrowDeckValue = BitConverter.ToInt32(data, index);
		index += 4;

		packet.CurrentPointValue = BitConverter.ToInt32(data, index);
		index += 4;


		return packet;
	}


}
