using Godot;
using System;
using System.Collections.Generic;

public partial class ShopPlayer : Node
{
	private int ID;
	private double _cursorTimer = 0;
	private const double CURSOR_SEND_RATE = 0.05; // 20Hz
	Dictionary<int, Control> cursors = new();
	[Export] PackedScene cursor;

    public override void _Ready()
    {
        ID = Global.multiplayerClientGlobals._id;
		Global.multiplayerClientGlobals.CursonUpdate += UpdateCursor;
		CreateCursors();
    }

	private void CreateCursors()
	{
		int[] ids = Global.multiplayerClientGlobals._remoteIds.ToArray();

		foreach (int id in ids)
		{
			if (cursors.Keys.Contains(id))
				continue;
			Control newCursor = cursor.Instantiate() as Control;
			cursors.Add(id, newCursor);
			GetParent().CallDeferred("add_child", newCursor);
		}
	}

	public override void _Process(double delta)
	{
		_cursorTimer += delta;

		if (_cursorTimer >= CURSOR_SEND_RATE)
		{
			_cursorTimer = 0;

			CursorUpdatePacket packet = new()
			{
				PlayerId = ID,
				Position = GetViewport().GetMousePosition()
			};

			Global.networkHandler._serverPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagUnsequenced);

			cursors[ID].Position = GetViewport().GetMousePosition();
		}
	}

	private void UpdateCursor(byte[] data)
	{
		CursorUpdatePacket packet = CursorUpdatePacket.CreateFromData(data);
		cursors[packet.PlayerId].Position = packet.Position;
	}
}
