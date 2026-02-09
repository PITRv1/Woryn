using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LobbyPlayer : Node
{
    private int _id;
    private readonly List<int> _ids = [];

	[Export] private PackedScene _lobbyPlayer;
	[Export] private PackedScene _scene;
	[Export] private VBoxContainer _lobbyBackground;

    public override void _Ready()
    {
        Global.multiplayerClientGlobals.HandleLocalIdAssignment += Local;
        Global.multiplayerClientGlobals.HandleRemoteIdAssignment += Remote;
        Global.networkHandler.OnPeerConnected += Remote;

		Global.multiplayerClientGlobals.NewPlayer += AddNewPlayer;
		Global.multiplayerClientGlobals.StartGame += StartGame;
    }

    private void Local(int id)
    {
        _id = id;
    }

    private void Remote(int id)
    {
        _ids.Add(id);
		
    }

	private void AddNewPlayer(byte[] data)
	{
		var packet = NewPlayer.CreateFromData(data);

		for (var i = _lobbyBackground.GetChildCount() - 1; i >= 0; i--)
			_lobbyBackground.RemoveChild(_lobbyBackground.GetChild(i));

		foreach (var player in packet.playerArray)
		{
			var newLobbyPlayer = _lobbyPlayer.Instantiate() as LobbyPlayerPlate;
			newLobbyPlayer.SetText(player.ToString());
			_lobbyBackground.AddChild(newLobbyPlayer);
		}
	}

	private void StartGameRequest()
	{
		var packet = new StartGame
		{
			senderId = (byte)_id,
		};
		Global.networkHandler.ServerPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	private void StartGame()
	{
		GetTree().ChangeSceneToPacked(_scene);
	}

	private void PrintIds()
	{
		foreach (var id in _ids)
			GD.Print(id);
	}
}
