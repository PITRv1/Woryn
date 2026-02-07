using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LobbyPlayer : Node
{
    public int ID;
    List<int> ids = new();

	[Export] PackedScene lobbyPlayer;
	[Export] PackedScene scene;
	[Export] VBoxContainer lobbyBackground;

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
        ID = id;
    }

    private void Remote(int id)
    {
        ids.Add(id);
		
    }

	private void AddNewPlayer(byte[] data)
	{
		NewPlayer packet = NewPlayer.CreateFromData(data);

		for (int i = lobbyBackground.GetChildCount() - 1; i >= 0; i--)
		{
			lobbyBackground.RemoveChild(lobbyBackground.GetChild(i));
		}

		foreach (byte player in packet.playerArray)
		{
			PlayerFein newLobbyPlayer = lobbyPlayer.Instantiate() as PlayerFein;
			newLobbyPlayer.SetText(player.ToString());
			lobbyBackground.AddChild(newLobbyPlayer);
		}
	}

	public void StartGameRequest()
	{
		StartGame packet = new StartGame
		{
			senderId = (byte)ID,
		};
		Global.networkHandler._serverPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	public void StartGame()
	{
		GetTree().ChangeSceneToPacked(scene);
	}

	public void PrintIds()
	{
		foreach (int id in ids)
		{
			GD.Print(id);
		}
	}
}
