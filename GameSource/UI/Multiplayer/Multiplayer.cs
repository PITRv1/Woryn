using Godot;
using System;

public partial class Multiplayer : Control
{
	[Export] MainUI mainUI;
	[Export] Control multiTypeMenu;
	[Export] Control multiHostMenu;
	[Export] Control multiFindMenu;
	[Export] Control multiJoinMenu;
	[Export] Control playerListMenu;
	[Export] LineEdit gameNameText;
	[Export] HSlider numberOfPlayersValue;
    [Export] ButtonGroup optionGroup;
    [Export] LineEdit ipAddressInputClient;
	[Export] private LineEdit _gameNameInput;
	[Export] private VBoxContainer _gamesContainer;
	[Export] private PackedScene _joinableGamePlaque;


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

	private void RefreshGames()
	{
		foreach (var child in _gamesContainer.GetChildren())
		{
			child.QueueFree();
		}
		
		Global.networkHandler.LanDiscovery.StartClientDiscovery();
	}
	
	private void OnServerDiscovered(string ip, string serverName)
	{
		var serverItem = _joinableGamePlaque.Instantiate() as JoinableGamePlaque;
		serverItem.SetMenu(serverName, ip, 1, 4);
		serverItem.JoinPressed += () => JoinServer(ip);
        
		_gamesContainer.AddChild(serverItem);
	}
    
	private void JoinServer(string ip)
	{
		Global.networkHandler.LanDiscovery.StopClientDiscovery();
		Global.networkHandler.StartClient(ip);
		ChangeMenu("player");
	}


    public override void _Ready()
    {
		var ipAdresses = IP.GetLocalAddresses();

		var ipAddressOnLocalNetwork = ipAdresses[ipAdresses.Length-1];

        CurrentMenu = multiTypeMenu;

		ipAddressInputClient.Text = ipAddressOnLocalNetwork;
		
		Global.networkHandler.LanDiscovery.ServerDiscovered += OnServerDiscovered;
    }

	private void ChangeMenu(string option)
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
			case "find":
				CurrentMenu = multiFindMenu;
				RefreshGames();
				break;
			case "player":
				CurrentMenu = playerListMenu;
				break;
			case "select&stop":
				ChangeMenu("select");
				Global.networkHandler.DisconnectClient();
				if (Global.networkHandler.IsServer) Global.networkHandler.StopServer();
				break;
		}
	}

	private void HostGame()
	{
		Global.networkHandler.LanDiscovery.ServerName = _gameNameInput.Text;
		
		Global.networkHandler.StartServer();
		Global.networkHandler.StartClient();

		ChangeMenu("player");
	}

	private void JoinGame()
	{
        Global.networkHandler.StartClient(ipAddressInputClient.Text);
		ChangeMenu("player");
	}
	
}
