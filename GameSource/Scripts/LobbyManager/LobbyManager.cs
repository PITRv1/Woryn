using Godot;
using System;
using System.Collections.Generic;

public partial class LobbyManager
{
	private List<byte> players;
	public LobbyManager()
	{
		players = new List<byte>();
		Global.lobbyManagerInstance = this;
	}

	public void AddToMultiplayerList(int id)
	{
		players.Add((byte)id);

		NewPlayer packet = new NewPlayer
		{
			playerArray = players.ToArray(),
		};
		
		foreach (int player in players)
		{
			Global.networkHandler.ClientPeers.TryGetValue(player, out var peer);

			if (peer != null)
				packet.Send(peer);
		}
	}

	public void StartGameRequest(byte[] data)
	{	
		StartGame packet = new StartGame
		{
			senderId = 0,
			playerCount = players.Count
		};

		foreach (int player in players)
		{
			Global.networkHandler.ClientPeers.TryGetValue(player, out var peer);

			if (peer != null)
				packet.Send(peer);
		}
	}
}
