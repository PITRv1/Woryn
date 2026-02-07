using Godot;
using System;

public partial class Multiplayer : Control
{
	[Export] MainUI mainUI;
	[Export] Control multiTypeMenu;
	[Export] Control multiHostMenu;
	[Export] Control multiJoinMenu;
	[Export] Control playerListMenu;
	[Export] LineEdit gameNameText;
	[Export] HSlider numberOfPlayersValue;
    [Export] ButtonGroup optionGroup;
	[Export] LineEdit ipAddressInputServer;
	[Export] LineEdit ipAddressInputClient;


	private Control _currentMenu;
	public Control CurrentMenu {
		get
		{
			return _currentMenu;
		}
		set {
			if (_currentMenu != null) _currentMenu.Visible = false;
			_currentMenu = value;
			_currentMenu.Visible = true;
		}
	}


    public override void _Ready()
    {
        CurrentMenu = multiTypeMenu;

		// Global.multiplayerClientGlobals.HandleLocalIdAssignment += Local;
        // Global.multiplayerClientGlobals.HandleRemoteIdAssignment += Remote;
		// Global.networkHandler.OnPeerDisconnected += HandleDisconnect;

    }

	public void ChangeMenu(string option)
	{
		switch(option)
		{
			case "host":
				CurrentMenu = multiHostMenu;
				break;
			case "join":
				CurrentMenu = multiJoinMenu;
				break;
			case "select":
				CurrentMenu = multiTypeMenu;
				break;
			case "main":
				mainUI.CurrentMenu = MainUI.MENUS.MAIN;
				
				CurrentMenu = multiTypeMenu;
				break;
			case "player":
				CurrentMenu = playerListMenu;
				break;
			case "select&stop":
				ChangeMenu("select");
				Global.networkHandler.DisconnectClient();
				if (Global.networkHandler._isServer) Global.networkHandler.StopServer();
				break;
		}
	}

	// public void HandleDisconnect(int id)
	// {
	// 	Node label = playerList.FindChild($"Label{id}");
	// 	if (label != null) label.QueueFree();
	// }

	public void HostGame()
	{
		Global.networkHandler.StartServer(ipAddressInputServer.Text);
        // Global.networkHandler.StartClient(ipAddressInputServer.Text);
        Global.networkHandler.StartClient("127.0.0.1");

		ChangeMenu("player");
	}

	// public void Local(int id)
	// {
	// 	AddPlayer(id);
	// }

	// public void Remote(int id)
	// {
	// 	AddPlayer(id);
	// }

	public void JoinGame()
	{
        Global.networkHandler.StartClient(ipAddressInputClient.Text);
		ChangeMenu("player");
	}

	// private void AddPlayer(int id)
	// {
	// 	Label playerLabel = new();
	// 	playerLabel.AddThemeFontSizeOverride("font_size", 60);
	// 	playerLabel.Text = $"Player:{id}";
	// 	playerLabel.Name = $"Label{id}";
	// 	playerList.AddChild(playerLabel);
	// }
	
}
